using Kai_Engine.ENGINE.Components;
using Kai_Engine.GAME.Management;
using Kai_Engine.ENGINE.Entities;
using Kai_Engine.ENGINE.Utils;
using System.Numerics;
using rlImGui_cs;
using Raylib_cs;
using ImGuiNET;
using System.Xml.Serialization;

namespace Kai_Engine.EDITOR
{
    internal class Kai_Editor
    {
        private readonly int _mouseBoundSize = 10;
        private readonly int _mouseOffset    = 4;

        private bool _showCollision = false;    
        private bool _selectObject  = false;

        private Color _mouseColor = Color.White;

        private Vector4 _selectedObjectTransform = new ();
        private Vector2 _colliderSize            = new (16,16);

        private GameObject? _selectedGameObject;


        ///######################################################################
        ///                           TODO:
        ///     - Have selection box stay on object when object is moved
        ///     - Keep reorganizing/optimizing editor code
        /// 
        ///######################################################################

        public void Init()
        {
            rlImGui.Setup(true);
            KaiLogger.Info("Initialized ImGUI", true);
        }

        public void Start(EntityManager eManager)
        {
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
            DrawSelected(new Vector2 (_selectedObjectTransform.X, _selectedObjectTransform.Y), new Vector2(_selectedObjectTransform.Z, _selectedObjectTransform.W));

            GameObject player = eManager.player;

            #region IMGUI
            rlImGui.Begin();

            //Editor Window
            if (ImGui.Begin("Kai Editor"))
            {
                ///######################################################################
                ///                           Check Boxes
                ///######################################################################
                ImGui.Checkbox("Select GameObjects", ref _selectObject);
                SeparatedSpacer();
                ImGui.Checkbox("Show Collision", ref _showCollision);
                SeparatedSpacer();
                ///######################################################################
                ///                             Player
                ///######################################################################
                if (player != null)
                {
                    ImGui.SeparatorText($"{player.Name.name}");
                    if (ImGui.TreeNode("Transform"))
                    {
                        ImGui.PushItemWidth(50); //Set input field size

                        ImGui.Text("Position");
                        ImGui.DragFloat("X", ref player.Transform.position.X);
                        ImGui.DragFloat("Y", ref player.Transform.position.Y);

                        ImGui.TreePop();
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
                        ImGui.DragFloat("X", ref _selectedGameObject.Transform.position.X);
                        ImGui.DragFloat("Y", ref _selectedGameObject.Transform.position.Y);

                        ImGui.TreePop();
                    }
                }
            }

            ImGui.End();
            rlImGui.End();
            #endregion
        }

        bool clicked = false;
        public void DetectMouse(EntityManager eManager)
        {
            foreach (var gameObject in eManager.AllObjects)
            {
                //Grab wall's components
                kTransform objectTransform = gameObject.GetComponent<kTransform>();
                kCollider objectCollider   = gameObject.GetComponent<kCollider>();

                //Determine wall's collider size
                Vector4 otherCol = objectCollider.ColliderSize(objectTransform, _colliderSize);

                //AABB collision check between Mouse Position and Wall's collider
                bool colliding = (int)GetMousePosition().X <= otherCol.X + otherCol.Z && (int)GetMousePosition().Z + (int)GetMousePosition().X >= otherCol.X
                              && (int)GetMousePosition().Y <= otherCol.Y + otherCol.W && (int)GetMousePosition().Y + (int)GetMousePosition().W >= otherCol.Y;

                //Set each wall's collision boolean to the results of 'colliding'
                objectCollider.isColliding = colliding;

                if (_selectObject && objectCollider.isColliding)
                {
                    //Do collision logic here
                    if (Raylib.IsMouseButtonPressed(0) && !clicked)
                    {
                        clicked = true;

                        //for outline drawing
                        _selectedObjectTransform = Selected(gameObject.Transform.position, gameObject.Transform.size);
                        
                        _selectedGameObject = gameObject;
                    }
                    else if (Raylib.IsMouseButtonReleased(0) && clicked)
                        clicked = false;

                    _mouseColor = Color.Red;
                }
            }
        }

        private void DrawObjectColliders(EntityManager eManager)
        {
            if (_showCollision)
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
            if (_selectObject)
            {
                Raylib.DrawRectangleLinesEx(new Rectangle((int)GetMousePosition().X, (int)GetMousePosition().Y,
                                                              (int)GetMousePosition().Z, (int)GetMousePosition().W), 2, _mouseColor);
            }
            _mouseColor = Color.White;
        }
        private void DrawSelected(Vector2 selectedObjectPosition, Vector2 selectedObjectSize)
        {
            if (_selectObject)
            {
                Raylib.DrawRectangleLinesEx(new Rectangle((int)selectedObjectPosition.X, (int)selectedObjectPosition.Y,
                                                          (int)selectedObjectSize.X, (int)selectedObjectSize.Y), 1, Color.White);
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
            return new Vector4 (selectedObjectPosition.X, selectedObjectPosition.Y, selectedObjectSize.X, selectedObjectSize.Y);
        }
        private Vector4 GetMousePosition()
        {
            return new(Raylib.GetMousePosition().X - _mouseOffset, Raylib.GetMousePosition().Y - _mouseOffset, _mouseBoundSize, _mouseBoundSize);
        }
    }
}
