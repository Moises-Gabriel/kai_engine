using Kai_Engine.ENGINE.Components;
using Kai_Engine.GAME.Management;
using Kai_Engine.ENGINE.Entities;
using Kai_Engine.ENGINE.Utils;
using Kai_Engine.ENGINE;
using System.Numerics;
using rlImGui_cs;
using Raylib_cs;
using ImGuiNET;

namespace Kai_Engine.EDITOR
{
    internal class Kai_Editor
    {
        ///######################################################################
        ///                                 FIXME:
        ///                                 
        ///   - When using RenderTexture mode, mouse selection doesn't work due
        ///   to discrepancy in mouse position to object position
        /// 
        ///######################################################################


        ///######################################################################
        ///                        Mouse Variables
        ///######################################################################
        private readonly int _mouseBoundSize = 10;
        private readonly int _mouseOffset = 4;
        private Color _mouseColor = Color.White;

        ///######################################################################
        ///                        Booleans
        ///######################################################################     
        private bool _showCollision = false;
        private bool _selectObject = false;
        private bool _showDeadZone = false;
        public bool DebugOpen = false;

        ///######################################################################
        ///                        Vectors
        ///######################################################################
        private Vector4 _selectedObjectTransform = new();
        private Vector2 _colliderSize = new(16, 16);

        ///######################################################################
        ///                        GameObjects
        ///######################################################################
        private GameObject? _selectedGameObject;
        private GameObject? player;


        ///######################################################################
        ///                        Main
        ///######################################################################
        public void Init()
        {
            rlImGui.Setup(true);
            KaiLogger.Info("Initialized ImGUI", true);
        }

        public void Start(EntityManager eManager)
        {
            player = eManager.player;
            KaiLogger.Important("Kai Editor: Ready", true);
        }

        public void Update(EntityManager eManager)
        {
            DetectMouse(eManager);

            if (Raylib.IsKeyPressed(KeyboardKey.Tab) && !DebugOpen)
            {
                KaiLogger.Info("Debug Open", false);
                DebugOpen = true;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Tab) && DebugOpen)
            {
                KaiLogger.Info("Debug Closed", false);
                DebugOpen = false;
            }
        }

        public void Draw(EntityManager eManager)
        {
            DrawMouseCollider();
            DrawObjectColliders(eManager);
            DrawSelectionBox(new Vector2(_selectedObjectTransform.X, _selectedObjectTransform.Y),
                             new Vector2(_selectedObjectTransform.Z, _selectedObjectTransform.W), eManager);
            DrawCameraDeadzone(eManager, new Vector2(Program.MapWidth, Program.MapHeight));
        }

        ///######################################################################
        ///                            Drawing
        ///######################################################################
        public void DrawGUI(EntityManager eManager)
        {
            Raylib.DrawFPS(Program.ScreenWidth - 80, Program.ScreenHeight / 16);
            //Raylib.DrawText(Raylib.GetFPS().ToString, Program.ScreenWidth - 80, Program.ScreenHeight / 16, 25, Color.Red);

            rlImGui.Begin();

            //Editor Window
            if (ImGui.Begin("Kai Debug"))
            {
                ///######################################################################
                ///                             Rendering
                ///######################################################################
                ImGui.SeparatorText("Rendering Mode");
                ImGui.Checkbox("Render Texture", ref Program.TextureMode);
                SeparatedSpacer();

                ///######################################################################
                ///                             Camera
                ///######################################################################

                ImGui.SeparatorText("Camera");
                ImGui.Checkbox("Show Deadzone", ref _showDeadZone);
                ImGui.PushItemWidth(50); //Set input field size
                ImGui.InputInt2("DeadZone Scale", ref eManager.Camera.DeadZoneScale.X); //not sure why this still affects the Y axis lol
                ImGui.PushItemWidth(50); //Set input field size
                ImGui.InputFloat("Zoom", ref eManager.Camera.RayCamera.Zoom);
                SeparatedSpacer();

                ///######################################################################
                ///                           Check Boxes
                ///######################################################################

                ImGui.Checkbox("Select GameObjects", ref _selectObject);
                ImGui.Checkbox("Show Colliders", ref _showCollision);
                SeparatedSpacer();

                ///######################################################################
                ///                             Player
                ///######################################################################

                if (player != null)
                {
                    if (player.GetComponent<kHealth>() != null)
                    {
                        kHealth? playerHealth = player.GetComponent<kHealth>();
                        ImGui.SeparatorText($"{player.Name.name}");
                        if (ImGui.TreeNode("Transform"))
                        {
                            ImGui.PushItemWidth(50); //Set input field size

                            ImGui.Text("Position");
                            ImGui.DragFloat("X", ref player.Transform.position.X, 17);
                            ImGui.DragFloat("Y", ref player.Transform.position.Y, 17);

                            ImGui.TreePop();
                        }
                        if (playerHealth != null)
                            ImGui.Text($"Health: {playerHealth.health}");
                    }
                }

                ///######################################################################
                ///                        Selected Object
                ///######################################################################

                if (_selectedGameObject != null)
                {
                    ImGui.SeparatorText($"{_selectedGameObject.Name.name}");
                    if (ImGui.TreeNode("Object Transform"))
                    {
                        ImGui.PushItemWidth(50); //Set input field size

                        ImGui.Text("Position");
                        ImGui.DragFloat("X", ref _selectedGameObject.Transform.position.X, 17);
                        ImGui.DragFloat("Y", ref _selectedGameObject.Transform.position.Y, 17);

                        ImGui.TreePop();
                    }
                    if (_selectedGameObject.GetComponent<kHealth>() != null)
                    {
                        kHealth? selectedObjectHealth = _selectedGameObject.GetComponent<kHealth>();
                        if (selectedObjectHealth != null)
                            ImGui.Text($"Health: {selectedObjectHealth.health}");
                    }
                }
            }

            ImGui.End();
            rlImGui.End();
        }
        private void DrawObjectColliders(EntityManager eManager)
        {
            Vector2 objectSize = new Vector2(16 * eManager.Camera.RayCamera.Zoom, 16 * eManager.Camera.RayCamera.Zoom);
            if (_showCollision)
            {
                //Display collision box for player
                if (eManager.player != null)
                {
                    kCollider? playerCollider = eManager.player.GetComponent<kCollider>();
                    Vector2 playerWorldPos = new Vector2(eManager.player.Transform.position.X, eManager.player.Transform.position.Y);
                    if (playerCollider != null && playerCollider.IsColliding)
                    {
                        playerCollider.DrawBounds(KaiMath.WorldToScreen(playerWorldPos, eManager.Camera.RayCamera), objectSize, Color.Red);
                        Raylib.DrawRectangle((int)KaiMath.WorldToScreen(playerWorldPos, eManager.Camera.RayCamera).X, (int)KaiMath.WorldToScreen(playerWorldPos, eManager.Camera.RayCamera).Y,
                                             (int)objectSize.X, (int)objectSize.Y, Color.Red);
                    }
                    else
                    {
                        playerCollider.DrawBounds(KaiMath.WorldToScreen(playerWorldPos, eManager.Camera.RayCamera), objectSize, Color.Green);
                        Raylib.DrawRectangle((int)KaiMath.WorldToScreen(playerWorldPos, eManager.Camera.RayCamera).X, (int)KaiMath.WorldToScreen(playerWorldPos, eManager.Camera.RayCamera).Y,
                                             (int)objectSize.X, (int)objectSize.Y, Color.Green);
                    }
                }

                //Display collision box for each wall
                foreach (var wall in eManager.WallObjects)
                {
                    if (wall.IsActive)
                    {
                        Vector2 wallWorldPos = new Vector2(wall.GetComponent<kTransform>().position.X, wall.GetComponent<kTransform>().position.Y);
                        wall.GetComponent<kCollider>().DrawBounds(KaiMath.WorldToScreen(wallWorldPos, eManager.Camera.RayCamera), objectSize, Color.Green);
                    }

                }

                //Display collision box for each item
                foreach (var item in eManager.ItemObjects)
                {
                    //Display collision box for each wall
                    if (item.IsActive)
                    {
                        Vector2 itemWorldPos = new Vector2(item.GetComponent<kTransform>().position.X, item.GetComponent<kTransform>().position.Y);
                        item.GetComponent<kCollider>().DrawBounds(KaiMath.WorldToScreen(itemWorldPos, eManager.Camera.RayCamera), objectSize, Color.Green);
                    }
                }
            }
        }
        private void DrawMouseCollider()
        {
            if (_selectObject)
            {
                Raylib.DrawRectangleLinesEx(new Rectangle((int)GetMousePosition().X, (int)GetMousePosition().Y,
                                                          (int)GetMousePosition().Z, (int)GetMousePosition().W), 2, _mouseColor);
            }
            _mouseColor = Color.White;
        }
        private void DrawSelectionBox(Vector2 selectedObjectPosition, Vector2 selectedObjectSize, EntityManager eManager)
        {
            Vector2 selectedObjectWorldPos = new Vector2((int)selectedObjectPosition.X, (int)selectedObjectPosition.Y);
            Raylib.DrawRectangleLinesEx(new Rectangle(KaiMath.WorldToScreen(selectedObjectWorldPos, eManager.Camera.RayCamera).X,
                                                      KaiMath.WorldToScreen(selectedObjectWorldPos, eManager.Camera.RayCamera).Y,
                                                      (int)selectedObjectSize.X * eManager.Camera.RayCamera.Zoom, (int)selectedObjectSize.Y * eManager.Camera.RayCamera.Zoom), 1, Color.White);
        }
        private void DrawCameraDeadzone(EntityManager eManager, Vector2 screenSize)
        {
            // Define the deadzone in world space, not screen space
            int deadZoneWidth = (int)screenSize.X / eManager.Camera.DeadZoneScale.X;
            int deadZoneHeight = (int)screenSize.Y / eManager.Camera.DeadZoneScale.Y;

            // Adjust the deadzone position relative to the camera target
            Vector2 deadZonePos = new Vector2
            (
                eManager.Camera.RayCamera.Target.X - deadZoneWidth / 2,
                eManager.Camera.RayCamera.Target.Y - deadZoneHeight / 2
            );

            // Convert the deadzone position from world space to screen space
            Vector2 deadZoneScreenPos = KaiMath.WorldToScreen(deadZonePos, eManager.Camera.RayCamera);

            // Calculate the deadzone rectangle in screen space
            Rectangle deadZone = new Rectangle
            (
                deadZoneScreenPos.X,
                deadZoneScreenPos.Y,
                deadZoneWidth * eManager.Camera.RayCamera.Zoom,  // Scale width by the Camera.RayCamera's zoom level
                deadZoneHeight * eManager.Camera.RayCamera.Zoom  // Scale height by the Camera.RayCamera's zoom level
            );

            if (_showDeadZone)
            {
                // Draw the deadzone rectangle on the screen
                Raylib.DrawRectangleLinesEx(deadZone, 1, Color.Red);
            }
        }

        ///######################################################################
        ///                             Utilities
        ///######################################################################
        bool clicked = false; //Determines if mouse button has been clicked (extra layer)
        public void DetectMouse(EntityManager eManager)
        {
            foreach (var gameObject in eManager.AllObjects)
            {
                if (gameObject != null && gameObject.IsActive)
                {
                    //Grab wall's components
                    kTransform? objectTransform = gameObject.GetComponent<kTransform>();
                    kCollider? objectCollider = gameObject.GetComponent<kCollider>();

                    //Determine wall's collider size
                    Vector2 otherWorldPos = new Vector2(objectTransform.position.X, objectTransform.position.Y);
                    Vector4 otherCol = objectCollider.ColliderSize(KaiMath.WorldToScreen(otherWorldPos, eManager.Camera.RayCamera), _colliderSize * eManager.Camera.RayCamera.Zoom);

                    //AABB collision check between Mouse Position and Wall's collider
                    bool colliding = (int)GetMousePosition().X <= otherCol.X + otherCol.Z && (int)GetMousePosition().Z + (int)GetMousePosition().X >= otherCol.X
                                  && (int)GetMousePosition().Y <= otherCol.Y + otherCol.W && (int)GetMousePosition().Y + (int)GetMousePosition().W >= otherCol.Y;

                    //Set each wall's collision boolean to the results of 'colliding'
                    objectCollider.IsColliding = colliding;

                    if (_selectObject && objectCollider.IsColliding)
                    {
                        //Do collision logic here
                        if (Raylib.IsMouseButtonPressed(0) && !clicked)
                        {
                            clicked = true;

                            //TODO: Figure out where to put this so that the outline updates with the object
                            //for outline drawing
                            _selectedObjectTransform = Selected(gameObject.Transform.position, gameObject.Transform.size);

                            _selectedGameObject = gameObject;
                        }
                        else if (Raylib.IsMouseButtonReleased(0) && clicked)
                        {
                            clicked = false;
                        }

                        _mouseColor = Color.Red;
                    }
                }
            }
        }
        private Vector4 Selected(Vector2 selectedObjectPosition, Vector2 selectedObjectSize)
        {
            return new Vector4(selectedObjectPosition.X, selectedObjectPosition.Y, selectedObjectSize.X, selectedObjectSize.Y);
        }
        private Vector4 GetMousePosition()
        {
            return new(Raylib.GetMousePosition().X - _mouseOffset, Raylib.GetMousePosition().Y - _mouseOffset, _mouseBoundSize, _mouseBoundSize);
        }
        private void SeparatedSpacer()
        {
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
        }
    }
}
