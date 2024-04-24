using Kai_Engine.ENGINE.UserInterface.UIObjects;
using System.Numerics;
using Raylib_cs;
using Kai_Engine.ENGINE.Utils;

namespace Kai_Engine.ENGINE.UserInterface
{
    internal class UIManager
    {
        ///########################################################################
        ///                                 TODO:
        ///                                 
        ///   UIObjects not rendering for some reason. 
        ///   Might have to do with adding a sprite as an additional 
        ///   component
        /// 
        ///########################################################################

        ///######################################################################
        ///                        Lists
        ///######################################################################
        public List<IUIObject> UIObjectInterfaces = new();
        public List<UIObject> UIObjects = new();

        ///######################################################################
        ///                        UI Object Variables
        ///######################################################################
        private string _basePath = Environment.CurrentDirectory;

        //Inventory
        private string? _inventorySpritePath;
        private Texture2D _inventorySpriteTexture = new();

        ///######################################################################
        ///                        Vectors
        ///######################################################################
        private Vector2 inventoryPosition = new();

        ///######################################################################
        ///                        Main
        ///######################################################################
        public void Init()
        {
            //Set sprite paths
            _inventorySpritePath = Path.Combine(_basePath, "GAME/Assets/inventory_sprite.png");

            //Initialize UI sprites
            _inventorySpriteTexture = Raylib.LoadTexture(_inventorySpritePath);

        }

        public void Start()
        {
            inventoryPosition = new Vector2(10, 10);

            Item(inventoryPosition, 3);
            ItemTwo(inventoryPosition, 3);
        }

        public void Update()
        {

        }

        public void Draw()
        {
            //UIGroup();
            //BlipGroup();
            foreach (var item in UIObjectInterfaces)
            {
                item.Draw();
            }
        }

        //Inventory display
        public void UIGroup()
        {
            UIBlock(new Vector2(inventoryPosition.X, inventoryPosition.Y), 2, 3);
            UIBlock(new Vector2(inventoryPosition.X + 50, inventoryPosition.Y), 2, 3);
            UIBlock(new Vector2(inventoryPosition.X + 100, inventoryPosition.Y), 2, 3);
            UIBlock(new Vector2(inventoryPosition.X + 150, inventoryPosition.Y), 2, 3);
        }

        public void BlipGroup()
        {
            //Shows selected item
            UIBlip(new Vector2(inventoryPosition.X + 5, 60), 1);
            UIBlip(new Vector2(inventoryPosition.X + 20, 60), 1);
            UIBlip(new Vector2(inventoryPosition.X + 35, 60), 1);
        }

        private void UIBlock(Vector2 position, int outlineThickness, int scale)
        {
            //Filled Rec
            Color fillColor = new Color(13, 16, 27, 255);
            Raylib.DrawRectangle((int)position.X, (int)position.Y, 16 * scale, 16 * scale, fillColor);

            //Rec lines
            Color outlineColor = new Color(175, 39, 71, 255);
            Rectangle outlineRec = new Rectangle(position, new Vector2(16 * scale, 16 * scale));
            Raylib.DrawRectangleLinesEx(outlineRec, outlineThickness, outlineColor);
        }

        private void UIBlip(Vector2 position, int scale)
        {
            //Filled Rec
            //Color fillColor = new Color(13, 16, 27, 255);
            Raylib.DrawRectangle((int)position.X, (int)position.Y, 4 * scale, 4 * scale, Color.RayWhite);
        }

        private void Item(Vector2 position, int scale)
        {
            UIObject inventoryBlock = new(position, scale, Layer.UI, true);

            UISprite sprite = new UISprite
            {
                texture = _inventorySpriteTexture,
                IsLoaded = true
            };

            inventoryBlock.AddComponent(sprite);

            UIObjects.Add(inventoryBlock);
            UIObjectInterfaces.Add(inventoryBlock);

            KaiLogger.Info("Sprite Loaded: " + sprite.IsLoaded.ToString(), false);
        }

        private void ItemTwo(Vector2 position, int scale)
        {
            UIObject inventoryBlock = new(position, scale, Layer.UI, true);

            UISprite sprite = new UISprite
            {
                texture = _inventorySpriteTexture,
                IsLoaded = true
            };

            inventoryBlock.AddComponent(sprite);

            UIObjects.Add(inventoryBlock);
            UIObjectInterfaces.Add(inventoryBlock);

            KaiLogger.Info("Sprite Loaded: " + sprite.IsLoaded.ToString(), false);
        }
    }
}