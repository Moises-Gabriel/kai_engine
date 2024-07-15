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
        private const string _gameName = "Holy Hell";

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
            //Engine Info
            KaiLogger.Important($"{_engineName}: [v:{_engineVersion}]", false);
            KaiLogger.Important($"Game: {_gameName}", false);

            ///######################################################################
            ///                             Initialize
            ///######################################################################
            EntityManager entityManager = new();
            UIManager uiManager = new();
            Kai_Editor kaiEditor = new();

            Raylib.InitWindow(ScreenWidth, ScreenHeight, _engineName);
            Raylib.SetTargetFPS(60);

            Image blankImage = Raylib.GenImageColor(ScreenWidth, ScreenHeight, Color.Blank);
            Texture2D shaderTexture = Raylib.LoadTextureFromImage(blankImage);
            Raylib.UnloadImage(blankImage);
            string frag = "GAME/Assets/Shaders/noise.fs";
            string vert = "GAME/Assets/Shaders/noise.vs";
            Shader backgroundShader = Raylib.LoadShader(vert, frag);

            if (_editable) kaiEditor.Init();

            entityManager.Init();
            uiManager.Init();

            ///######################################################################
            ///                             Start
            ///######################################################################
            entityManager.Start();
            uiManager.Start();

            if (_editable) kaiEditor.Start(entityManager);

            //Main Game Loop
            while (!Raylib.WindowShouldClose())
            {

                ///######################################################################
                ///                             Update
                ///######################################################################
                entityManager.Update();
                uiManager.Update();

                if (_editable) kaiEditor.Update(entityManager);
                ///######################################################################
                ///                             Draw
                ///######################################################################

                Raylib.BeginDrawing();
                Raylib.ClearBackground(clearColor);

                // Raylib.BeginShaderMode(backgroundShader);
                // Raylib.DrawTexture(shaderTexture, ScreenWidth, ScreenHeight, Color.White);
                // Raylib.EndShaderMode();

                Raylib.BeginMode2D(entityManager.Camera.RayCamera);

                entityManager.Draw();

                Raylib.EndMode2D();

                //uiManager.Draw();
                if (_editable) kaiEditor.Draw(entityManager);
                if (kaiEditor.DebugOpen && _editable) kaiEditor.DrawGUI(entityManager);

                Raylib.EndDrawing();
            }

            //Cleanup
            Raylib.UnloadShader(backgroundShader);
            Raylib.CloseWindow();
        }
    }
}


