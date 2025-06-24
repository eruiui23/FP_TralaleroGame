using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TralalaGame
{
    // An abstract base class for all enemy types.
    public abstract class Enemy : GameObject, ICollidable
    {
        public abstract Rectangle Bounds { get; }
        protected Enemy(Point position, Size size) : base(position, size) { }
    }
}
