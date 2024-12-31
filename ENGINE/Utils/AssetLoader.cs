using Raylib_cs;

namespace Kai_Engine.ENGINE.Utils
{
    public static class AssetLoader
    {
        private static string workingDirectory = Environment.CurrentDirectory;

        //Sprite Sheet
        public static string? SpriteSheetPath = "";
        public static Texture2D SpriteSheet = new();

        //UI Sprites
        public static string? InventorySpritePath;
        public static Texture2D InventorySpriteTexture = new();

        public static void Load()
        {
            //Game Assets
            SpriteSheetPath = Path.Combine(workingDirectory, "GAME/Assets/sprite_sheet.png");
            SpriteSheet = Raylib.LoadTexture(SpriteSheetPath);

            //UI Assets
            InventorySpritePath = Path.Combine(workingDirectory, "GAME/Assets/inventory_sprite.png");
            InventorySpriteTexture = Raylib.LoadTexture(InventorySpritePath);
        }
    }
}