using Kai_Engine.ENGINE.Entities;
using Raylib_cs;
using System.Numerics;

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
        public bool IsActive = false;
        //Individual collision boolean
        public bool IsColliding = false;
        //Determine if collision has finished processing
        public bool FinishedProcessing = false;

        //Set collider size to object size
        public Vector4 ColliderSize(Vector2 position, Vector2 size)
        {
            //x,y,w(z),h(w)
            return new Vector4((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
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
        public void DrawBounds(Vector2 position, Vector2 size, Color debugColor)
        {
            Raylib.DrawRectangleLines((int)ColliderSize(position, size).X, (int)ColliderSize(position, size).Y,
                                      (int)ColliderSize(position, size).Z, (int)ColliderSize(position, size).W, debugColor);
        }
        #endregion
    }

}
