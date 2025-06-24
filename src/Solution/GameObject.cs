using System.Drawing;
using System.Windows.Forms;

namespace TralalaGame
{
    // It exists only to be inherited from.
    public abstract class GameObject
    {
        // --- Common Properties ---
        // All game objects have a position and a PictureBox to represent them.
        public PictureBox Box { get; protected set; }
        public Point Position { get => Box.Location; set => Box.Location = value; }
        public Size Size { get => Box.Size; set => Box.Size = value; }

        // --- Abstract Methods (Must be implemented by children) ---
        // Every game object must have its own way of updating its state.
        public abstract void Update();

        // --- Virtual Methods (Can be optionally overridden by children) ---
        // We provide a basic drawing behavior, but children can change it if needed.
        public virtual void Draw(Graphics g, Point cameraPosition)
        {
            // By default, do nothing. The PictureBox handles its own painting.
            // This is here for future flexibility.
        }

        // Constructor to initialize common properties
        protected GameObject(Point position, Size size)
        {
            Box = new PictureBox
            {
                Location = position,
                Size = size
            };
        }
    }
}