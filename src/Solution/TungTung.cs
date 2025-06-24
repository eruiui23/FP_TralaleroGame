using System.Collections.Generic; // --- NEW: Add this for Dictionary
using System.Drawing;
using System.Drawing.Drawing2D;   // --- NEW: Add this for InterpolationMode
using System.Reflection;
using System.Windows.Forms;

namespace TralalaGame
{
    // --- NEW: Enum for enemy states, just like PlayerState ---
    public enum EnemyState
    {
        WalkRight = 1,
        WalkLeft = 0
    }

    public class TungTung : Enemy
    {
        public override Rectangle Bounds => this.Box.Bounds;
        private static Image _spriteSheet;

        // --- Movement Variables ---
        private int _speed;
        private int _direction = 1;
        private int _patrolStartX;
        private int _patrolEndX;

        // --- NEW: Animation Properties ---
        private const int FRAME_WIDTH = 31; // Width of one enemy frame
        private const int FRAME_HEIGHT = 47; // Height of one enemy frame
        private Dictionary<EnemyState, int> _animationFrames;
        private EnemyState _animationState;
        private int _currentFrame;

        // --- MODIFIED: Constructor ---
        public TungTung(Point position, int patrolDistance)
            : base(position, new Size(FRAME_WIDTH, FRAME_HEIGHT))
        {
            _speed = 4;
            _patrolStartX = position.X;
            _patrolEndX = position.X + patrolDistance;

            if (_spriteSheet == null)
            {
                // IMPORTANT: Add an "EnemySpriteSheet.png" to your resources.
                _spriteSheet = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("TralalaGame.Resources.tungtungtung-Sheet.png"));
            }

            InitializeAnimationData();
            _animationState = EnemyState.WalkRight;
            _currentFrame = 0;

            // --- KEY CHANGE: We now handle drawing ourselves ---
            // this.Box.Image = _sprite; // DELETE THIS LINE
            this.Box.Paint += Enemy_Paint; // ADD THIS LINE
            this.Box.BackColor = Color.Transparent;
        }

        public override void Draw(Graphics g, Point cameraPosition)
        {
            int screenX = this.Position.X - cameraPosition.X;
            int screenY = this.Position.Y - cameraPosition.Y;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            Rectangle sourceRect = new Rectangle(
                _currentFrame * FRAME_WIDTH,
                (int)_animationState * FRAME_HEIGHT,
                FRAME_WIDTH,
                FRAME_HEIGHT
            );

            g.DrawImage(_spriteSheet,
                new Rectangle(screenX, screenY, FRAME_WIDTH, FRAME_HEIGHT),
                sourceRect,
                GraphicsUnit.Pixel);
        }

        // --- NEW: Method to define animation lengths ---
        private void InitializeAnimationData()
        {
            _animationFrames = new Dictionary<EnemyState, int>
            {
                { EnemyState.WalkRight, 3 }, // Assuming 4 frames for walking right
                { EnemyState.WalkLeft, 3 }   // Assuming 4 frames for walking left
            };
        }

        // --- NEW: Paint Event Handler ---
        private void Enemy_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            Rectangle sourceRect = new Rectangle(
                _currentFrame * FRAME_WIDTH,
                (int)_animationState * FRAME_HEIGHT, // Use state to pick the row
                FRAME_WIDTH,
                FRAME_HEIGHT
            );

            Rectangle destinationRect = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT);

            e.Graphics.DrawImage(_spriteSheet, destinationRect, sourceRect, GraphicsUnit.Pixel);
        }

        // --- MODIFIED: Update method now also handles animation state ---
        public override void Update()
        {
            // --- Movement Logic (same as before) ---
            this.Box.Left += _speed * _direction;

            if (this.Box.Left <= _patrolStartX)
            {
                _direction = 1; // Move right
                this.Box.Left = _patrolStartX;
            }
            else if (this.Box.Left >= _patrolEndX)
            {
                _direction = -1; // Move left
                this.Box.Left = _patrolEndX;
            }

            // --- Animation Logic (new) ---
            UpdateAnimation();
        }

        // --- NEW: Helper method for animation, just like in Tralala ---
        private void UpdateAnimation()
        {
            EnemyState previousState = _animationState;

            // Determine current animation state based on direction
            _animationState = (_direction > 0) ? EnemyState.WalkRight : EnemyState.WalkLeft;

            // If the state changed (e.g., turned around), reset the frame to the beginning
            if (previousState != _animationState)
            {
                _currentFrame = 0;
            }

            // Advance the frame
            int totalFrames = _animationFrames[_animationState];
            _currentFrame = (_currentFrame + 1) % totalFrames;

            // Tell the PictureBox to repaint itself
            this.Box.Invalidate();
        }
    }
}