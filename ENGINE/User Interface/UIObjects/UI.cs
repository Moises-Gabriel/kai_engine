using System.Numerics;
using Raylib_cs;

namespace Kai_Engine.ENGINE.UserInterface.UIObjects
{
    public interface IUIObject { void Draw(); }
    public class UIObject : IUIObject
    {
        public bool IsActive = true;

        //Components List
        public List<IUIComponent> Components { get; } = new();

        //Default Components
        public UISprite? Sprite { get; set; }
        public UITransform Transform { get; set; }
        public Layer Layer { get; set; }

        public UIObject(Texture2D sprite, Vector2 position, Vector2 size, Layer layer, bool isActive)
        {
            Sprite = new UISprite { sprite = sprite };
            Transform = new UITransform { position = position, size = size };
            Layer = layer;
            IsActive = isActive;

            //Add default components to component list so they can be accessed through GetComponent
            Components.Add(Transform);
        }

        public void AddComponent(IUIComponent component)
        {
            component.SetParentObject(this);
            Components.Add(component);
        }

        public void RemoveComponent(IUIComponent component)
        {
            Components.Remove(component);
        }

        public T? GetComponent<T>() where T : class, IUIComponent
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
                Raylib.DrawTexture(Sprite.sprite, (int)Transform.position.X, (int)Transform.position.Y, Color.White);
            }
        }
    }
}