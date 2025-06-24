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
        private int _visibleHeight; 
        private Rectangle _collisionBounds; 

        public Rectangle Bounds => _collisionBounds;

        public Tile(Point location, Size size, int visibleHeight = 32) : base(location, size)
        {
            _visibleHeight = visibleHeight;

            // collision bounds
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
            // tile position
            int screenX = this.Position.X - cameraPosition.X;
            int screenY = this.Position.Y - cameraPosition.Y;

            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            // ada brp ssquare tile per platform objeect, nah itu direpeat. Harusnya gitu
            int tileCount = this.Size.Width / TileWidth;

            for (int i = 0; i < tileCount; i++)
            {
                // kalau ga di repeat, tile nya ga keliatan
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