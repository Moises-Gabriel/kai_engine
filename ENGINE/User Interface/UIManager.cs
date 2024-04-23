using Kai_Engine.ENGINE.UserInterface.UIObjects;
using System.Numerics;
using Raylib_cs;

namespace Kai_Engine.ENGINE.UserInterface
{
    internal class UIManager
    {
        //File directory
        private string _basePath = Environment.CurrentDirectory;

        //List of all UI in the game
        public List<IUIObject> UIObjects = new();

        //Inventory icons
        private string? _inventorySpritePath;
        private Texture2D _inventorySprite = new();

        public void Init()
        {
            //Set sprite paths
            _inventorySpritePath = Path.Combine(_basePath, "GAME/Assets/inventory_sprite.png");

            //Initialize UI sprites
            _inventorySprite = Raylib.LoadTexture(_inventorySpritePath);

        }

        public void Start()
        {
            ItemUI(new Vector2(60, 10), 3);
        }

        public void Update()
        {

        }

        public void Draw()
        {
            UIGroup();

            foreach (var ui in UIObjects)
            {
                ui.Draw();
            }
        }

        //Inventory display
        public void UIGroup()
        {
            UIBlock(new Vector2(10, 10), 2, 3);
            UIBlock(new Vector2(60, 10), 2, 3);
            UIBlock(new Vector2(110, 10), 2, 3);
            UIBlock(new Vector2(160, 10), 2, 3);
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

        private void ItemUI(Vector2 position, int scale)
        {
            UIObject inventoryBlock = new(_inventorySprite, position, scale, Layer.UI, true);
            UIObjects.Add(inventoryBlock);
        }
    }
}