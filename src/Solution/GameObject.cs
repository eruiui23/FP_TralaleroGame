using System.Drawing;
using System.Windows.Forms;

namespace TralalaGame
{
    public abstract class GameObject
    {
        // --- Common Properties ---
        // Semua gameobject punya PioctureBox 
        public PictureBox Box { get; protected set; }
        public Point Position { get => Box.Location; set => Box.Location = value; }
        public Size Size { get => Box.Size; set => Box.Size = value; }

        // --- Abstract Methods  ---
        // Update buat game object
        public abstract void Update();

        // --- Virtual Methods ---
        //buat di override di class turunan
        public virtual void Draw(Graphics g, Point cameraPosition)
        {
            // Buat gampang kalau ada object yang ga perlu di draw
        }

        // Constructor
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