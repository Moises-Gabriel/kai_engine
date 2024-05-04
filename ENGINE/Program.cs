using Kai_Engine.ENGINE.UserInterface;
using Kai_Engine.GAME.Management;
using Kai_Engine.ENGINE.Utils;
using Kai_Engine.EDITOR;
using System.Numerics;
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
        private const string _gameName = "Down & Down";

        internal static int ScreenWidth = 1280;
        internal static int ScreenHeight = 720;

        internal static int MapWidth = 640;
        internal static int MapHeight = 360;
        private static RenderTexture2D _renderTexture;

        //Bools
        private const bool _editable = true;
        internal static bool TextureMode = true;

        static void Main()
        {
            //Engine Info
            KaiLogger.Important($"{_engineName}: [v{_engineVersion}]", false);
            KaiLogger.Important($"Game: {_gameName}", false);

            ///######################################################################
            ///                             Initialize
            ///######################################################################
            EntityManager entityManager = new();
            UIManager uiManager = new();
            Kai_Editor kaiEditor = new();

            Raylib.InitWindow(ScreenWidth, ScreenHeight, _engineName);
            Raylib.SetTargetFPS(60);

            _renderTexture = Raylib.LoadRenderTexture(MapWidth, MapHeight);

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

                if (TextureMode)
                {
                    Raylib.BeginTextureMode(_renderTexture);
                    Raylib.ClearBackground(Color.Black);
                    Raylib.BeginMode2D(entityManager.Camera.RayCamera);

                    entityManager.Draw();

                    Raylib.EndMode2D();

                    if (_editable) kaiEditor.Draw(entityManager);

                    Raylib.EndTextureMode();

                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.DarkGray);

                    Raylib.DrawTextureRec(_renderTexture.Texture, new Rectangle(0, 0, _renderTexture.Texture.Width, -_renderTexture.Texture.Height),
                                          new Vector2(ScreenWidth / 4, ScreenHeight / 4), Color.White);

                    uiManager.Draw();
                    if (kaiEditor.DebugOpen) kaiEditor.DrawGUI(entityManager);

                    Raylib.EndDrawing();
                }
                else
                {
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.Black);

                    Raylib.BeginMode2D(entityManager.Camera.RayCamera);

                    entityManager.Draw();

                    Raylib.EndMode2D();

                    if (_editable) kaiEditor.Draw(entityManager);
                    uiManager.Draw();
                    if (kaiEditor.DebugOpen && _editable) kaiEditor.DrawGUI(entityManager);

                    Raylib.EndDrawing();
                }
            }

            //Cleanup
            Raylib.CloseWindow();
        }
    }
}


