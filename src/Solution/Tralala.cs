using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace TralalaGame
{
    // PlayerState enum buat animasi sesuai spritesheet
    public enum PlayerState 
    {
        FallRight = 0,
        IdleRight = 1,
        RunRight = 2,
        WalkRight = 3,
        JumpRight = 4,
        FallLeft = 5,
        IdleLeft = 6,
        RunLeft = 7,
        WalkLeft = 8,
        JumpLeft = 9
    }

    public class Tralala : GameObject, ICollidable
    {
        // --- Sprite & Animation ---
        private const int PWidth = 58;
        private const int PHeight = 40;
        private Image _spriteSheet;
        private Dictionary<PlayerState, int> _animationFrames;
        private int _currentFrame;
        private PlayerState _animationState;
        private char _facingDirection = 'R';

        // --- Physics & Movement ---
        private Point _velocity;
        private bool _isJumping = false;
        private bool _isMovingRight = false;
        private bool _isMovingLeft = false;
        private bool _isRunning = false;
        private List<Tile> _levelTiles;
        private int _levelWidth;
        private int _levelHeight;
        private Point _startPosition;
        private int _groundLevelY;

        // --- Physics Constants ---
        private const int WalkSpeed = 6;
        private const int RunSpeed = 10;
        private const int StraightJumpSpeed = 25;
        private const int RunningJumpSpeed = 22;
        private const int Gravity = 2;

        public Rectangle Bounds => this.Box.Bounds;
        public Tralala(Point startPosition, List<Tile> tiles, int levelWidth, int levelHeight) : base(startPosition, new Size(PWidth, PHeight))
        {
            
            _spriteSheet = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("TralalaGame.Resources.Assets.png"));
            InitializeAnimationData();

            _levelTiles = tiles; // NEW: Store the tiles
            _animationState = PlayerState.IdleRight;
            _currentFrame = 0;
            _velocity = Point.Empty;
            _groundLevelY = 600; // Keep the initial ground level

            this.Box.Paint += PlayerPictureBox_Paint;
            _levelWidth = levelWidth;
            _levelHeight = levelHeight;
            _startPosition = startPosition;
        }

        public PictureBox GetPictureBox() => this.Box;

        public override void Draw(Graphics g, Point cameraPosition)
        {
            //posisi player
            int screenX = this.Position.X - cameraPosition.X;
            int screenY = this.Position.Y - cameraPosition.Y;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            Rectangle sourceRect = new Rectangle(
                _currentFrame * PWidth,
                (int)_animationState * PHeight,
                PWidth,
                PHeight
            );
            // ini buat gambar playernya
            g.DrawImage(
                _spriteSheet,
                new Rectangle(screenX, screenY, PWidth, PHeight),
                sourceRect,
                GraphicsUnit.Pixel
            );
        }

        // 
        // ini untuk handle input dari keyboard
        public void HandleInput(bool left, bool right, bool jump, bool run)
        {
            _isMovingLeft = left;
            _isMovingRight = right;
            _isRunning = run;

            // kalau pencet A dan D bareng, stop gerak 
            if (_isMovingLeft && _isMovingRight)
            {
                _isMovingLeft = false;
                _isMovingRight = false;
            }

            // --- Jump Logic ---
            if (jump && !_isJumping)
            {
                _isJumping = true;

                // cek karakter lagi gerak kiri atau kanan
                if (_isMovingLeft || _isMovingRight)
                {
                    // kalau iya, perform larilompat (lari + lompat)
                    _velocity.Y = -RunningJumpSpeed;
                }
                else
                {
                    // kalau gaada gerak, cuma penceet spaasi baru gerak. StraightJump
                    _velocity.Y = -StraightJumpSpeed;
                }
            }
        }

        // In Tralala.cs

        public override void Update()
        {
            // Gravitasi nih buat karakter
            _velocity.X = 0;
            if (_isMovingLeft)
            {
                _velocity.X = _isRunning ? -RunSpeed : -WalkSpeed;
            }
            if (_isMovingRight)
            {
                _velocity.X = _isRunning ? RunSpeed : WalkSpeed;
            }

            // Gravity
            _velocity.Y += Gravity;

            //ini buat simpen posisi awal char, buat apa? biar bisa cek collision
            Point originalPosition = this.Box.Location;

            // vertical movement
            this.Box.Top += _velocity.Y;
            bool onGround = false;
            bool hitCeiling = false;

            // vertical collision
            foreach (var tile in _levelTiles)
            {
                Rectangle playerBounds = this.Box.Bounds;
                Rectangle tileBounds = tile.Bounds;

                if (playerBounds.IntersectsWith(tileBounds))
                {
                    // cek original postion, nih kita dari bawah tile apa dari atas (HeadBump atau landing)
                    //ini buat head bump
                    if (_velocity.Y < 0 && originalPosition.Y >= tileBounds.Bottom)
                    {
                        this.Box.Top = tileBounds.Bottom;
                        _velocity.Y = 0;
                        hitCeiling = true;
                        break;
                    }
                    // nah ini buat kalau dari atas (landing)
                    else if (_velocity.Y > 0 && (originalPosition.Y + this.Box.Height) <= tileBounds.Top)
                    {
                        this.Box.Top = tileBounds.Top - this.Box.Height;
                        _velocity.Y = 0;
                        _isJumping = false;
                        onGround = true;
                        break;
                    }
                }
            }

            // kalau gaada collision dari atas, berarti kita bisa gerak horizontal
            if (!hitCeiling)
            {
                this.Box.Left += _velocity.X;

                // Check horizontal collisions
                foreach (var tile in _levelTiles)
                {
                    Rectangle playerBounds = this.Box.Bounds;
                    Rectangle tileBounds = tile.Bounds;

                    if (playerBounds.IntersectsWith(tileBounds))
                    {
                        // buat cek collision dari kiri atau kanan
                        if (originalPosition.Y + this.Box.Height > tileBounds.Top &&
                            originalPosition.Y < tileBounds.Bottom)
                        {
                            if (_velocity.X > 0)
                            {
                                this.Box.Left = tileBounds.Left - this.Box.Width;
                            }
                            else if (_velocity.X < 0)
                            {
                                this.Box.Left = tileBounds.Right;
                            }
                            _velocity.X = 0;
                            break;
                        }
                    }
                }
            }

            // dinding windows (kanan, kiri, bawah)
            if (this.Box.Left < 0) { this.Box.Left = 0; }
            if (this.Box.Right > _levelWidth) { this.Box.Left = _levelWidth - this.Box.Width; }
            if (this.Box.Bottom > _levelHeight)
            {
                Reset();
            }

            UpdateAnimation(onGround);
        }

        public void Reset()
        {
            // Reset posisi karakter (Respawn)
            this.Box.Location = _startPosition;

            // Reset semua ke awal
            _velocity = Point.Empty;
            _isJumping = false;
            _animationState = PlayerState.IdleRight;
            _facingDirection = 'R';
        }
        private void UpdateAnimation(bool onGround)
        {
            PlayerState previousState = _animationState;

            // ngarah mana nih karakter
            if (_isMovingLeft) _facingDirection = 'L';
            if (_isMovingRight) _facingDirection = 'R';

            // animation state
            if (!onGround) // biar tau lagi di udara atau ga
            {
                _isJumping = true; // di uadara berarti lagi lompat
                if (_facingDirection == 'L')
                {
                    _animationState = (_velocity.Y < 0) ? PlayerState.JumpLeft : PlayerState.FallLeft;
                }
                else // Facing Right
                {
                    _animationState = (_velocity.Y < 0) ? PlayerState.JumpRight : PlayerState.FallRight;
                }
            }
            else if (_isMovingLeft)
            {
                _animationState = _isRunning ? PlayerState.RunLeft : PlayerState.WalkLeft;
            }
            else if (_isMovingRight)
            {
                _animationState = _isRunning ? PlayerState.RunRight : PlayerState.WalkRight;
            }
            else // Idle
            {
                _animationState = (_facingDirection == 'L') ? PlayerState.IdleLeft : PlayerState.IdleRight;
            }

            // kalau state berubah, reset frame ke 0
            if (previousState != _animationState)
            {
                _currentFrame = 0;
            }

            // kalau gaada ini, animasi ga jalan
            int totalFrames = _animationFrames[_animationState];
            _currentFrame = (_currentFrame + 1) % totalFrames;

            this.Box.Invalidate();
        }

        // ini buat inisialisasi data animasi
        private void InitializeAnimationData()
        {
            _animationFrames = new Dictionary<PlayerState, int>
            {
                { PlayerState.FallRight, 2 }, { PlayerState.IdleRight, 4 }, { PlayerState.RunRight, 5 },
                { PlayerState.WalkRight, 5 }, { PlayerState.JumpRight, 3 }, { PlayerState.FallLeft, 2 },
                { PlayerState.IdleLeft, 4 }, { PlayerState.RunLeft, 5 }, { PlayerState.WalkLeft, 5 },
                { PlayerState.JumpLeft, 3 }
            };
        }

        // ini buat ngegambar playernya di PictureBox biar ga pake Draw method 
        private void PlayerPictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            Rectangle sourceRect = new Rectangle(
                _currentFrame * PWidth,
                (int)_animationState * PHeight,
                PWidth,
                PHeight
            );
            Rectangle destinationRect = new Rectangle(0, 0, PWidth, PHeight);
            e.Graphics.DrawImage(
                _spriteSheet,
                destinationRect,
                sourceRect,
                GraphicsUnit.Pixel
            );
        }
    }
}