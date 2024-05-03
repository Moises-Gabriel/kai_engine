using Kai_Engine.ENGINE.UserInterface.UIObjects;
using System.Numerics;
using Raylib_cs;

namespace Kai_Engine.ENGINE.UserInterface
{
    public interface IUIComponent { void SetParentObject(UIObject parentUIObject); }

    ///########################################################################
    ///                                 TODO:
    ///     Nothing for now                                 
    ///                        
    ///########################################################################
    public class UISprite : IUIComponent
    {
        public string? Tag = "";
        public Texture2D Texture;
        public string? FilePath = "";
        public bool IsLoaded = false;

        private UIObject? _uIObject;
        public void SetParentObject(UIObject parentUIObject)
        {
            _uIObject = parentUIObject;
        }
        public UIObject uIObject
        {
            get { return _uIObject; }
        }
    }

    public class UITransform : IUIComponent
    {
        public Vector2 position;

        private UIObject? _uIObject;
        public void SetParentObject(UIObject parentUIObject)
        {
            _uIObject = parentUIObject;
        }
        public UIObject uIObject
        {
            get { return _uIObject; }
        }
    }

    public class UIScale : IUIComponent
    {
        public float scale;
        private UIObject? _uIObject;
        public void SetParentObject(UIObject parentUIObject)
        {
            _uIObject = parentUIObject;
        }
        public UIObject uIObject
        {
            get { return _uIObject; }
        }
    }

}