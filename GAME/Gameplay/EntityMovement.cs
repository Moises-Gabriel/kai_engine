using Kai_Engine.ENGINE.Components;
using Kai_Engine.ENGINE.Entities;
using Kai_Engine.GAME.Management;
using Kai_Engine.ENGINE.Utils;
using System.Numerics;
using Raylib_cs;

namespace Kai_Engine.GAME.Gameplay
{
    internal class EntityMovement
    {
        ///######################################################################
        ///                     //TODO: IN ENTITY MOVEMENT
        ///                           
        ///   - When colliding with wall, damage is ticked multiple times
        ///   a second (because it's per frame); not wanted [pending]
        ///   
        ///   - Figure out how to actually destroy/remove a wall when its
        ///   health goes all the way down [pending/done]
        /// 
        ///######################################################################
        
        private enum Direction { UP, DOWN, LEFT, RIGHT }
        private Direction? lastDirection = null;

        ///#####################################################
        ///
        ///                 PLAYER MOVEMENT
        /// 
        ///#####################################################

        private int _moveDistance = 17; //17 pixels is one unit for the game
        public void MovePlayer(EntityManager eManager)
        {
            GameObject player = eManager.player;

            if (player != null)
            {
                kTransform playerTransform = player.GetComponent<kTransform>();
                kCollider playerCollider = player.GetComponent<kCollider>();

                KeyboardKey keyPressed = (KeyboardKey)Raylib.GetKeyPressed();

                if (!playerCollider.isColliding)
                {
                    switch (keyPressed)
                    {
                        case KeyboardKey.W:
                            playerTransform.position.Y -= _moveDistance;
                            lastDirection = Direction.UP;
                            break;
                        case KeyboardKey.S:
                            playerTransform.position.Y += _moveDistance;
                            lastDirection = Direction.DOWN;
                            break;
                        case KeyboardKey.A:
                            playerTransform.position.X -= _moveDistance;
                            lastDirection = Direction.LEFT;
                            break;
                        case KeyboardKey.D:
                            playerTransform.position.X += _moveDistance;
                            lastDirection = Direction.RIGHT;
                            break;
                        default:
                            lastDirection = null;  //No valid key, no direction
                            break;
                    }
                }
            }
        }

        private GameObject? otherObject;

        //Check which direction we were going when we collided
        public async void CheckDirection(GameObject player)
        {
            kCollider playerCollider = player.GetComponent<kCollider>();
            kTransform playerTransform = player.GetComponent<kTransform>();

            if (playerCollider.isColliding && lastDirection != null && otherObject != null)
            {
                Vector2 newPosition = playerTransform.position;
                switch (lastDirection)
                {
                    case Direction.UP:
                        newPosition.Y += _moveDistance;
                        Dig(otherObject);
                        break;
                    case Direction.DOWN:
                        newPosition.Y -= _moveDistance;
                        break;
                    case Direction.LEFT:
                        newPosition.X += _moveDistance;
                        break;
                    case Direction.RIGHT:
                        newPosition.X -= _moveDistance;
                        break;
                }

                await Bump(playerTransform, newPosition);
                if (otherObject != null)
                    Dig(otherObject);
                lastDirection = null;  //Reset direction after handling collision
            }

            //Reset collision flags
            playerCollider.isColliding = false;
            if (otherObject != null)
            {
                otherObject.GetComponent<kCollider>().isColliding = false;
            }
            otherObject = null;  //Clear the reference to prevent unintended repeated interaction
        }

        //Reset position to before wall
        private async Task Bump(kTransform playerTransform, Vector2 direction)
        {
            await Task.Delay(75); //milliseconds
            playerTransform.position = direction;
        }

        ///#####################################################
        ///
        ///                 PLAYER INTERACTION
        ///    (could possibly be moved into another class)
        /// 
        ///#####################################################
       
        //Dig at wall and destroy it (when damaged enough)
        private void Dig(GameObject other)
        {
            kHealth otherHealth = other.GetComponent<kHealth>();

            if (otherHealth != null)
            {
                otherHealth.health--;
                KaiLogger.Info($"{other.Name.name} Health: " + otherHealth.health, false);

                if (otherHealth.health <= 0)
                {
                    //Destroy wall
                    other.IsActive = false;

                    //Additional logic to actually remove the wall from the game
                }
            }
        }

        ///#####################################################
        ///
        ///                    COLLISION
        /// 
        ///#####################################################

        public void CheckCollision(EntityManager eManager, GameObject player)
        {
            kTransform playerTransform = player.GetComponent<kTransform>();
            kCollider playerCollider = player.GetComponent<kCollider>();

            Vector4 playerCol = playerCollider.ColliderSize(playerTransform, playerTransform.size);

            GameObject closestObject = null;  // Track the closest object being collided with
            float closestDistance = 1.0f;

            foreach (var gameObject in eManager.AllObjects)
            {
                if (gameObject.IsActive)
                {
                    if (gameObject == player) continue; // Skip self-detection

                    kTransform objectTransform = gameObject.GetComponent<kTransform>();
                    kCollider objectCollider = gameObject.GetComponent<kCollider>();

                    Vector4 otherCol = objectCollider.ColliderSize(objectTransform, objectTransform.size);

                    // AABB collision check between colliders
                    bool colliding = (int)playerCol.X <= otherCol.X + otherCol.Z && (int)playerCol.Z + (int)playerCol.X >= otherCol.X
                                  && (int)playerCol.Y <= otherCol.Y + otherCol.W && (int)playerCol.Y + (int)playerCol.W >= otherCol.Y;

                    if (colliding)
                    {
                        float distance = Vector2.Distance(playerTransform.position, objectTransform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestObject = gameObject;
                        }
                    }
                }
            }

            otherObject = closestObject;  // Assign the closest colliding object

            if (otherObject != null)
            {
                otherObject.GetComponent<kCollider>().isColliding = true;
                playerCollider.isColliding = true;
            }

        }
    }
}
