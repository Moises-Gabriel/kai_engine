using Raylib_cs;

namespace Kai_Engine.ENGINE.Utils
{
    public static class AssetLoader
    {
        private static string workingDirectory = Environment.CurrentDirectory;
       
        //Sprite Sheet
        public static string? _spriteSheetPath = "";
        public static Texture2D _spriteSheet = new();

        //UI Sprites
        public static string? _inventorySpritePath;
        public static Texture2D _inventorySpriteTexture = new();

        public static void Load()
        {
            //Set path & Initialize spritesheet
            _spriteSheetPath = Path.Combine(workingDirectory, "GAME/Assets/sprite_sheet.png");
            _spriteSheet = Raylib.LoadTexture(_spriteSheetPath);

            //Set path & Initialize UI sprites
            _inventorySpritePath = Path.Combine(workingDirectory, "GAME/Assets/inventory_sprite.png");
            _inventorySpriteTexture = Raylib.LoadTexture(_inventorySpritePath);
        }
    }
}