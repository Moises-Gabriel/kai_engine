using Kai_Engine.ENGINE.Components;
using Kai_Engine.GAME.Management;
using Kai_Engine.ENGINE.Entities;
using Kai_Engine.ENGINE.Utils;
using System.Numerics;
using rlImGui_cs;
using Raylib_cs;
using ImGuiNET;

namespace Kai_Engine.EDITOR
{
    internal class Kai_Editor
    {
        internal bool selectObject = false;
        internal bool showCollision = false;    

        private readonly int _mouseBoundSize = 10;
        private readonly int _mouseOffset = 4;
        private Color _mouseColor = Color.White;

        private Vector2 _colliderSize = new (16,16);
        private Vector4 _selectedObject = new ();

        internal GameObject? player;

        public void Init()
        {
            rlImGui.Setup(true);
            KaiLogger.Info("Initialized ImGUI", true);
        }

        public void Start(EntityManager eManager)
        {
            player = eManager.player;
            KaiLogger.Info("Walls in scene: " + eManager.WallObjects.Count.ToString(), false);
        }

        public void Update(EntityManager eManager)
        {
            DetectMouse(eManager);
        }

        public void Draw(EntityManager eManager)
        {
            DrawMouseCollider();
            DrawObjectColliders(eManager);
            DrawSelected(Selected(_selectedObject));

            #region IMGUI
            rlImGui.Begin();

            //Editor Window
            if (ImGui.Begin("Kai Editor"))
            {
                ImGui.Checkbox("Select GameObjects", ref selectObject);
                ImGui.Spacing();
                ImGui.Separator();
                ImGui.Spacing();
                ImGui.Checkbox("Show Collision", ref showCollision);

                ImGui.Spacing();
                ImGui.Separator();
                ImGui.Spacing();
                ImGui.SeparatorText($"{player.Name.name}");
                //Transform Dropdown
                if (ImGui.TreeNode("Transform"))
                {
                    if (player != null)
                    {
                        ImGui.PushItemWidth(50); //Set input field size

                        ImGui.Text("Position");
                        ImGui.DragFloat("X", ref player.Transform.position.X);
                        ImGui.DragFloat("Y", ref player.Transform.position.Y);

                        //TODO: Allow scaling for collider size
                        ImGui.Text("Size");
                        ImGui.DragFloat("W", ref _colliderSize.X);
                        ImGui.DragFloat("H", ref _colliderSize.Y);
                    }
                    ImGui.TreePop();
                }
                ImGui.Spacing();
                ImGui.SeparatorText("GameObject");
            }

            ImGui.End();
            rlImGui.End();
            #endregion
        }

        public void DetectMouse(EntityManager eManager)
        {
            foreach (var wall in eManager.WallObjects)
            {
                kTransform wallTransform = wall.GetComponent<kTransform>();
                kCollider wallCollider = wall.GetComponent<kCollider>();

                Vector4 otherCol = wallCollider.ColliderSize(wallTransform, _colliderSize);

                bool colliding = (int)GetMousePosition().X <= otherCol.X + otherCol.Z && (int)GetMousePosition().Z + (int)GetMousePosition().X >= otherCol.X
                              && (int)GetMousePosition().Y <= otherCol.Y + otherCol.W && (int)GetMousePosition().Y + (int)GetMousePosition().W >= otherCol.Y;

                wallCollider.isColliding = colliding;
                GameObject selectedObject = wallCollider.gameObject;

                if (selectObject)
                {
                    if (wallCollider.isColliding)
                    {
                        //Do collision logic here
                        if (Raylib.IsMouseButtonPressed(0))
                        {
                            _selectedObject = Selected(otherCol);
                            //TODO: Go into the entity manager and figure out why the name isn't coming out right (ie Wall_4999)
                            KaiLogger.Info($"Selected Wall's Data: {selectedObject.Name.name}", false);
                        }
                        _mouseColor = Color.Red;
                    }
                }
            }
        }

        public Vector4 Selected(Vector4 selectedObject)
        {
            return selectedObject;
        }

        private void DrawObjectColliders(EntityManager eManager)
        {
            if (showCollision)
            {
                foreach (var wall in eManager.WallObjects)
                {
                    //Display collision box for each wall
                    wall.GetComponent<kCollider>().DrawBounds(wall.GetComponent<kTransform>(), new Vector2(16, 16), Color.Green);

                }
            }
        }
        
        private void DrawMouseCollider()
        {
            if (selectObject)
            {
                Raylib.DrawRectangleLinesEx(new Rectangle((int)GetMousePosition().X, (int)GetMousePosition().Y,
                                                              (int)GetMousePosition().Z, (int)GetMousePosition().W), 2, _mouseColor);
            }
            _mouseColor = Color.White;
        }

        private void DrawSelected(Vector4 selectedObject)
        {
            Raylib.DrawRectangleLinesEx(new Rectangle((int)selectedObject.X, (int)selectedObject.Y,
                                                      (int)selectedObject.Z, (int)selectedObject.W), 1, Color.White);
        }
        private Vector4 GetMousePosition()
        {
            return new(Raylib.GetMousePosition().X - _mouseOffset, Raylib.GetMousePosition().Y - _mouseOffset, _mouseBoundSize, _mouseBoundSize);
        }
    }
}
