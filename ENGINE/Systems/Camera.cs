using Raylib_cs;
using Kai_Engine.ENGINE.Components;
using System.Numerics;
using Kai_Engine.ENGINE.Utils;

namespace Kai_Engine.ENGINE.Systems
{
    public class Camera
    {
        public Camera2D RayCamera;

        public void Start()
        {
            RayCamera.Offset = new Vector2(Program.MapWidth / 2, Program.MapHeight / 2);
            RayCamera.Rotation = 0;
            RayCamera.Zoom = 2.0f;
        }

        public void Update(ref Camera2D camera, kTransform playerPosition, Vector2 screenSize)
        {
            //Define the deadzone
            int deadZoneWidth = (int)screenSize.X / 4;
            int deadZoneHeight = (int)screenSize.Y / 4;

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
                //Adjust this factor to control the smoothing speed
                float smoothFactor = 0.025f;

                //Determine if player is outside the deadzone horizontally
                if (playerPosition.position.X < deadZone.X)
                {
                    camera.Target.X = KaiMath.Lerp(camera.Target.X, playerPosition.position.X + (deadZone.Width / 2), smoothFactor);
                }
                else if (playerPosition.position.X > deadZone.X + deadZone.Width)
                {
                    camera.Target.X = KaiMath.Lerp(camera.Target.X, playerPosition.position.X - (deadZone.Width / 2), smoothFactor);
                }

                //Determine if player is outside the deadzone vertically
                if (playerPosition.position.Y < deadZone.Y)
                {
                    camera.Target.Y = KaiMath.Lerp(camera.Target.Y, playerPosition.position.Y + (deadZone.Height / 2), smoothFactor);
                }
                else if (playerPosition.position.Y > deadZone.Y + deadZone.Height)
                {
                    camera.Target.Y = KaiMath.Lerp(camera.Target.Y, playerPosition.position.Y - (deadZone.Height / 2), smoothFactor);
                }
            }

        }
    }
}