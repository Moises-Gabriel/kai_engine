using Kai_Engine.ENGINE.Components;
using Raylib_cs;
using System.Numerics;

namespace Kai_Engine.ENGINE.Entities
{
    public interface IEntity { void Draw(); }

    //NOTE: Layer hierarchy goes from bottom to top
    public enum Layer
    {
        Floor,
        Wall,
        Player,
        UI
    }

    public class GameObject : IEntity
    {

        //Components List
        public List<IComponent> Components { get; } = new List<IComponent>();

        //Default Components
        public kName      Name      { get; set; }
        public kTransform Transform { get; set; }
        public kSprite    Sprite    { get; set; }
        public Layer      Layer     { get; set; }


        public GameObject(Texture2D sprite, Vector2 position, Layer layer, string name)
        {
            Name      = new kName      { name = name };
            Transform = new kTransform { position = position, size = new Vector2(16, 16) };
            Sprite    = new kSprite    { sprite = sprite };
            Layer     = layer;

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

        public void Draw()
        {
            Raylib.DrawTexture(Sprite.sprite, (int)Transform.position.X, (int)Transform.position.Y, Color.White);
        }
    }
}
