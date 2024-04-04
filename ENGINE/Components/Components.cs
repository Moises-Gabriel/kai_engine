using Raylib_cs;
using System.Numerics;
using Kai_Engine.ENGINE.Utils;

namespace Kai_Engine.ENGINE.Components
{

    public interface IComponent { }

    public class kName : IComponent
    {
        public string? name;
    }
    public class kTransform : IComponent
    {
        public Vector2 position;
        public Vector2 size;
    }
    public class kSprite : IComponent
    {
        public Texture2D sprite;
        public string? filePath;
    }
    public class kHealth : IComponent
    {
        public float health;
    }
    public class kTag : IComponent
    {
        public string? tag;
    }
    public class kCollider : IComponent
    {
        //Set the size/bounds of the collider component
        public Vector4 colliderSize = new Vector4(); //set custom collider size

        //Set collider size to object size
        public void SetBounds(kTransform transform)
        {
            colliderSize = new Vector4((int)transform.position.X, (int)transform.position.Y, (int)transform.size.X, (int)transform.size.Y); //x,y,w(z),h(w)
        }

        //Draws DEBUG rect around player
        public Color debugColor;
        public void DrawBounds(kTransform transform)
        {
            //the offsets allow the bounds to be slightly larger than the player
            if (transform != null)
                Raylib.DrawRectangleLines((int)transform.position.X - 4, (int)transform.position.Y - 4, (int)transform.size.X + 8, (int)transform.size.Y + 8, debugColor);
        }
    }

}
