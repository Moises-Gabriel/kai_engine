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

        private Color _color = new Color();

        internal GameObject? player;

        public void Init()
        {
            rlImGui.Setup(true);
            KaiLogger.Info("Initialized ImGUI", true);
        }

        public void Start(EntityManager eManager)
        {
            player = eManager.player;
        }

        public void Update(EntityManager eManager)
        {
            foreach (var wall in eManager.WallObjects)
            {
                kCollider? wallCollider = wall.GetComponent<kCollider>();

                if (wallCollider != null)
                {
                    MouseOver(wallCollider);
                }
            }
        }

        public void Draw()
        {
            Vector4 _mouseBounds = new Vector4(Raylib.GetMousePosition().X - 8, Raylib.GetMousePosition().Y - 8, 16, 16);

            if (selectObject)
            {
                Raylib.DrawRectangleLines((int)_mouseBounds.X, (int)_mouseBounds.Y, (int)_mouseBounds.Z, (int)_mouseBounds.W, _color);
            }
            
            rlImGui.Begin();

            //Editor Window
            if (ImGui.Begin("Kai Editor"))
            {
                ImGui.Checkbox("Select GameObjects", ref selectObject);

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

                        ImGui.Text("Size");
                        ImGui.DragFloat("W", ref player.Transform.size.X);
                        ImGui.DragFloat("H", ref player.Transform.size.Y);
                    }
                    ImGui.TreePop();
                }
                ImGui.Spacing();
                ImGui.SeparatorText("GameObject");
            }

            ImGui.End();
            rlImGui.End();

        }

        public bool AABBCollision(kCollider other)
        {
            Vector4 _mouseBounds = new Vector4(Raylib.GetMousePosition().X - 8, Raylib.GetMousePosition().Y - 8, 16, 16);
            Vector4 otherCol     = other.colliderSize;

            return _mouseBounds.X <= otherCol.X + otherCol.Z && _mouseBounds.Z + _mouseBounds.X >= otherCol.X
                && _mouseBounds.Y <= otherCol.Y + otherCol.W && _mouseBounds.Y + _mouseBounds.W >= otherCol.Y;
        }
        public void MouseOver(kCollider other)
        {
            if  (selectObject)
            {
                if (AABBCollision(other))
                    _color = Color.Red;
                else
                    _color = Color.White;
            }
        }
    }
}
