using System.Drawing;

namespace TralalaGame
{
    // Yg bisa di collide sama object yg laen
    public interface ICollidable
    {
        Rectangle Bounds { get; }
    }
}