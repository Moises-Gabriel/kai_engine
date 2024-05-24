using System.Numerics;
using Raylib_cs;

namespace Kai_Engine.ENGINE.Utils
{
    public static class KaiMath
    {
        //Linear interpolation
        public static float Lerp(float start, float end, float amount)
        {
            return start + (end - start) * amount;
        }

        // Convert a world space position to screen space
        public static Vector2 WorldToScreen(Vector2 worldPos, Camera2D camera)
        {
            Vector2 screenSize = new Vector2(Program.MapWidth / 4, Program.MapHeight / 4);
            Vector2 screenPos = new Vector2(
                (worldPos.X - camera.Target.X) * camera.Zoom + screenSize.X,
                (worldPos.Y - camera.Target.Y) * camera.Zoom + screenSize.Y
            );
            return screenPos;
        }
    }

    public struct Vector2Int
    {
        public int X;
        public int Y;

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}