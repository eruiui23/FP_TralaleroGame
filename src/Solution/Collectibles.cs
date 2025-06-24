using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace TralalaGame
{
    public class Collectible : GameObject, ICollidable
    {
        // --- Properties ---
        public bool IsCollected { get; private set; }
        public Rectangle Bounds => this.Box.Bounds;
        private static Image _spriteSheet;

        private const int FRAME_WIDTH = 21;   // The width of a single frame
        private const int FRAME_HEIGHT = 26;  // The height of a single frame
        private const int TOTAL_FRAMES = 6;   // How many frames are in the coin's animation
        private int _currentFrame;

        // --- Constructor ---
        public Collectible(Point position) : base(position, new Size(FRAME_WIDTH, FRAME_HEIGHT))
        {
            IsCollected = false;
            _currentFrame = 0;

            if (_spriteSheet == null)
            {
                // IMPORTANT: Make sure you have "CoinSpriteSheet.png" in your resources.
                _spriteSheet = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("TralalaGame.Resources.coin.png"));
            }
            // --- KEY CHANGE: We now handle drawing ourselves ---
            // this.Box.Image = _sprite; // DELETE THIS LINE
            this.Box.Paint += Collectible_Paint; // ADD THIS LINE
            this.Box.BackColor = Color.Transparent;
        }

        public override void Draw(Graphics g, Point cameraPosition)
        {
            if (this.IsCollected) return; // Don't draw if collected

            int screenX = this.Position.X - cameraPosition.X;
            int screenY = this.Position.Y - cameraPosition.Y;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            Rectangle sourceRect = new Rectangle(
                _currentFrame * FRAME_WIDTH, 0, FRAME_WIDTH, FRAME_HEIGHT);

            g.DrawImage(
                _spriteSheet,
                new Rectangle(screenX, screenY, FRAME_WIDTH, FRAME_HEIGHT),
                sourceRect,
                GraphicsUnit.Pixel
            );
        }

        // --- NEW: Paint Event Handler ---
        private void Collectible_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            Rectangle sourceRect = new Rectangle(
                _currentFrame * FRAME_WIDTH, // X position on the sprite sheet
                0,                           // Y position (top row)
                FRAME_WIDTH,
                FRAME_HEIGHT
            );

            Rectangle destinationRect = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT);

            e.Graphics.DrawImage(
                _spriteSheet,
                destinationRect,
                sourceRect,
                GraphicsUnit.Pixel
            );
        }

        public void OnCollected()
        {
            IsCollected = true;
            this.Box.Visible = false;
        }

        // --- MODIFIED: Update method now handles animation ---
        public override void Update()
        {
            // Advance to the next frame, looping back to 0 at the end
            _currentFrame = (_currentFrame + 1) % TOTAL_FRAMES;

            // Tell the PictureBox it needs to be redrawn to show the new frame
            this.Box.Invalidate();
        }
    }
}