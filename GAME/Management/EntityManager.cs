using Kai_Engine.ENGINE.Components;
using Kai_Engine.ENGINE.Entities;
using Kai_Engine.ENGINE.Systems;
using Kai_Engine.GAME.Gameplay;
using Kai_Engine.ENGINE.Utils;
using Kai_Engine.ENGINE;
using System.Numerics;
using System.Data;
using Raylib_cs;
using System.Runtime.CompilerServices;

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

        //Sprite Sheet
        public string? _spriteSheetPath = "";
        public Texture2D _spriteSheet = new();

        //Item Entity                         
        private Rectangle _itemRec = new();
        public List<GameObject> ItemObjects = new();

        //Wall Entity           
        private Rectangle _wallRec = new();
        public List<GameObject> WallObjects = new();


        //Floor Entity
        private Rectangle _floorRec = new();

        //Player Entity         
        private Rectangle _playerRec = new();
        public GameObject? player;

        //Camera
        public Camera? Camera;
        #endregion

        //Gameplay
        private EntityMovement? _eMovement;

        public void Init()
        {
            //Set path & Initialize spritesheet
            _spriteSheetPath = Path.Combine(_basePath, "GAME/Assets/sprite_sheet.png");
            _spriteSheet = Raylib.LoadTexture(_spriteSheetPath);

            //Spritesheet Rectangles
            int w = _spriteSheet.Width / 10;
            int h = _spriteSheet.Height / 10;
            _playerRec = new(16, 0, w, h);
            _wallRec = new(0, 16, w, h);
            _itemRec = new(0, 32, w, h);
            _floorRec = new(0, 48, w, h);

            //Initialize Camera
            Camera = new Camera();
        }

        public void Start()
        {
            //Initialize script references
            _eMovement = new EntityMovement();

            //Create level
            GenerateLevel();

            //Set target after player has spawned
            Camera.Start(player.Transform);
        }

        public void Update()
        {
            ///######################################################################
            ///                          Entity Movement
            ///######################################################################
            if (player != null && _eMovement != null)
            {
                _eMovement.CheckCollision(this, player);

                _eMovement.MovePlayer(this);
                _eMovement.CheckDirection(player);

                _eMovement.CheckCollision(this, player);
            }

            Camera.Update(ref Camera.RayCamera, player.Transform, new Vector2(Program.MapWidth, Program.MapHeight));
        }

        public void Draw()
        {
            //Draw based on position in Layer enum
            var sortedEntities = Entities.OrderBy(e => ((GameObject)e).Layer).ToList();
            foreach (var entity in sortedEntities)
            {
                entity.Draw();
            }
        }

        public void AddPlayer(Vector2 spawnPoint)
        {
            GameObject _player = new(_spriteSheet, _playerRec, spawnPoint, Layer.Player, "Player", true);
            player = _player;

            //Add Components
            kHealth playerHealth = new kHealth
            {
                health = 50
            };

            kCollider playerCollider = new();
            kTransform playerTransform = player.Transform;

            Vector2 playerPosition = new Vector2(playerTransform.position.X, playerTransform.position.Y);
            playerCollider.ColliderSize(playerPosition, playerTransform.size);

            player.AddComponent(playerHealth);
            player.AddComponent(playerCollider);

            Entities.Add(player);
        }
        public void AddItem(Vector2 spawnPoint)
        {
            GameObject item = new(_spriteSheet, _itemRec, spawnPoint, Layer.Item, "Item", true);

            //Add components
            kCollider itemCollider = new();
            kTransform itemTransform = item.Transform;

            kHealth itemHealth = new kHealth
            {
                health = 10
            };

            Vector2 itemPosition = new Vector2(itemTransform.position.X, itemTransform.position.Y);
            itemCollider.ColliderSize(itemPosition, itemTransform.size);

            item.AddComponent(itemCollider);
            item.AddComponent(itemHealth);

            Entities.Add(item);
            AllObjects.Add(item);
            ItemObjects.Add(item);
        }
        public void AddWall(Vector2 currentPosition, int health)
        {
            int counter = 0;
            counter++;
            GameObject walls = new GameObject(_spriteSheet, _wallRec, new Vector2(currentPosition.X, currentPosition.Y), Layer.Wall, $"Wall_{counter}", true);

            //Initializing wall entity components
            kCollider wallCollider = new kCollider();
            kHealth wallHealth = new kHealth();
            kTag wallTag = new kTag();

            wallHealth.health = health;
            wallTag.tag = "wall";

            //Components
            walls.AddComponent(wallCollider);


            kTransform wallTrans = walls.Transform;
            Vector2 wallPosition = new Vector2(wallTrans.position.X, wallTrans.position.Y);
            wallCollider.ColliderSize(wallPosition, new Vector2(16, 16));
            wallCollider.IsActive = true;

            walls.AddComponent(wallHealth);
            walls.AddComponent(wallTag);

            //Add entities and transforms to list.
            Entities.Add(walls);                          //List of IEntities
            AllObjects.Add(walls);                        //List of All GameObjects
            WallObjects.Add(walls);                       //List of Wall GameObjects
        }
        public void AddFloor(Vector2 position)
        {
            KaiLogger.Info($"Placing Floors", false);

            //Place floors
            GameObject floor = new GameObject(_spriteSheet, _floorRec, new Vector2(position.X, position.Y), Layer.Floor, $"Floor", true);
            Entities.Add(floor);
            freePositions.Add(floor.Transform.position);
        }
        public void GenerateLevel()
        {
            //Parameter is maximum steps for walker
            //Higher steps = denser wall placement
            DrunkardsWalk(10000);
        }

        #region Drunkard's Walk
        //List of Taken positions
        private readonly List<Vector2> takenPositions = new();
        //List of Free positions
        private readonly List<Vector2> freePositions = new();
        void DrunkardsWalk(int maxSteps)
        {
            Random random = new Random();

            int tileSize = Program.cellSize;
            int tileOffset = tileSize + 1;

            KaiLogger.Info($"Starting Walk", false);

            /* --- Drunkard's Walk Algorithm --- */

            //Directions - up, down, left, right
            (int dx, int dy)[] directions = { (0, 1), (0, -1), (-1, 0), (1, 0) };

            //Pick a start point
            Vector2 currentPos = new Vector2(random.Next(0, Program.MapWidth), random.Next(0, Program.MapHeight));

            //Loop through the walk
            int counter = 0;
            for (int step = 0; step < maxSteps; step++)
            {
                //choose direction at random
                (int dx, int dy) direction = directions[random.Next(0, directions.GetLength(0))];

                //move to new position
                int newX = (int)currentPos.X + direction.dx * tileOffset;
                int newY = (int)currentPos.Y + direction.dy * tileOffset;

                //update current position with new position
                currentPos.X = newX;
                currentPos.Y = newY;

                counter++;
                if (!takenPositions.Contains(currentPos))
                {
                    AddFloor(new Vector2(currentPos.X, currentPos.Y));
                }
            }

            //Spawn player at a free location
            Vector2 playerSpawnPoint = freePositions[random.Next(0, freePositions.Count)];
            AddPlayer(playerSpawnPoint);

            takenPositions.Add(playerSpawnPoint); //add player spawn point to taken positions
            freePositions.Remove(playerSpawnPoint); //remove player spawn point from available positions

            GenerateBorder(freePositions, tileOffset);

            //Spawn items at free locations
            int spawnCount = random.Next(0, 30);
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

        private void GenerateBorder(List<Vector2> freePositions, int tileOffset)
        {
            // Directions to check around each tile (up, down, left, right)
            (int dx, int dy)[] directions = { (0, 1), (0, -1), (-1, 0), (1, 0) };

            HashSet<Vector2> wallPositions = new HashSet<Vector2>();

            foreach (var floorPos in freePositions)
            {
                foreach (var (dx, dy) in directions)
                {
                    Vector2 adjacentPos = new Vector2(floorPos.X + dx * tileOffset, floorPos.Y + dy * tileOffset);
                    if (!freePositions.Contains(adjacentPos) && !wallPositions.Contains(adjacentPos))
                    {
                        AddWall(adjacentPos, 10000);
                        wallPositions.Add(adjacentPos);
                        takenPositions.Add(adjacentPos); //List of Vector2 
                    }
                }
            }
        }
        #endregion

    }
}
