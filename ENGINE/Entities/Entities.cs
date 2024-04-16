using Kai_Engine.ENGINE.Components;
using Raylib_cs;
using System.Numerics;

namespace Kai_Engine.ENGINE.Entities
{
    public interface IEntity { void Draw(Camera c); }

    //NOTE: Layer hierarchy goes from bottom to top
    public enum Layer
    {
        Floor,
        Item,
        Wall,
        Player,
        UI
    }

    public class GameObject : IEntity
    {
        public bool IsActive = true;

        //Components List
        public List<IComponent> Components { get; } = new List<IComponent>();

        //Default Components
        public kName Name { get; set; }
        public kTransform Transform { get; set; }
        public kSprite Sprite { get; set; }
        public Layer Layer { get; set; }


        public GameObject(Texture2D sprite, Vector2 position, Layer layer, string name, bool isActive)
        {
            Name = new kName { name = name };
            Transform = new kTransform { position = position, size = new Vector2(16, 16) };
            Sprite = new kSprite { sprite = sprite };
            IsActive = isActive;
            Layer = layer;

            //Add default components to component list so they can be accessed through GetComponent
            Components.Add(Transform);
            Components.Add(Sprite);
        }

        public void AddComponent(IComponent component)
        {
            component.SetParentObject(this);
            Components.Add(component);
        }

        public T? GetComponent<T>() where T : class, IComponent
        {
            // Find the first component of the specified type
            foreach (var component in Components)
            {
                if (component is T typedComponent)
                    return typedComponent;
            }
            return null; // Component not found
        }

        public void Draw(Camera camera)
        {
            if (IsActive)
            {
                Raylib.DrawTexture(Sprite.sprite, (int)Transform.position.X - (int)camera.Position.X, (int)Transform.position.Y - (int)camera.Position.Y, Color.White);
            }
        }
    }

    public class Camera
    {
        public Vector2 Position;
        public float Zoom;

        public Camera() 
        {
            Zoom = 1f; //default camera zoom
        }

        public void Update(GameObject target)
        {
            float viewWidth = Program.MapWidth / 2;
            float viewHeight = Program.MapHeight / 2;

            Position = new Vector2(
                target.Transform.position.X - viewWidth * Zoom,
                target.Transform.position.Y - viewHeight * Zoom
            );
        }

        public void Clamp(Vector2 min, Vector2 max)
        {
            Position = new Vector2(
                Math.Clamp(Position.X, min.X, max.X - Program.MapWidth),
                Math.Clamp(Position.Y, min.Y, max.Y - Program.MapHeight)
            );
        }
    }
}

