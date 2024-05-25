using Kai_Engine.ENGINE.Components;
using Raylib_cs;
using System.Numerics;

namespace Kai_Engine.ENGINE.Entities
{
    public interface IEntity { void Draw(); }

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


        public GameObject(Texture2D sprite, Rectangle spriteRec, Vector2 position, Layer layer, string name, bool isActive)
        {
            Name = new kName { name = name };
            Transform = new kTransform { position = position, size = new Vector2(16, 16) };
            Sprite = new kSprite { sprite = sprite, rectangle = spriteRec };
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

        public void RemoveComponent(IComponent component)
        {
            Components.Remove(component);
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
            if (IsActive)
            {
                Raylib.DrawTextureRec(Sprite.sprite, Sprite.rectangle, new Vector2((int)Transform.position.X, (int)Transform.position.Y), Color.White);
            }
        }
    }
}

