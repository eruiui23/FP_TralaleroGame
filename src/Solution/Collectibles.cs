using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace TralalaGame
{
    public class Collectible : GameObject, ICollidable
    {
        // Properties 
        public bool IsCollected { get; private set; }
        public Rectangle Bounds => this.Box.Bounds;
        private static Image _spriteSheet;

        private const int FRAME_WIDTH = 21;   // Lebar coin
        private const int FRAME_HEIGHT = 26;  // Tinggi coin
        private const int TOTAL_FRAMES = 6;   // Frame
        private int _currentFrame;

        // Constructor 
        public Collectible(Point position) : base(position, new Size(FRAME_WIDTH, FRAME_HEIGHT))
        {
            IsCollected = false;
            _currentFrame = 0;

            if (_spriteSheet == null)
            {
                _spriteSheet = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("TralalaGame.Resources.coin.png"));
            }
          
            this.Box.Paint += Collectible_Paint; 
            this.Box.BackColor = Color.Transparent;
        }

        public override void Draw(Graphics g, Point cameraPosition)
        {
            if (this.IsCollected) return; // Kalau ke collect, dont draw

            int screenX = this.Position.X - cameraPosition.X;
            int screenY = this.Position.Y - cameraPosition.Y;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            Rectangle sourceRect = new Rectangle(_currentFrame * FRAME_WIDTH, 0, FRAME_WIDTH, FRAME_HEIGHT);

            g.DrawImage(
                _spriteSheet,
                new Rectangle(screenX, screenY, FRAME_WIDTH, FRAME_HEIGHT),
                sourceRect,
                GraphicsUnit.Pixel
            );
        }

        // NEW: Paint Event Handler 
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

        // MODIFIED: Update method now handles animation 
        public override void Update()
        {
            //Animation
            _currentFrame = (_currentFrame + 1) % TOTAL_FRAMES;

            // Idk i forgot whaat is this, but I think ini nge refresh PictureBoxnya
            this.Box.Invalidate();
        }
    }
}