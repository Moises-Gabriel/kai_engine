using Kai_Engine.ENGINE.UserInterface;
using Kai_Engine.GAME.Management;
using Kai_Engine.ENGINE.Utils;
using System.Globalization;
using Kai_Engine.EDITOR;
using Raylib_cs;

namespace Kai_Engine.ENGINE
{
    internal class Program
    {
        private const string _engineName = "Kai";
        private const string _engineVersion = "0.0.1";
        private const string _gameName = "Crawl";

        internal static int ScreenWidth = 1280;
        internal static int ScreenHeight = 720;

        internal static int MapWidth = 2560;
        internal static int MapHeight = 1440;

        internal static int cellSize = 16; //update this to match sprite size

        internal static Color clearColor = new Color(28, 22, 19, 255);

        //Debug Editor
        private const bool _editable = true;

        static void Main()
        {
            KaiLogger.Important("Program", $"{_engineName}: [v:{_engineVersion}]", false);
            KaiLogger.Important("Program", $"Game: {_gameName}", false);

            Raylib.InitWindow(ScreenWidth, ScreenHeight, _engineName);
            Raylib.SetTargetFPS(60);

            Game game = new Game();
            game.Init();
            game.Start();

            KaiLogger.Important("Program", "Starting Game Loop", false);
            while (!Raylib.WindowShouldClose())
            {
                game.Update();

                Raylib.BeginDrawing();

                game.Draw();
                game.UIDraw();

                Raylib.EndDrawing();
            }

            //Cleanup
            Raylib.CloseWindow();
        }
    }
}


