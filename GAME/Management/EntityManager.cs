using Kai_Engine.ENGINE;
using Kai_Engine.ENGINE.Components;
using Kai_Engine.ENGINE.Entities;
using Kai_Engine.ENGINE.Utils;
using Kai_Engine.GAME.Gameplay;
using Raylib_cs;
using System.Numerics;
using System.IO;

namespace Kai_Engine.GAME.Management
{
    internal class EntityManager
    {
        ///######################################################################
        ///                     TODO: IN ENTITY MANAGER
        ///                           
        ///   - Nothing for now; Great job!
        /// 
        ///######################################################################

        #region Entity Variables
        private string _basePath = Environment.CurrentDirectory;

        //List of all entities/GameObjects in the game
        public List<IEntity> Entities = new();
        public List<GameObject> AllObjects = new();

        //Item Entity                         
        public List<GameObject> ItemObjects = new();
        private string? _itemSpritePath;
        private Texture2D _itemSprite = new();

        //Wall Entity                         
        public List<GameObject> WallObjects = new();
        private string? _wallSpritePath;
        private Texture2D _wallSprite = new();

        //Floor Entity                        
        private string? _floorSpritePath;
        private Texture2D _floorSprite = new();

        //Player Entity                       
        public GameObject? player;
        private string? _playerSpritePath;
        private Texture2D _playerSprite = new();
        public bool PlayerCreated = false;

        //Camera
        public Camera? Camera;
        public bool Clamped = true;
        #endregion

        //Gameplay
        private EntityMovement? _eMovement;

        public void Start()
        {
            //Set sprite paths
            _itemSpritePath   = Path.Combine(_basePath, "GAME/Assets/item_sprite.png");
            _wallSpritePath   = Path.Combine(_basePath, "GAME/Assets/wall_sprite.png");
            _floorSpritePath  = Path.Combine(_basePath, "GAME/Assets/floor_sprite.png");
            _playerSpritePath = Path.Combine(_basePath, "GAME/Assets/player_sprite.png");

            //Initialize entity sprites
            _itemSprite = Raylib.LoadTexture(_itemSpritePath);
            _floorSprite = Raylib.LoadTexture(_floorSpritePath);
            _wallSprite = Raylib.LoadTexture(_wallSpritePath);
            _playerSprite = Raylib.LoadTexture(_playerSpritePath);

            _eMovement = new EntityMovement();

            //Create level
            GenerateLevel();

            //Initialize Camera
            Camera = new Camera();
            Camera.Zoom = 1f;
        }

        public void Update()
        {
            _eMovement.CheckCollision(this, player);

            _eMovement.MovePlayer(this);
            _eMovement.CheckDirection(player);

            _eMovement.CheckCollision(this, player);

            Camera.Update(player);
            if (Clamped)
            {
                Camera.Clamp(Vector2.Zero, new Vector2(Program.MapWidth, Program.MapHeight)); //Clamps to viewport
            }
        }

        public void Draw()
        {
            //Draw based on position in Layer enum
            var sortedEntities = Entities.OrderBy(e => ((GameObject)e).Layer).ToList();
            foreach (var entity in sortedEntities)
            {
                entity.Draw(Camera);
            }
        }

        public void AddPlayer(Vector2 spawnPoint)
        {
            GameObject _player = new(_playerSprite, spawnPoint, Layer.Player, "Player", true);
            player = _player;

            //Add Components
            kHealth playerHealth = new();          //initialize Health component
            playerHealth.health = 50;                      //set the health                        

            kCollider playerCollider = new();    //initialize Collider component
            kTransform playerTransform = player.Transform;

            playerCollider.ColliderSize(playerTransform, playerTransform.size);     //Set the bounds of the collider

            player.AddComponent(playerHealth);             //add Health component
            player.AddComponent(playerCollider);           //add Player Collider

            Entities.Add(player);                          //add Player to entity list
            //AllObjects.Add(player);                        //add Player to all objects list
        }

        public void AddItem(Vector2 spawnPoint)
        {
            GameObject item = new(_itemSprite, spawnPoint, Layer.Item, "Item", true);

            //Add components
            kCollider itemCollider = new();
            kTransform itemTransform = item.Transform;

            kHealth itemHealth = new();          //initialize Health component
            itemHealth.health = 10;                      //set the health

            itemCollider.ColliderSize(itemTransform, itemTransform.size);

            item.AddComponent(itemCollider);
            item.AddComponent(itemHealth);

            Entities.Add(item);
            AllObjects.Add(item);
            ItemObjects.Add(item);
        }

        public void GenerateLevel()
        {
            //Parameter is maximum steps for walker
            //Higher steps = denser wall placement
            DrunkardsWalk(5000);
        }

        #region Drunkard's Walk
        //List of Taken positions
        private readonly List<Vector2> takenPositions = new();

        //List of Free positions
        private readonly List<Vector2> freePositions = new();

        void DrunkardsWalk(int maxSteps)
        {
            int tileSize = 16;

            KaiLogger.Info($"Placing Floors", false);

            //Place floors
            int floorCounter = 0;
            for (int x = 0; x < Program.MapWidth; x += (tileSize + 1))
            {
                for (int y = 0; y < Program.MapHeight; y += (tileSize + 1))
                {
                    floorCounter++;
                    GameObject floor = new GameObject(_floorSprite, new Vector2(x, y), Layer.Floor, $"Floor_{floorCounter}", true);
                    Entities.Add(floor);
                }
            }

            KaiLogger.Info($"Starting Walk", false);



            /* --- Drunkard's Walk Algorithm --- */

            //Directions - up, down, left, right
            (int dx, int dy)[] directions = { (0, 1), (0, -1), (-1, 0), (1, 0) };

            //Pick a start point
            Random random = new Random();
            Vector2 currentPos = new Vector2(0, 0);

            int counter = 0;
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

                    counter++;
                    if (!takenPositions.Contains(currentPos))
                    {
                        GameObject walls = new GameObject(_wallSprite, new Vector2(currentPos.X, currentPos.Y), Layer.Wall, $"Wall_{counter}", true);

                        //Initializing wall entity components
                        kCollider wallCollider = new kCollider();
                        kHealth wallHealth = new kHealth();
                        kTag wallTag = new kTag();

                        wallHealth.health = 25;
                        wallTag.tag = "wall";

                        //Components
                        walls.AddComponent(wallCollider);

                        kTransform wallTrans = walls.Transform;
                        wallCollider.ColliderSize(wallTrans, new Vector2(16, 16));
                        wallCollider.IsActive = true;

                        walls.AddComponent(wallHealth);
                        walls.AddComponent(wallTag);

                        //Add entities and transforms to list.
                        Entities.Add(walls);                          //List of IEntities
                        AllObjects.Add(walls);                        //List of All GameObjects
                        WallObjects.Add(walls);                       //List of GameObjects
                        takenPositions.Add(walls.Transform.position); //List of Vector2
                    }
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



            //Spawn player at a free location
            Vector2 playerSpawnPoint = freePositions[random.Next(0, freePositions.Count)];
            AddPlayer(playerSpawnPoint);
            PlayerCreated = true;

            takenPositions.Add(playerSpawnPoint); //add player spawn point to taken positions
            freePositions.Remove(playerSpawnPoint); //remove player spawn point from available positions

            //Spawn items at free locations
            int spawnCount = 5;
            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 itemSpawnPoints = freePositions[random.Next(0, freePositions.Count)];
                AddItem(itemSpawnPoints);

                takenPositions.Add(itemSpawnPoints);
                freePositions.Remove(itemSpawnPoints);
            }


            //DEBUG
            KaiLogger.Info("Taken Positions: " + takenPositions.Count.ToString(), false);
            KaiLogger.Info("Free Positions: " + freePositions.Count.ToString(), false);
        }
        #endregion

    }
}
