using Kai_Engine.GAME.Management;
using Kai_Engine.ENGINE.Entities;
using Kai_Engine.ENGINE.Utils;
using rlImGui_cs;
using Raylib_cs;
using ImGuiNET;

namespace Kai_Engine.EDITOR
{
    internal class Kai_Editor
    {
        internal bool selectObject = false;

        internal GameObject? player;

        public void Init()
        {
            //Initialize ImGUI
            rlImGui.Setup(true);
            KaiLogger.Info("Initialized ImGUI", true);
        }

        public void Start(EntityManager eManager)
        {
            player = eManager.player;
        }

        public void Update()
        {

        }

        public void Draw()
        {
            if (selectObject)
                Raylib.DrawRectangleLines((int)Raylib.GetMousePosition().X - 4, (int)Raylib.GetMousePosition().Y - 4, 8, 8, Color.White);;
            
            rlImGui.Begin();

            //Editor Window
            if (ImGui.Begin("Kai Editor"))
            {
                ImGui.Checkbox("Select GameObjects", ref selectObject);
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

                //Sprite Dropdown
                if (ImGui.TreeNode("Sprite"))
                {
                    //Empty for now
                    ImGui.Button("Boop");
                    ImGui.TreePop();
                }

                ImGui.SeparatorText("GameObject");
                
            }

            ImGui.End();
            rlImGui.End();

        }
    }
}
