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
        

        // Form Constant
        private const int PlayerInitPosX = 100;
        private const int PlayerInitPosY = 360;

        // Polymorphic Game Object Management 
        private List<GameObject> _gameObjects; // List Game Object buat Polymorphism
        private Tralala _tralala; // Buat Player Character
        private List<Tile> _tiles; // Tiles list buat rendering & collision
        private List<Collectible> _collectibles; // list coins
        private List<Enemy> _enemies; // list enemies

        //  Coin
        private int _coinsCollected;
        private int _totalCoinsInLevel;

        // Variable class
        private System.Windows.Forms.Timer gameTimer;
        private TextFont _framelabel;
        private Font _textfont;

        // info key pressed
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
            InitializeEnemies();      
            InitializeCollectibles(); 
            InitializeCharacter();    
            InitializeGameObjects();  
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
            int visibleHeight = 32; //motong biar tophalf aja yg kelihatan (spritenya kegedean jir)

            // Kumpulan tiles nih
            var bottom1 = new Tile(new Point(1, 500), new Size(196, visibleHeight), visibleHeight);
            var bottom2 = new Tile(new Point(360, 500), new Size(256, visibleHeight), visibleHeight);
            var second1 = new Tile(new Point(580, 400), new Size(256, visibleHeight), visibleHeight);
            var third1 = new Tile(new Point(320, 300), new Size(256, visibleHeight), visibleHeight);
            var second2 = new Tile(new Point(1, 234), new Size(256, visibleHeight), visibleHeight);
            var fourth1 = new Tile(new Point(283, 109), new Size(200, visibleHeight), visibleHeight);
            var final = new Tile(new Point(644, 73), new Size(234, visibleHeight), visibleHeight);

            // Jangan lupa di add dulu
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
            // Coin manual
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
            // Enemy manual
            _enemies.Add(new TungTung(new Point(360, 452), 230)); // Bolak balik 230px
            _enemies.Add(new TungTung(new Point(283, 61), 150)); // 150px
        }
        private void InitializeCharacter()
        {
            _tralala = new Tralala(new Point(PlayerInitPosX, PlayerInitPosY), _tiles, this.ClientSize.Width, this.ClientSize.Height);
        }
        private void InitializeGameObjects()
        {
            // Clear just to be safe
            _gameObjects = new List<GameObject>();

            // masukin semua object biar gampang update & draw
            _gameObjects.AddRange(_tiles);
            _gameObjects.AddRange(_collectibles);
            _gameObjects.AddRange(_enemies);
            _gameObjects.Add(_tralala); // player paling terakhir biar di atas semuanya


        }
        private void InitializeTimer()
        {
            gameTimer = new System.Windows.Forms.Timer
            {
                Interval = 60  //FPS IS HERE
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

            // Check Coin collilsion
            foreach (var coin in _collectibles)
            {
                // check kalao coin belum di collect
                if (!coin.IsCollected && playerBounds.IntersectsWith(coin.Bounds))
                {
                    coin.OnCollected();
                    _coinsCollected++;
                }
            }

            // cek collision enemy
            foreach (var enemy in _enemies)
            {
                if (playerBounds.IntersectsWith(enemy.Bounds))
                {
                    // nge hit enemy? reset mas poke
                    _tralala.Reset();
                    break; 
                }
            }

            //  Clean up to improve performance katanya
            _gameObjects.RemoveAll(go => go is Collectible c && c.IsCollected);
        }
        private void LoadCustomFont()
        {
            // Pixel font nih
            PrivateFontCollection pfc = new PrivateFontCollection();

            using (var fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TralalaGame.Resources.5x7 MT Pixel.ttf"))
            {
                if (fontStream != null)
                {
                    // buffer buat baca font nya
                    byte[] fontdata = new byte[fontStream.Length];
                    fontStream.Read(fontdata, 0, (int)fontStream.Length);

                    // masukin font nya ke lib
                    GCHandle handle = GCHandle.Alloc(fontdata, GCHandleType.Pinned);
                    pfc.AddMemoryFont(handle.AddrOfPinnedObject(), fontdata.Length);
                    handle.Free();
                }
            }

            // object font baru
            _textfont = new Font(pfc.Families[0], 16, FontStyle.Regular);
            pfc.Dispose();
        }
        private void InitializeUI()
        {
            _framelabel = new TextFont
            {
                Text = $"COINS: 0/{_totalCoinsInLevel}",   
                Location = new Point(10, 10),
                AutoSize = true,
                Font = _textfont, // pakai font pixel
                ForeColor = Color.White,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(_framelabel);
        }

        // --- Game Loop ---
        private void GameTimer_Tick(object sender, System.EventArgs e)
        {
            // 1. Handle Input 
            _tralala.HandleInput(_leftPressed, _rightPressed, _jumpPressed, _shiftPressed);

            // 2. Update game objects
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Update();
            }

            // 3. Check player coillisons
            CheckInteractions();

            // 4. Reset jump 
            _jumpPressed = false;

            // 5. Update UI
            UpdateCoins();

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Method dipanggil kalo perlu ngegambar ulang form

            // 1. panggil base method untuk inisialisasi form
            base.OnPaint(e);

            // 3. buat ngegambar semua game objects
            foreach (var gameObject in _gameObjects)
            {
                // kita gak perlu ngecek gameObject itu Tralala atau Tile, soalnya semua class udah inherit dari GameObject
                // pakai point.empty karena kamera kita diem
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
                e.SuppressKeyPress = true; // bikin space ga nge scroll formnya
            }
            if (e.KeyCode == Keys.ShiftKey) _shiftPressed = true;
        }

        private void LevelForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A) _leftPressed = false;
            if (e.KeyCode == Keys.D) _rightPressed = false;
            if (e.KeyCode == Keys.ShiftKey) _shiftPressed = false;
        }

        private void UpdateCoins()
        {
            _framelabel.Text = $"COIN(S) COLLECTED: {_coinsCollected}/{_totalCoinsInLevel}";
        }
    }
}