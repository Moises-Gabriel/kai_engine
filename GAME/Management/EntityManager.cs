using Kai_Engine.ENGINE.Components;
using Kai_Engine.ENGINE.Entities;
using Kai_Engine.ENGINE.Utils;
using System.Numerics;
using Raylib_cs;
using Kai_Engine.ENGINE;

namespace Kai_Engine.GAME.Management
{
    internal class EntityManager
    {
        #region Entity Variables
        //List of all entities in the game
        public List<IEntity> Entities        = new();
                                             
        //Wall Entity                         
        public List<GameObject> WallObjects  = new();
        private string _wallSpritePath       = @"C:\Dev\CSharp\Kai_Engine_CS\Kai_Engine\GAME\Assets\wall_sprite.png";
        private Texture2D _wallSprite        = new();
                                             
        //Floor Entity                        
        private string _floorSpritePath      = @"C:\Dev\CSharp\Kai_Engine_CS\Kai_Engine\GAME\Assets\floor_sprite.png";
        private Texture2D _floorSprite       = new();
                                             
        //Player Entity                       
        public GameObject? player;           
        private string _playerSpritePath     = @"C:\Dev\CSharp\Kai_Engine_CS\Kai_Engine\GAME\Assets\player_sprite.png";
        private Texture2D _playerSprite      = new();
        public bool PlayerCreated            = false;
        #endregion

        public void Start()
        {
            //Initialize entity sprites
            _wallSprite   = Raylib.LoadTexture(_wallSpritePath);
            _floorSprite  = Raylib.LoadTexture(_floorSpritePath);
            _playerSprite = Raylib.LoadTexture(_playerSpritePath);

            //Create level
            GenerateLevel();
        }

        public void Update()
        {
            //Check collisions between every entity
            foreach (var wall in WallObjects)
            {
                kCollider? playerCollider = player.GetComponent<kCollider>();
                kCollider? wallCollider = wall.GetComponent<kCollider>();
            }
        }

        public void Draw()
        {
            foreach (var entity in Entities)
            {
                entity.Draw();
            }

            //DEBUG
            DebugDraw();
        }
        
        public void SpawnPlayer(Vector2 spawnPoint)
        {
            GameObject _player = new GameObject(_playerSprite, spawnPoint, "Player");
            player = _player;

            //Add Components
            kHealth playerHealth = new kHealth();          //initialize Health component
            playerHealth.health = 50;                      //set the health                        
                                                           
            kCollider playerCollider = new kCollider();    //initialize Collider component
            kTransform playerTransform = player.Transform;

            playerCollider.ColliderSize(playerTransform, new Vector2(16,16));     //Set the bounds of the collider

            player.AddComponent(playerHealth);             //add Health component
            player.AddComponent(playerCollider);           //add Player Collider

            Entities.Add(player);                          //add Player to entity list
        }

        public void GenerateLevel()
        {
            //Parameter is maximum steps for walker
            //Higher steps = denser wall placement
            DrunkardsWalk(5000);
        }

        /// <summary>
        /// Utilizes the Drunkard's Walk algorithm to create a nice cave system.
        /// The caves are generally cavernous with few smaller rooms. 
        /// </summary>

        #region Drunkard's Walk
        //List of Taken positions
        private List<Vector2> takenPositions = new List<Vector2>();

        //List of Free positions
        private List<Vector2> freePositions = new List<Vector2>();

        void DrunkardsWalk(int maxSteps)
        {
            KaiLogger.Info($"Starting Walk", false);

            int tileSize = 16;

            //Initializing wall entity components
            kCollider wallCollider = new kCollider();
            kHealth wallHealth = new kHealth();
            kTag wallTag = new kTag();

            wallHealth.health = 100;
            wallTag.tag = "wall";

            /* --- Drunkard's Walk Algorithm --- */

            //Directions - up, down, left, right
            (int dx, int dy)[] directions = { (0, 1), (0, -1), (-1, 0), (1, 0) };

            //Pick a start point
            Random random = new Random();
            Vector2 currentPos = new Vector2(0, 0);

            //Loop through the walk
            for (int step = 0; step < maxSteps; step++)
            {
                //choose direction at random
                (int dx, int dy) direction = directions[random.Next(0, directions.GetLength(0))];

                //move to new position
                int newX = (int)currentPos.X + direction.dx * (tileSize + 1);
                int newY = (int)currentPos.Y + direction.dy * (tileSize + 1);

                //if new position is within grid, add wall
                if (newX >= 0 && newX <= Program.MapWidth && newY >= 0 && newY <= Program.MapHeight)
                {
                    //update current position with new position
                    currentPos.X = newX;
                    currentPos.Y = newY;

                    GameObject walls = new GameObject(_wallSprite, new Vector2(currentPos.X, currentPos.Y), $"Wall_{step}");

                    //Components
                    walls.AddComponent(wallCollider);

                    kTransform wallTrans = walls.Transform;
                    wallCollider.ColliderSize(wallTrans, new Vector2(16,16));
                    wallCollider.isActive = true;

                    walls.AddComponent(wallHealth);
                    walls.AddComponent(wallTag);

                    //Add entities and transforms to list.
                    Entities.Add(walls);                          //List of IEntities
                    WallObjects.Add(walls);                       //List of GameObjects
                    takenPositions.Add(walls.Transform.position); //List of Vector2
                }
            }

            //Now loop through entire map and add clear spaces to list of available spawn points
            for (int x = 0; x < Program.MapWidth; x += tileSize + 1)
            {
                for (int y = 0; y < Program.MapHeight; y += tileSize + 1)
                {
                    Vector2 cellPosition = new Vector2(x, y);
                    bool isTaken = false;

                    // Check if the current position is in the list of taken positions
                    foreach (Vector2 pos in takenPositions)
                    {
                        if (cellPosition == pos)
                            isTaken = true;
                    }

                    // If the position is not taken, add it to the list of free positions
                    if (!isTaken)
                        freePositions.Add(cellPosition);
                }
            }

            //Place floors
            int floorCounter = 0;
            foreach (Vector2 pos in freePositions)
            {
                floorCounter++;
                GameObject floor = new GameObject(_floorSprite, pos, $"Floor_{floorCounter}");
                Entities.Add(floor);
            }

            //Spawn player at a free location
            Vector2 spawnPoint = freePositions[random.Next(0, freePositions.Count)];
            SpawnPlayer(spawnPoint);
            PlayerCreated = true;


            takenPositions.Add(spawnPoint); //add player spawn point to taken positions
            freePositions.Remove(spawnPoint); //remove player spawn point from available positions

            //DEBUG
            KaiLogger.Info($"Spawn Point: {spawnPoint}", false);
            KaiLogger.Info("Taken Positions: " + takenPositions.Count.ToString(), false);
            KaiLogger.Info("Free Positions: " + freePositions.Count.ToString(), false);
        }
        #endregion

        #region DEBUG

        void DebugDraw()
        {
            if (player != null)
            {
                kCollider? playerCollider = player.GetComponent<kCollider>();
                playerCollider.DrawBounds(player.Transform, new Vector2 (16,16), Color.White);
            }
        }

        #endregion
    }
}
