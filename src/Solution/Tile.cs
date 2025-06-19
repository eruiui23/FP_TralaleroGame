using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using TralalaGame;


namespace TralalaGame
{
    public class Tile
    {
        public PictureBox Box { get; private set; }
        public Rectangle Bounds => Box.Bounds;

        // NEW: Store the tile image.
        // We make it 'static' so the image is only loaded from resources once,
        // saving memory no matter how many tiles you create.
        private static Image _tileSprite;
        private const int TileWidth = 64;
        private const int TileHeight = 64;

        public Tile(Point location, Size size)
        {
            // NEW: Load the sprite from resources if it hasn't been loaded yet.
            if (_tileSprite == null)
            {
                _tileSprite = Resource.Sand;
            }

            Box = new PictureBox
            {
                Location = location,
                Size = size,
                // MODIFIED: Make the PictureBox background transparent, as we are custom drawing.
                BackColor = Color.Blue
            };

            // NEW: We will handle the drawing ourselves in the Paint event.
            Box.Paint += Tile_Paint;
        }

        // NEW: This method is called whenever the tile needs to be drawn.
        private void Tile_Paint(object sender, PaintEventArgs e)
        {
            // Use NearestNeighbor for a sharp, pixelated look, same as the player.
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            // Calculate how many times we need to draw the tile horizontally.
            int tileCount = Box.Width / TileWidth;

            // Loop and draw the tile sprite side-by-side.
            for (int i = 0; i < tileCount; i++)
            {
                // Define where on the PictureBox to draw this specific tile copy.
                var destinationRect = new Rectangle(i * TileWidth, 0, TileWidth, TileHeight);

                // Define which part of the source image to draw (the whole 64x64 sprite).
                var sourceRect = new Rectangle(0, 0, TileWidth, TileHeight);

                e.Graphics.DrawImage(
                    _tileSprite,          // The image to draw
                    destinationRect,      // Where to draw it on the control
                    sourceRect,           // Which part of the image to use
                    GraphicsUnit.Pixel
                );
            }
        }
    }
}