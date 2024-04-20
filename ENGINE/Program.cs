using Kai_Engine.GAME.Management;
using Kai_Engine.ENGINE.Utils;
using Kai_Engine.EDITOR;
using Raylib_cs;

namespace Kai_Engine.ENGINE
{
    internal class Program
    {
        private const string _engineName = "Kai";
        private const string _engineVersion = "0.0.1";
        private const string _gameName = "Down & Down";

        internal static int MapWidth = 1280;
        internal static int MapHeight = 720;

        //Editor
        private const bool _editable = true;

        static void Main()
        {
            //Engine Info
            KaiLogger.Important($"{_engineName}: [v{_engineVersion}]", false);
            KaiLogger.Important($"Game: {_gameName}", false);

            //------Initialize------
            EntityManager entityManager = new();
            Kai_Editor kaiEditor = new();

            Raylib.InitWindow(MapWidth, MapHeight, _engineName);
            Raylib.SetTargetFPS(120);

            if (_editable)
                kaiEditor.Init();

            entityManager.Init();

            //------Start------
            entityManager.Start();
            if (_editable)
                kaiEditor.Start(entityManager);

            //Main Game Loop
            while (!Raylib.WindowShouldClose())
            {

                //------Update------
                entityManager.Update();
                if (_editable)
                    kaiEditor.Update(entityManager);

                //-------Draw------
                Raylib.BeginDrawing();
                Raylib.BeginMode2D(entityManager.Camera.RayCamera);

                Raylib.ClearBackground(Color.Black);
                entityManager.Draw();

                Raylib.EndMode2D();

                if (_editable)
                    kaiEditor.Draw(entityManager);

                Raylib.EndDrawing();
            }

            //Cleanup
            Raylib.CloseWindow();
        }
    }
}
