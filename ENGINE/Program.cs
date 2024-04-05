using Kai_Engine.EDITOR;
using Kai_Engine.ENGINE.Utils;
using Kai_Engine.GAME.Management;
using Raylib_cs;

namespace Kai_Engine.ENGINE
{
    internal class Program
    {
        private const string _engineName = "Kai";
        private const string _engineVersion = "0.0.1";
        private const string _gameName = "Down & Down";

        internal static int MapWidth = 960;
        internal static int MapHeight = 540;

        static void Main()
        {
            EntityManager entityManager = new();
            Kai_Editor kaiEditor = new();

            //Engine Info
            KaiLogger.Important($"{_engineName}: [v{_engineVersion}]", false);
            KaiLogger.Important($"Game: {_gameName}", false);

            //------Initialize------
            Raylib.InitWindow(MapWidth, MapHeight, _engineName);
            kaiEditor.Init();

            //------Start------
            entityManager.Start();
            kaiEditor.Start(entityManager);

            //Main Game Loop
            while (!Raylib.WindowShouldClose())
            {

                //------Update------
                entityManager.Update();
                kaiEditor.Update(entityManager);

                //-------Draw------
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                entityManager.Draw();
                kaiEditor.Draw();

                Raylib.EndDrawing();
            }

            //Cleanup
            Raylib.CloseWindow();
        }
    }
}
