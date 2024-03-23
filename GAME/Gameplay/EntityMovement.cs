using Kai_Engine.ENGINE.Components;
using Kai_Engine.ENGINE.Entities;
using Kai_Engine.ENGINE.Utils;
using Raylib_cs;

namespace Kai_Engine.GAME.Gameplay
{
    internal class EntityMovement
    {
        public bool _colliding;

        //TODO: MOVEMENT NEEDS TO BE REDONE. THERE SHOULD BE AN INPUT HANDLER
        public void MovePlayer(GameObject player)
        {
            //Keyboard map
            if (Raylib.IsKeyPressed(KeyboardKey.W))
                player.Transform.position.Y -= 17;

            if (Raylib.IsKeyPressed(KeyboardKey.A))
                player.Transform.position.X -= 17;

            if (Raylib.IsKeyPressed(KeyboardKey.S))
                player.Transform.position.Y += 17;

            if (Raylib.IsKeyPressed(KeyboardKey.D))
                player.Transform.position.X += 17;
        }


        //TODO: COLLISION SYSTEM NEEDS TO BE REWRITTEN OR CHECKED FOR ERRORS

        #region Collisions
        public bool AABBCollision(kCollider self, kCollider other)
        {
            //NOTE: Z is Width and W is Height in the Vector4
            var selfCol = self.colliderSize;
            var otherCol = other.colliderSize;

            //to align with the bound debug, the size needs to be increased by 8 and shifted over 4 pixels
            return selfCol.X <= otherCol.X + otherCol.Z && selfCol.Z + selfCol.X >= otherCol.X
                && selfCol.Y <= otherCol.Y + otherCol.W && selfCol.Y + selfCol.W >= otherCol.Y;
        }

        public void CheckCollision(kCollider self, kCollider other)
        {
            if (self != null && other != null)
            {
                if (AABBCollision(self, other))
                    _colliding = true;
                else
                    _colliding = false;
            }
        }
        #endregion
    }
}
