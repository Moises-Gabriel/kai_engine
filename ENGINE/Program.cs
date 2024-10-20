using Kai_Engine.ENGINE.UserInterface;
using Kai_Engine.GAME.Management;
using Kai_Engine.ENGINE.Utils;
using System.Globalization;
using Kai_Engine.EDITOR;
using Raylib_cs;

namespace Kai_Engine.ENGINE
{
    //NOTE: Layer hierarchy goes from bottom to top
    public enum Layer
    {
        Floor,
        Item,
        Wall,
        Player,
        UI
    }

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
            #region ENGINE INFO
            //--------------------
            KaiLogger.Important("Program", $"{_engineName}: [v:{_engineVersion}]", false);
            KaiLogger.Important("Program", $"Game: {_gameName}", false);
            //--------------------
            #endregion

            #region INITIALIZATION
            //--------------------
            EntityManager entityManager = new();
            UIManager uiManager = new();
            Kai_Editor kaiEditor = new();

            Raylib.InitWindow(ScreenWidth, ScreenHeight, _engineName);
            Raylib.SetTargetFPS(60);

            if (_editable) kaiEditor.Init();

            entityManager.Init();
            uiManager.Init();
            //--------------------
            #endregion

            #region START
            //--------------------
            entityManager.Start();
            uiManager.Start();

            if (_editable) kaiEditor.Start(entityManager);
            //--------------------
            #endregion

            #region GAME LOOP
            //--------------------
            KaiLogger.Important("Program", "Starting Game Loop", false);
            while (!Raylib.WindowShouldClose())
            {
                #region UPDATE
                //--------------------
                entityManager.Update();
                uiManager.Update();
                if (_editable) kaiEditor.Update(entityManager);
                //--------------------
                #endregion
                #region DRAW
                //--------------------
                Raylib.BeginDrawing();
                Raylib.ClearBackground(clearColor);

                Raylib.BeginMode2D(entityManager.Camera.RayCamera);
                entityManager.Draw();
                Raylib.EndMode2D();

                uiManager.Draw();

                if (_editable)
                    kaiEditor.Draw(entityManager);

                if (kaiEditor.DebugOpen && _editable)
                    kaiEditor.DrawGUI(entityManager);

                Raylib.EndDrawing();
                //--------------------
                #endregion
            }
            //--------------------
            #endregion

            //Cleanup
            Raylib.CloseWindow();
        }
    }
}


