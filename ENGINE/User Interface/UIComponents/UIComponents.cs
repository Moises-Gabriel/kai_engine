using Kai_Engine.ENGINE.UserInterface.UIObjects;
using System.Numerics;
using Raylib_cs;

namespace Kai_Engine.ENGINE.UserInterface
{
    public interface IUIComponent { void SetParentObject(UIObject parentUIObject); }

    ///########################################################################
    ///                                 TODO:
    ///                        
    ///   - Needs TEXT component
    /// 
    ///########################################################################
    public class UISprite : IUIComponent
    {
        public Texture2D sprite;
        public string? filePath;

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
        public Vector2 size;

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