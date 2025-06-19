using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TralalaGame
{
    public class LevelForm : Form
    {
        // --- Form Constants ---
        private const int PlayerInitPosX = 100;
        private const int PlayerInitPosY = 360;

        // --- Class Variables ---
        private System.Windows.Forms.Timer gameTimer;
        private Label _framelabel;
        private long _totalFrameCount;
        private Tralala _tralala;
        private List<Tile> _tiles;

        // --- NEW: Input state tracking belongs to the form ---
        private bool _leftPressed = false;
        private bool _rightPressed = false;
        private bool _jumpPressed = false;
        private bool _shiftPressed = false;

        public LevelForm()
        {
            InitializeLevel();
            InitializeTiles();
            InitializeCharacter();
            InitializeTimer();
            InitializeInput(); // Set up keyboard listeners
            InitializeUI();
        }

        private void InitializeLevel()
        {
            this.Text = "Tralala Test Drive (Optimized)";
            this.Size = new Size(800, 600);
            this.BackColor = Color.LightSkyBlue;
            this.DoubleBuffered = true;
        }

        private void InitializeTiles()
        {
            _tiles = new List<Tile>();

            // Add a few platforms
            var bottom1 = new Tile(new Point(1, 500), new Size(256, 64));
            var bottom2 = new Tile(new Point(384, 500), new Size(840, 64));


            _tiles.Add(bottom1);
            _tiles.Add(bottom2);


            // Add the tile PictureBoxes to the form's controls
            foreach (var tile in _tiles)
            {
                this.Controls.Add(tile.Box);
            }
        }

        private void InitializeCharacter()
        {
            _tralala = new Tralala(new Point(PlayerInitPosX, PlayerInitPosY), _tiles, this.ClientSize.Width, this.ClientSize.Height);
            this.Controls.Add(_tralala.GetPictureBox());
        }

        private void InitializeTimer()
        {
            gameTimer = new System.Windows.Forms.Timer
            {
                Interval = 50  // Adjusted for a smoother ~50 FPS update rate
            };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void InitializeInput()
        {
            this.KeyDown += LevelForm_KeyDown;
            this.KeyUp += LevelForm_KeyUp;
        }

        private void InitializeUI()
        {
            _framelabel = new Label
            {
                Text = "Total frames: 0",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Arial", 16),
                ForeColor = Color.Black
            };
            this.Controls.Add(_framelabel);
        }

        // --- Game Loop (Now much cleaner!) ---
        private void GameTimer_Tick(object sender, System.EventArgs e)
        {
            // 1. Pass the current input state to the player object.
            _tralala.HandleInput(_leftPressed, _rightPressed, _jumpPressed, _shiftPressed);

            // 2. Tell the character to update its physics and animation.
            _tralala.Update();

            // 3. Reset the jump flag after it's been processed to prevent continuous jumping.
            _jumpPressed = false;

            // 4. Update UI
            UpdateFrameCount();
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

        private void UpdateFrameCount()
        {
            _totalFrameCount++;
            _framelabel.Text = "Total frames: " + _totalFrameCount;
        }
    }
}