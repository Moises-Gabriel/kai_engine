using Raylib_cs;
using Kai_Engine.ENGINE.Systems;
using Kai_Engine.ENGINE.Utils;

namespace Kai_Engine
{
    internal class Program
    {
        private const string _engineName    = "Kai Engine";
        private const string _engineVersion = "0.0.1";
        private const string _gameName      = "Down & Down";

        internal static int MapWidth  = 960;
        internal static int MapHeight = 540;


        static void Main()
        {
            EntityManager entityManager = new();

            //Initialize Window
            Raylib.InitWindow(MapWidth, MapHeight, _engineName);


            //Start Engine Processes
            KaiLogger.Important($"{_engineName}: [v{_engineVersion}]", true);
            KaiLogger.Important($"Game: {_gameName}", true);

            entityManager.Start();

            //Main Game Loop
            while (!Raylib.WindowShouldClose())
            {

                //Update
                entityManager.Update();

                //Draw
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                    entityManager.Render();

                Raylib.EndDrawing();
            }

            //Cleanup
            Raylib.CloseWindow();
        } 
    }
}
