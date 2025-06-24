using System.Collections.Generic; 
using System.Drawing;
using System.Drawing.Drawing2D;   
using System.Reflection;
using System.Windows.Forms;

namespace TralalaGame
{
    // --- Enum kayak player ---
    public enum EnemyState
    {
        WalkRight = 1,
        WalkLeft = 0
    }

    public class TungTung : Enemy
    {
        public override Rectangle Bounds => this.Box.Bounds;
        private static Image _spriteSheet;

        // --- Movement ---
        private int _speed;
        private int _direction = 1;
        private int _patrolStartX;
        private int _patrolEndX;

        // --- NEW: Animation ---
        private const int FRAME_WIDTH = 31; // lebar frame
        private const int FRAME_HEIGHT = 47; // tinggi frame
        private Dictionary<EnemyState, int> _animationFrames;
        private EnemyState _animationState;
        private int _currentFrame;

        // --- Constructor ---
        public TungTung(Point position, int patrolDistance) : base(position, new Size(FRAME_WIDTH, FRAME_HEIGHT))
        {
            _speed = 4;
            _patrolStartX = position.X;
            _patrolEndX = position.X + patrolDistance;

            if (_spriteSheet == null)
            {
                _spriteSheet = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("TralalaGame.Resources.tungtungtung-Sheet.png"));
            }

            InitializeAnimationData();
            _animationState = EnemyState.WalkRight;
            _currentFrame = 0;

            //buat ngegambar di picturebox
            this.Box.Paint += Enemy_Paint; 
            this.Box.BackColor = Color.Transparent;
        }

        // biar bisa dipanggil di GameObject
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

        // --- method buar animasiin ---
        private void InitializeAnimationData()
        {
            _animationFrames = new Dictionary<EnemyState, int>
            {
                { EnemyState.WalkRight, 3 }, // Ada 3 fram yg jalan ke kanan
                { EnemyState.WalkLeft, 3 }   // ada 3 frame yg jalan ke kiri
            };
        }

        // --- Paint Event Handler ---
        // Ini yang dipanggil pas PictureBox mau digambar
        private void Enemy_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            Rectangle sourceRect = new Rectangle(
                _currentFrame * FRAME_WIDTH,
                (int)_animationState * FRAME_HEIGHT, // pkai state buat nentuin baris yg mo dipake
                FRAME_WIDTH,
                FRAME_HEIGHT
            );

            Rectangle destinationRect = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT);

            e.Graphics.DrawImage(_spriteSheet, destinationRect, sourceRect, GraphicsUnit.Pixel);
        }

        // ---Update Method ---
        public override void Update()
        {
            // --- Movement Logic ---
            this.Box.Left += _speed * _direction;

            if (this.Box.Left <= _patrolStartX)
            {
                _direction = 1; // Ke knana
                this.Box.Left = _patrolStartX;
            }
            else if (this.Box.Left >= _patrolEndX)
            {
                _direction = -1; // ke kiri
                this.Box.Left = _patrolEndX;
            }

            // --- Animation Logic ---
            UpdateAnimation();
        }

        // --- Helper method kayak di tralalaa ---
        private void UpdateAnimation()
        {
            EnemyState previousState = _animationState;

            //tentuin animsi dari arah geraknya
            _animationState = (_direction > 0) ? EnemyState.WalkRight : EnemyState.WalkLeft;

            // kalau ganti state, reset framenya ke 0
            if (previousState != _animationState)
            {
                _currentFrame = 0;
            }

            // buat majuin frame animasi
            int totalFrames = _animationFrames[_animationState];
            _currentFrame = (_currentFrame + 1) % totalFrames;

            // Repaint PictureBox untuk update animasi
            this.Box.Invalidate();
        }
    }
}