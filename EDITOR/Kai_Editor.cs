using ImGuiNET;
using Kai_Engine.ENGINE.Components;
using Kai_Engine.ENGINE.Entities;
using Kai_Engine.ENGINE.Utils;
using Kai_Engine.GAME.Management;
using Raylib_cs;
using rlImGui_cs;
using System.Numerics;

namespace Kai_Engine.EDITOR
{
    internal class Kai_Editor
    {
        private readonly int _mouseBoundSize = 10;
        private readonly int _mouseOffset = 4;

        private bool _showCollision = false;
        private bool _selectObject = false;
        private bool _showViewport = true;

        private Color _mouseColor = Color.White;

        private Vector4 _selectedObjectTransform = new();
        private Vector2 _colliderSize = new(16, 16);

        private GameObject? _selectedGameObject;

        internal GameObject? player;

        ///########################################################################
        ///                        TODO: KAI EDITOR
        ///                        
        ///   - Debug doesn't align with camera view, only world view; [pending]
        ///     - Going to try fixing the camera system first before messing
        ///       with debug
        /// 
        ///########################################################################

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

        bool debugOpen = false;
        public void Update(EntityManager eManager)
        {
            DetectMouse(eManager);

            if (Raylib.IsKeyPressed(KeyboardKey.Tab) && !debugOpen)
            {
                KaiLogger.Info("Debug Open", false);
                debugOpen = true;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Tab) && debugOpen)
            {
                KaiLogger.Info("Debug Closed", false);
                debugOpen = false;
            }
        }

        public void Draw(EntityManager eManager)
        {
            DrawMouseCollider();
            DrawObjectColliders(eManager);
            DrawSelectionBox(new Vector2(_selectedObjectTransform.X, _selectedObjectTransform.Y), new Vector2(_selectedObjectTransform.Z, _selectedObjectTransform.W));
            DrawCameraViewport(eManager);

            if (debugOpen)
            {
                DrawGUI(eManager);
            }
        }

        private void DrawGUI(EntityManager eManager)
        {
            rlImGui.Begin();

            //Editor Window
            if (ImGui.Begin("Kai Editor"))
            {
                ///######################################################################
                ///                             Camera
                ///######################################################################
                if (eManager.Camera != null)
                {
                    ImGui.SeparatorText("Camera");
                    ImGui.Checkbox("Show Viewport", ref _showViewport);
                }
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
                        ImGui.Text($"Health: {_selectedGameObject.GetComponent<kHealth>().health}");
                    }
                }
            }

            ImGui.End();
            rlImGui.End();
        }

        bool clicked = false; //Determines if mouse button has been clicked (extra layer)
        public void DetectMouse(EntityManager eManager)
        {
            foreach (var gameObject in eManager.AllObjects)
            {
                if (gameObject.IsActive)
                {
                    //Grab wall's components
                    kTransform objectTransform = gameObject.GetComponent<kTransform>();
                    kCollider objectCollider = gameObject.GetComponent<kCollider>();

                    //Determine wall's collider size
                    Vector4 otherCol = objectCollider.ColliderSize(objectTransform, _colliderSize);

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

        private void DrawObjectColliders(EntityManager eManager)
        {
            if (_showCollision)
            {
                if (eManager.player != null)
                {
                    kCollider? playerCollider = eManager.player.GetComponent<kCollider>();

                    if (playerCollider.IsColliding)
                    {
                        playerCollider.DrawBounds(eManager.player.Transform, new Vector2(16, 16), Color.Red);
                        Raylib.DrawRectangle((int)eManager.player.Transform.position.X, (int)eManager.player.Transform.position.Y,
                                             (int)eManager.player.Transform.size.X, (int)eManager.player.Transform.size.Y, Color.Red);
                    }
                    else
                    {
                        playerCollider.DrawBounds(eManager.player.Transform, new Vector2(16, 16), Color.Green);
                        Raylib.DrawRectangle((int)eManager.player.Transform.position.X, (int)eManager.player.Transform.position.Y,
                                             (int)eManager.player.Transform.size.X, (int)eManager.player.Transform.size.Y, Color.Green);
                    }
                }

                foreach (var wall in eManager.WallObjects)
                {
                    //Display collision box for each wall
                    if (wall.IsActive)
                    {
                        wall.GetComponent<kCollider>().DrawBounds(wall.GetComponent<kTransform>(), new Vector2(16, 16), Color.Green);
                    }

                }

                foreach (var item in eManager.ItemObjects)
                {
                    if (item.IsActive)
                    {
                        //Display collision box for each wall
                        item.GetComponent<kCollider>().DrawBounds(item.GetComponent<kTransform>(), new Vector2(16, 16), Color.Green);
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
        private void DrawSelectionBox(Vector2 selectedObjectPosition, Vector2 selectedObjectSize)
        {
            Raylib.DrawRectangleLinesEx(new Rectangle((int)selectedObjectPosition.X, (int)selectedObjectPosition.Y,
                                                      (int)selectedObjectSize.X, (int)selectedObjectSize.Y), 1, Color.White);
        }

        private void DrawCameraViewport(EntityManager eManager)
        {
            //Camera dimensions
            Vector2 cameraPosition = new Vector2((int)eManager.Camera.Position.X, (int)eManager.Camera.Position.Y);
            Vector2 cameraSize = new Vector2((int)eManager.Camera.Viewport.Width, (int)eManager.Camera.Viewport.Height);

            //Camera rectangle
            Rectangle camera = new Rectangle(cameraPosition.X, cameraPosition.Y, cameraSize.X, cameraSize.Y);

            //Deadzone dimensions
            Vector2 deadZonePosition = new Vector2((int)eManager.Camera.DeadZone.X, (int)eManager.Camera.DeadZone.Y);
            Vector2 deadZoneSize = new Vector2((int)eManager.Camera.DeadZone.Width, (int)eManager.Camera.DeadZone.Height);

            //Deadzone rectangle
            Rectangle deadZone = new Rectangle(deadZonePosition.X, deadZonePosition.Y, deadZoneSize.X, deadZoneSize.Y);

            if (_showViewport)
            {
                //Total viewport
                Raylib.DrawRectangleLinesEx(camera, 2, Color.Red);

                //Deadzone
                Raylib.DrawRectangleLinesEx(deadZone, 2, Color.Green);
            }
        }
        private void SeparatedSpacer()
        {
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
        }
        private Vector4 Selected(Vector2 selectedObjectPosition, Vector2 selectedObjectSize)
        {
            return new Vector4(selectedObjectPosition.X, selectedObjectPosition.Y, selectedObjectSize.X, selectedObjectSize.Y);
        }
        private Vector4 GetMousePosition()
        {
            return new(Raylib.GetMousePosition().X - _mouseOffset, Raylib.GetMousePosition().Y - _mouseOffset, _mouseBoundSize, _mouseBoundSize);
        }
    }
}
