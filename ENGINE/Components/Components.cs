using Raylib_cs;
using System.Numerics;
using Kai_Engine.ENGINE.Utils;
using Kai_Engine.ENGINE.Entities;

namespace Kai_Engine.ENGINE.Components
{

    public interface IComponent { void SetParentObject(GameObject parentObject); }

    public class kName : IComponent
    {
        public string? name;

        private GameObject _gameObject;
        public void SetParentObject(GameObject parentObject)
        {
            _gameObject = parentObject;
        }
        public GameObject gameObject
        {
            get { return _gameObject; }
        }
    }
    public class kTransform : IComponent
    {
        public Vector2 position;
        public Vector2 size;

        private GameObject _gameObject;
        public void SetParentObject(GameObject parentObject)
        {
            _gameObject = parentObject;
        }
        public GameObject gameObject
        {
            get { return _gameObject; }
        }
    }
    public class kSprite : IComponent
    {
        public Texture2D sprite;
        public string? filePath;

        private GameObject? _gameObject;
        public void SetParentObject(GameObject parentObject)
        {
            _gameObject = parentObject;
        }
        public GameObject gameObject
        {
            get { return _gameObject; }
        }
    }
    public class kHealth : IComponent
    {
        public float health;

        private GameObject _gameObject;
        public void SetParentObject(GameObject parentObject)
        {
            _gameObject = parentObject;
        }
        public GameObject gameObject
        {
            get { return _gameObject; }
        }
    }
    public class kTag : IComponent
    {
        public string? tag;

        private GameObject _gameObject;
        public void SetParentObject(GameObject parentObject)
        {
            _gameObject = parentObject;
        }
        public GameObject gameObject
        {
            get { return _gameObject; }
        }
    }
    public class kCollider : IComponent
    {
        //Determine if collider is active
        public bool isActive        = false;
        //Individual collision boolean
        public bool isColliding     = false;

        //Set collider size to object size
        public Vector4 ColliderSize(kTransform transform, Vector2 size)
        {
            //x,y,w(z),h(w)
            return new Vector4((int)transform.position.X, (int)transform.position.Y, (int)size.X, (int)size.Y);
        }

        private GameObject _gameObject;
        public void SetParentObject(GameObject parentObject)
        {
            _gameObject = parentObject;
        }
        public GameObject gameObject
        {
            get { return _gameObject; }
        }

        #region DEBUG
        public void DrawBounds(kTransform transform, Vector2 size, Color debugColor)
        {
            Raylib.DrawRectangleLines((int)ColliderSize(transform, size).X, (int)ColliderSize(transform, size).Y, 
                                      (int)ColliderSize(transform, size).Z, (int)ColliderSize(transform, size).W, debugColor);
        }
        #endregion
    }

}
