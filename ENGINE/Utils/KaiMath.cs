using System.Numerics;
using Raylib_cs;

namespace Kai_Engine.ENGINE.Utils
{
    public static class KaiMath
    {
        public static float Lerp(float start, float end, float amount)
        {
            return start + (end - start) * amount;
        }

        // Convert a world space position to screen space
        public static Vector2 WorldToScreen(Vector2 worldPos, Camera2D camera)
        {
            Vector2 screenSize = new Vector2(Program.MapWidth, Program.MapHeight);
            Vector2 screenPos = new Vector2(
                (worldPos.X - camera.Target.X) * camera.Zoom + screenSize.X / 2,
                (worldPos.Y - camera.Target.Y) * camera.Zoom + screenSize.Y / 2
            );
            return screenPos;
        }
    }
}