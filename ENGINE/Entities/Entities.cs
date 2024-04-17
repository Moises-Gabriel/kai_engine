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
        public Vector2 Position { get; private set; }
        public Rectangle Viewport { get; private set; } // The camera's viewport dimensions.
        public Rectangle DeadZone { get; private set; } //Camera deadzone dimensions
        public float SmoothTime { get; set; } = 0.2f; // Smoothing factor.

        public Camera(int viewportWidth, int viewportHeight)
        {
            Viewport = new Rectangle(0, 0, viewportWidth, viewportHeight);
            DeadZone = new Rectangle(0, 0, Viewport.Width * 0.5f, Viewport.Height * 0.5f);
        }

        public void Update(Vector2 playerPosition)
        {
            FollowPlayer(playerPosition);
        }

        private void FollowPlayer(Vector2 playerPosition)
        {
            // Calculate the center of the viewport
            Vector2 viewCenter = new Vector2(playerPosition.X + Viewport.Width * 0.5f, playerPosition.Y + Viewport.Height * 0.5f);
            Vector2 difference = playerPosition - viewCenter;

            //Calculate position of DeadZone
            DeadZone = new Rectangle(viewCenter.X / 2, viewCenter.Y / 2, DeadZone.Width, DeadZone.Height);

            // Only move camera if the player is outside the deadzone
            // if (Math.Abs(difference.X) > DeadZone.Width || Math.Abs(difference.Y) > DeadZone.Height)
            // {
            //     // Calculate the new target position for the camera
            //     Vector2 targetPosition = Position;

            //     if (Math.Abs(difference.X) > DeadZone.Width)
            //         targetPosition.X = playerPosition.X - Viewport.Width / 2;

            //     if (Math.Abs(difference.Y) > DeadZone.Height)
            //         targetPosition.Y = playerPosition.Y - Viewport.Height / 2;

            //     // Interpolate between the current position and the target position
            //     float smoothSpeed = 0.1f; // Smoothing speed, adjust as necessary
            //     Position = Vector2.Lerp(Position, targetPosition, smoothSpeed);
            // }
        }

    }
}

