using System.Drawing;
using System.Windows.Forms;

namespace SpriteAnimation
{
    public class LevelForm : Form
    {
        // --- Form Constants ---
        private const int PlayerInitPosX = 100;
        private const int PlayerInitPosY = 400; // Adjusted starting Y to be on the "ground"

        // --- Class Variables ---
        private System.Windows.Forms.Timer gameTimer;
        private Label _framelabel;
        private long _totalFrameCount;
        private Tralala _tralala;

        public LevelForm()
        {
            // --- Form Initialization ---
            InitializeLevel();
            InitializeCharacter();
            InitializeTimer();
            InitializeInput(); // Set up keyboard listeners

            // --- UI Elements ---
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

        private void InitializeLevel()
        {
            this.Text = "Tralala Test Drive";
            this.Size = new Size(800, 600);
            this.BackColor = Color.LightSkyBlue;

            // *** IMPORTANT: This prevents flickering and makes animation smooth ***
            this.DoubleBuffered = true;
        }

        private void InitializeCharacter()
        {
            // Create the player character at its starting position
            _tralala = new Tralala(new Point(PlayerInitPosX, PlayerInitPosY));

            // *** IMPORTANT: This adds the character to the screen so it's visible ***
            this.Controls.Add(_tralala.GetPictureBox());
        }

        private void InitializeTimer()
        {
            gameTimer = new System.Windows.Forms.Timer();
            // Set the interval to ~50 times per second for smooth updates
            gameTimer.Interval = 50;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void InitializeInput()
        {
            // Listen for key presses and releases
            this.KeyDown += LevelForm_KeyDown;
            this.KeyUp += LevelForm_KeyUp;
        }

        // --- Game Loop ---
        // This method is the "heartbeat" of your game. It runs on every timer tick.
        private void GameTimer_Tick(object sender, System.EventArgs e)
        {
            // Tell the character to update its physics and animation
            _tralala.Update();

            // Update your frame counter label
            UpdateFrameCount();
        }

        // --- Input Handling ---
        private void LevelForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Tell the character if the shift key is being held down
            _tralala.SetRunning(e.Shift);

            // Tell the character to START moving
            if (e.KeyCode == Keys.A)
            {
                _tralala.StartMove('L');
            }
            else if (e.KeyCode == Keys.D)
            {
                _tralala.StartMove('R');
            }

            // Tell the character to JUMP
            if (e.KeyCode == Keys.Space)
            {
                _tralala.Jump();
                // This prevents the annoying "ding" sound when pressing space
                e.SuppressKeyPress = true;
            }
        }

        private void LevelForm_KeyUp(object sender, KeyEventArgs e)
        {
            // Update the running state in case Shift was the key that was released
            _tralala.SetRunning(e.Shift);

            // Tell the character to STOP moving
            if (e.KeyCode == Keys.A)
            {
                _tralala.StopMove('L');
            }
            else if (e.KeyCode == Keys.D)
            {
                _tralala.StopMove('R');
            }
        }

        private void UpdateFrameCount()
        {
            _totalFrameCount++;
            _framelabel.Text = "Total frames: " + _totalFrameCount;
        }
    }
}