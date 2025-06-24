using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TralalaGame
{
    public class Tile : GameObject, ICollidable
    {
        private static Image _tileSprite;
        private const int TileWidth = 64;
        private const int TileHeight = 64;
        private int _visibleHeight; // How much of the sprite is visible
        private Rectangle _collisionBounds; // Separate collision rectangle

        public Rectangle Bounds => _collisionBounds;

        public Tile(Point location, Size size, int visibleHeight = 32) : base(location, size)
        {
            _visibleHeight = visibleHeight;

            // Set collision bounds to match the visible area
            _collisionBounds = new Rectangle(
                location.X,
                location.Y,
                size.Width,
                _visibleHeight
            );

            if (_tileSprite == null)
            {
                _tileSprite = Resource.Sand;
            }

            this.Box.BackColor = Color.Transparent;
        }

        public override void Draw(Graphics g, Point cameraPosition)
        {
            // Calculate the tile's position on the screen relative to the camera
            int screenX = this.Position.X - cameraPosition.X;
            int screenY = this.Position.Y - cameraPosition.Y;

            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            // The width of the tile platform comes from the Size property of the GameObject
            int tileCount = this.Size.Width / TileWidth;

            for (int i = 0; i < tileCount; i++)
            {
                // The destination is now based on the calculated screen position
                var destinationRect = new Rectangle(screenX + (i * TileWidth), screenY, TileWidth, _visibleHeight);
                var sourceRect = new Rectangle(0, 0, TileWidth, _visibleHeight);
                g.DrawImage(_tileSprite, destinationRect, sourceRect, GraphicsUnit.Pixel);
            }
        }

        public override void Update()
        {
        }
    }
}