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

    internal class Game
    {
        //Debug Editor
        private const bool _editable = true;
        internal static int cellSize = 16; //update this to match sprite size
        internal static Color clearColor = new Color(28, 22, 19, 255);

        private EntityManager _entityManager = new();
        private UIManager _uiManager = new();
        private Kai_Editor _kaiEditor = new();


        public void Init()
        {

            if (_editable) _kaiEditor.Init();

            _entityManager.Init();
            _uiManager.Init();
        }

        public void Start()
        {
            if (_editable) _kaiEditor.Start(_entityManager);

            _entityManager.Start();
            _uiManager.Start();

        }

        public void Update()
        {
            if (_editable) _kaiEditor.Update(_entityManager);

            _entityManager.Update();
            _uiManager.Update();
        }

        public void Draw()
        {
            #region DRAW
            //--------------------
            Raylib.ClearBackground(clearColor);

            Raylib.BeginMode2D(_entityManager.Camera.RayCamera);
            _entityManager.Draw();
            Raylib.EndMode2D();
            //--------------------
            #endregion
        }

        public void UIDraw()
        {
            #region DRAW
            //--------------------
            _uiManager.Draw();

            if (_editable)
                _kaiEditor.Draw(_entityManager);

            if (_kaiEditor.DebugOpen && _editable)
                _kaiEditor.DrawGUI(_entityManager);
            //--------------------
            #endregion
        }
    }
}


