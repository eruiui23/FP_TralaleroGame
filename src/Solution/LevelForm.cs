using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TralalaGame
{
    public class LevelForm : Form
    {
        

        // --- Form Constants ---
        private const int PlayerInitPosX = 100;
        private const int PlayerInitPosY = 360;

        // --- Polymorphic Game Object Management ---
        private List<GameObject> _gameObjects; // A single list for everything!
        private Tralala _tralala; // We still keep a direct reference for input handling.
        private List<Tile> _tiles; // Keep this for collision checking for now.
        private List<Collectible> _collectibles;
        private List<Enemy> _enemies;

        // --- UI and Colectibles
        private int _coinsCollected;
        private int _totalCoinsInLevel;

        // --- Class Variables ---
        private System.Windows.Forms.Timer gameTimer;
        private TextFont _framelabel;
        private Font _textfont;
        private Label _cursorCoordsLabel;
        private long _totalFrameCount;

        // --- NEW: Input state tracking belongs to the form ---
        private bool _leftPressed = false;
        private bool _rightPressed = false;
        private bool _jumpPressed = false;
        private bool _shiftPressed = false;

        public LevelForm()
        {
            
            _collectibles = new List<Collectible>();
            _enemies = new List<Enemy>();
            _coinsCollected = 0;
            var allResourceNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (var name in allResourceNames) { System.Diagnostics.Debug.WriteLine(name); }


            InitializeLevel();
            InitializeTiles();
            InitializeEnemies();      // NEW
            InitializeCollectibles(); // NEW
            InitializeCharacter();    // Character must be initialized AFTER tiles
            InitializeGameObjects();  // NEW HELPER
            _totalCoinsInLevel = _collectibles.Count;
            InitializeTimer();
            InitializeInput();
            LoadCustomFont();
            InitializeUI();
        }


        private void InitializeLevel()
        {
            this.Text = "Tralala Test Drive (Optimized)";
            this.Size = new Size(800, 600);
            this.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("TralalaGame.Resources.BG.jpg")); // Set the image
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.DoubleBuffered = true;
            
        }

        private void InitializeTiles()
        {
            _tiles = new List<Tile>();
            int visibleHeight = 32; // Only show top 32 pixels

            // Create tiles with visible height matching collision bounds
            var bottom1 = new Tile(new Point(1, 500), new Size(196, visibleHeight), visibleHeight);
            var bottom2 = new Tile(new Point(360, 500), new Size(256, visibleHeight), visibleHeight);
            var second1 = new Tile(new Point(580, 400), new Size(256, visibleHeight), visibleHeight);
            var third1 = new Tile(new Point(320, 300), new Size(256, visibleHeight), visibleHeight);
            var second2 = new Tile(new Point(1, 234), new Size(256, visibleHeight), visibleHeight);
            var fourth1 = new Tile(new Point(283, 109), new Size(200, visibleHeight), visibleHeight);
            var final = new Tile(new Point(644, 73), new Size(234, visibleHeight), visibleHeight);

            _tiles.Add(bottom1);
            _tiles.Add(bottom2);
            _tiles.Add(second1);
            _tiles.Add(third1);
            _tiles.Add(second2);
            _tiles.Add(fourth1);
            _tiles.Add(final);

            _gameObjects = new List<GameObject>();
            _gameObjects.AddRange(_tiles);
        }
        private void InitializeCollectibles()
        {
            // Create and add collectibles to their own list
            _collectibles.Add(new Collectible(new Point(600, 360)));
            _collectibles.Add(new Collectible(new Point(700, 360)));
            _collectibles.Add(new Collectible(new Point(650, 360)));
            _collectibles.Add(new Collectible(new Point(750, 360)));
           
            _collectibles.Add(new Collectible(new Point(340, 260)));
            _collectibles.Add(new Collectible(new Point(390, 260)));
            _collectibles.Add(new Collectible(new Point(440, 260)));
            _collectibles.Add(new Collectible(new Point(490, 260)));
            _collectibles.Add(new Collectible(new Point(540, 260)));

            _collectibles.Add(new Collectible(new Point(100, 194)));
            _collectibles.Add(new Collectible(new Point(50, 194)));
            _collectibles.Add(new Collectible(new Point(150, 194)));
            _collectibles.Add(new Collectible(new Point(200, 194)));

            _collectibles.Add(new Collectible(new Point(670, 33)));
            _collectibles.Add(new Collectible(new Point(720, 33)));
        }
        private void InitializeEnemies()
        {
            // Create and add enemies to their own list
            _enemies.Add(new TungTung(new Point(360, 452), 230)); // Patrols 250px on bottom2 tile
            _enemies.Add(new TungTung(new Point(283, 61), 150)); // Patrols 150px on fourth1 tile
        }
        private void InitializeCharacter()
        {
            _tralala = new Tralala(new Point(PlayerInitPosX, PlayerInitPosY), _tiles, this.ClientSize.Width, this.ClientSize.Height);
        }
        private void InitializeGameObjects()
        {
            // Clear the list to be safe
            _gameObjects = new List<GameObject>();

            // Add all objects to the single list for easy updating
            _gameObjects.AddRange(_tiles);
            _gameObjects.AddRange(_collectibles);
            _gameObjects.AddRange(_enemies);
            _gameObjects.Add(_tralala); // Add player last so they draw on top

            
        }
        private void InitializeTimer()
        {
            gameTimer = new System.Windows.Forms.Timer
            {
                Interval = 60  // Adjusted for a smoother ~50 FPS update rate
            };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void InitializeInput()
        {
            this.KeyDown += LevelForm_KeyDown;
            this.KeyUp += LevelForm_KeyUp;
        }


        private void CheckInteractions()
        {
            Rectangle playerBounds = _tralala.Bounds;

            // Check for collision with collectibles
            foreach (var coin in _collectibles)
            {
                // Only check if the coin hasn't been collected yet
                if (!coin.IsCollected && playerBounds.IntersectsWith(coin.Bounds))
                {
                    coin.OnCollected();
                    _coinsCollected++;
                }
            }

            // Check for collision with enemies
            foreach (var enemy in _enemies)
            {
                if (playerBounds.IntersectsWith(enemy.Bounds))
                {
                    // Player hit an enemy, reset their position
                    _tralala.Reset();
                    // Optional: Remove a life, play a sound, etc.
                    break; // Exit the loop after one hit
                }
            }

            // Optional: Clean up collected items from the main list to improve performance
            _gameObjects.RemoveAll(go => go is Collectible c && c.IsCollected);
        }
        private void LoadCustomFont()
        {
            // Create a private font collection
            PrivateFontCollection pfc = new PrivateFontCollection();

            using (var fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TralalaGame.Resources.5x7 MT Pixel.ttf"))
            {
                if (fontStream != null)
                {
                    // Create a buffer to read the font data
                    byte[] fontdata = new byte[fontStream.Length];
                    fontStream.Read(fontdata, 0, (int)fontStream.Length);

                    // Add the font to the collection
                    GCHandle handle = GCHandle.Alloc(fontdata, GCHandleType.Pinned);
                    pfc.AddMemoryFont(handle.AddrOfPinnedObject(), fontdata.Length);
                    handle.Free();
                }
            }

            // Create a new Font object from the collection
            _textfont = new Font(pfc.Families[0], 16, FontStyle.Regular);
            pfc.Dispose();
        }
        private void InitializeUI()
        {
            _framelabel = new TextFont
            {
                Text = $"COINS: 0/{_totalCoinsInLevel}",     // NEW (it will be updated immediately anyway)
                Location = new Point(10, 10),
                AutoSize = true,
                Font = _textfont,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(_framelabel);
        }

        // --- Game Loop (Now much cleaner!) ---
        private void GameTimer_Tick(object sender, System.EventArgs e)
        {
            // 1. Handle Input (specific to player)
            _tralala.HandleInput(_leftPressed, _rightPressed, _jumpPressed, _shiftPressed);

            // 2. Update all game objects (player, enemies, etc.)
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Update();
            }

            // 3. Check for player interactions with other objects (NEW!)
            CheckInteractions();

            // 4. Reset the jump flag
            _jumpPressed = false;

            // 5. Update UI
            UpdateCoins();

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // This method is called every time the form needs to be redrawn.

            // 1. Call the base method (good practice).
            base.OnPaint(e);

            // 2. The background is drawn automatically by `this.BackgroundImage`,
            //    so we don't need to draw it here.

            // 3. Loop through every single game object and tell it to draw itself.
            foreach (var gameObject in _gameObjects)
            {
                // We pass the form's graphics object to each object's Draw method.
                // We use Point.Empty for the camera because your background is static.
                gameObject.Draw(e.Graphics, Point.Empty);
            }
        }

        // --- Input Handling ---
        private void LevelForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A) _leftPressed = true;
            if (e.KeyCode == Keys.D) _rightPressed = true;
            if (e.KeyCode == Keys.Space)
            {
                _jumpPressed = true;
                e.SuppressKeyPress = true; // Prevent the default space bar action
            }
            if (e.KeyCode == Keys.ShiftKey) _shiftPressed = true;
        }

        private void LevelForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A) _leftPressed = false;
            if (e.KeyCode == Keys.D) _rightPressed = false;
            if (e.KeyCode == Keys.ShiftKey) _shiftPressed = false;
            // No need to handle KeyUp for Jump, as it's a single action.
        }

        private void UpdateCoins()
        {
            _framelabel.Text = $"COIN(S) COLLECTED: {_coinsCollected}/{_totalCoinsInLevel}";
        }
    }
}