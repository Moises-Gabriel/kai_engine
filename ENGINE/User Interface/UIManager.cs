using Kai_Engine.ENGINE.UserInterface.UIObjects;
using System.Numerics;
using Raylib_cs;

namespace Kai_Engine.ENGINE.UserInterface
{
    internal class UIManager
    {
        private string _basePath = Environment.CurrentDirectory;

        //List of all UI in the game
        public List<IUIObject> uIObjects = new();

        //UI Object       
        private string? _testSpritePath;
        private Texture2D _testSpriteTex = new();

        public void Init()
        {
            _testSpritePath = Path.Combine(_basePath, "GAME/Assets/test_sprite.png");
            _testSpriteTex = Raylib.LoadTexture(_testSpritePath);

        }

        public void Start()
        {
            CreateUI(Vector2.Zero);
        }

        public void Update()
        {

        }

        public void Draw()
        {
            foreach (var ui in uIObjects)
            {
                ui.Draw();
            }
        }

        public void CreateUI(Vector2 position)
        {
            UIObject test = new(_testSpriteTex, position, new Vector2(32, 32), Layer.UI, true);

            uIObjects.Add(test);
        }
    }


}