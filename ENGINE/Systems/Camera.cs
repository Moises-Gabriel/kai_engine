using Raylib_cs;
using Kai_Engine.ENGINE.Components;
using System.Numerics;
using Kai_Engine.ENGINE.Utils;

namespace Kai_Engine.ENGINE.Systems
{
    public class Camera
    {
        public Camera2D RayCamera;
        public Vector2Int DeadZoneScale = new(6, 6);
        public float SmoothFactor = 0.15f; //Adjust this factor to control the smoothing speed

        public void Start(kTransform playerTransform)
        {
            RayCamera.Offset = new Vector2(Program.MapWidth / 4, Program.MapHeight / 4);
            RayCamera.Target = playerTransform.position;
            RayCamera.Rotation = 0.0f;
            RayCamera.Zoom = 1.0f;
        }

        public void Update(ref Camera2D camera, kTransform playerPosition, Vector2 screenSize)
        {
            //Define the deadzone
            int deadZoneWidth = (int)screenSize.X / (int)DeadZoneScale.X;
            int deadZoneHeight = (int)screenSize.Y / (int)DeadZoneScale.Y;

            Rectangle deadZone = new Rectangle
            (
                camera.Target.X - deadZoneWidth / 2,
                camera.Target.Y - deadZoneHeight / 2,
                deadZoneWidth,
                deadZoneHeight
            );

            //Check if the player's position is outside the deadzone
            bool isOutsideDeadZone =
                playerPosition.position.X < deadZone.X ||
                playerPosition.position.X > deadZone.X + deadZone.Width ||
                playerPosition.position.Y < deadZone.Y ||
                playerPosition.position.Y > deadZone.Y + deadZone.Height;

            if (isOutsideDeadZone)
            {


                //Determine if player is outside the deadzone horizontally
                if (playerPosition.position.X < deadZone.X)
                {
                    camera.Target.X = KaiMath.Lerp(camera.Target.X, playerPosition.position.X + (deadZone.Width / 2), SmoothFactor);
                }
                else if (playerPosition.position.X > deadZone.X + deadZone.Width)
                {
                    camera.Target.X = KaiMath.Lerp(camera.Target.X, playerPosition.position.X - (deadZone.Width / 2), SmoothFactor);
                }

                //Determine if player is outside the deadzone vertically
                if (playerPosition.position.Y < deadZone.Y)
                {
                    camera.Target.Y = KaiMath.Lerp(camera.Target.Y, playerPosition.position.Y + (deadZone.Height / 2), SmoothFactor);
                }
                else if (playerPosition.position.Y > deadZone.Y + deadZone.Height)
                {
                    camera.Target.Y = KaiMath.Lerp(camera.Target.Y, playerPosition.position.Y - (deadZone.Height / 2), SmoothFactor);
                }
            }

        }
    }
}