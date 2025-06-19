using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace TralalaGame
{
    // Your PlayerState enum can remain the same
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

    public class Tralala
    {
        // --- Sprite & Animation Configuration ---
        private const int PWidth = 64;
        private const int PHeight = 64;
        private PictureBox _playerPictureBox;
        private Image _spriteSheet;
        private Dictionary<PlayerState, int> _animationFrames;
        private int _currentFrame;
        private PlayerState _animationState;
        private char _facingDirection = 'R';

        // --- Physics & Movement Variables ---
        private Point _velocity;
        private bool _isJumping = false;
        private bool _isMovingRight = false;
        private bool _isMovingLeft = false;
        private bool _isRunning = false;
        private List<Tile> _levelTiles;
        private int _levelWidth;
        private int _levelHeight;
        private Point _startPosition;

        // --- Physics Constants ---
        private const int WalkSpeed = 6;
        private const int RunSpeed = 10;
        private const int JumpSpeed = 24;
        private const int Gravity = 2;
        private int _groundLevelY;
        public Tralala(Point startPosition, List<Tile> tiles, int levelWidth, int levelHeight)
        {
            _spriteSheet = Image.FromStream(
    Assembly.GetExecutingAssembly()
    .GetManifestResourceStream("TralalaGame.Resources.Assets.png"));
            InitializeAnimationData();

            _levelTiles = tiles; // NEW: Store the tiles
            _animationState = PlayerState.IdleRight;
            _currentFrame = 0;
            _velocity = Point.Empty;
            _groundLevelY = 600; // Keep the initial ground level

            _playerPictureBox = new PictureBox
            {
                Size = new Size(PWidth, PHeight),
                Location = startPosition,
                BackColor = Color.Transparent,
            };
            _playerPictureBox.Paint += PlayerPictureBox_Paint;
            _levelWidth = levelWidth;
            _levelHeight = levelHeight;
            _startPosition = startPosition;
        }

        public PictureBox GetPictureBox() => _playerPictureBox;

        // --- NEW: A single method to handle all input states ---
        // This is called by the form on every tick BEFORE Update()
        public void HandleInput(bool left, bool right, bool jump, bool run)
        {
            _isMovingLeft = left;
            _isMovingRight = right;
            _isRunning = run;

            // Prevent moving in both directions at once
            if (_isMovingLeft && _isMovingRight)
            {
                _isMovingLeft = false;
                _isMovingRight = false;
            }

            if (jump && !_isJumping)
            {
                _isJumping = true;
                _velocity.Y = -JumpSpeed;
            }
        }

        // In Tralala.cs

        public void Update()
        {
            // 1. Apply Horizontal Velocity from input
            _velocity.X = 0;
            if (_isMovingLeft)
            {
                _velocity.X = _isRunning ? -RunSpeed : -WalkSpeed;
            }
            if (_isMovingRight)
            {
                _velocity.X = _isRunning ? RunSpeed : WalkSpeed;
            }

            // 2. Apply Gravity
            _velocity.Y += Gravity;

            // --- SEPARATE AXIS COLLISION LOGIC ---

            // 3. Handle Horizontal Movement and Collision
            _playerPictureBox.Left += _velocity.X;

            foreach (var tile in _levelTiles)
            {
                Rectangle playerBounds = _playerPictureBox.Bounds;
                Rectangle tileBounds = tile.Bounds;

                if (playerBounds.IntersectsWith(tileBounds))
                {
                    if (_velocity.X > 0) { _playerPictureBox.Left = tileBounds.Left - playerBounds.Width; }
                    else if (_velocity.X < 0) { _playerPictureBox.Left = tileBounds.Right; }
                    break;
                }
            }
            if (_playerPictureBox.Left < 0)
            {
                _playerPictureBox.Left = 0;
            }
            if (_playerPictureBox.Right > _levelWidth)
            {
                // Position the character so its right edge is at the boundary
                _playerPictureBox.Left = _levelWidth - _playerPictureBox.Width;
            }
            if (_playerPictureBox.Bottom > _levelHeight)
            {
                _playerPictureBox.Top = _levelHeight;
            }

            // 4. Handle Vertical Movement and Collision
            _playerPictureBox.Top += _velocity.Y;
            bool onGround = false;

            foreach (var tile in _levelTiles)
            {
                Rectangle playerBounds = _playerPictureBox.Bounds;
                Rectangle tileBounds = tile.Bounds;

                if (playerBounds.IntersectsWith(tileBounds))
                {
                    // This prevents the "floating" bug when sliding against a wall.
                    if (_velocity.Y > 0 && (_playerPictureBox.Bounds.Bottom - _velocity.Y) <= tileBounds.Top)
                    {
                        _playerPictureBox.Top = (tileBounds.Top)- playerBounds.Height;
                        _velocity.Y = 0;
                        _isJumping = false;
                        onGround = true;
                        break;
                    }
                    // Colliding from the bottom (hitting your head)
                    else if (_velocity.Y < 0)
                    {
                        _playerPictureBox.Top = tileBounds.Bottom;
                        _velocity.Y = 0;
                        break;
                    }
                }
            }

            // 5. Fallback to original ground level
            if (_playerPictureBox.Bottom > _levelHeight)
            {
                // Reset the character's location to its starting point
                _playerPictureBox.Location = _startPosition;

                // Reset velocity to stop them from instantly falling again
                _velocity = Point.Empty;
            }

            // 6. Update Animation
            UpdateAnimation(onGround);
        }
        private void UpdateAnimation(bool onGround)
        {
            PlayerState previousState = _animationState;

            // Determine facing direction
            if (_isMovingLeft) _facingDirection = 'L';
            if (_isMovingRight) _facingDirection = 'R';

            // Determine animation state based on actions
            if (!onGround) // Use the onGround flag to determine if airborne
            {
                _isJumping = true; // We are in the air
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

            // Reset frame if animation state has changed
            if (previousState != _animationState)
            {
                _currentFrame = 0;
            }

            // Advance frame
            int totalFrames = _animationFrames[_animationState];
            _currentFrame = (_currentFrame + 1) % totalFrames;

            _playerPictureBox.Invalidate();
        }

        // The InitializeAnimationData and PlayerPictureBox_Paint methods remain unchanged.
        // We've removed StartMove, StopMove, SetRunning, and Jump as public methods.

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