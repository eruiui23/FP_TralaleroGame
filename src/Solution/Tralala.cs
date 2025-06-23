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

    public class Tralala : GameObject, ICollidable
    {
        // --- Sprite & Animation Configuration ---
        private const int PWidth = 58;
        private const int PHeight = 40;
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
        private const int StraightJumpSpeed = 25;
        private const int RunningJumpSpeed = 22;
        private const int Gravity = 2;
        private int _groundLevelY;

        public Rectangle Bounds => this.Box.Bounds;
        public Tralala(Point startPosition, List<Tile> tiles, int levelWidth, int levelHeight) : base(startPosition, new Size(PWidth, PHeight))
        {
            var allResourceNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (var name in allResourceNames)
            {
                // This will print every available resource name to the "Output" window in Visual Studio
                System.Diagnostics.Debug.WriteLine(name);
            }
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
            // Calculate the player's position on the screen
            int screenX = this.Position.X - cameraPosition.X;
            int screenY = this.Position.Y - cameraPosition.Y;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            Rectangle sourceRect = new Rectangle(
                _currentFrame * PWidth,
                (int)_animationState * PHeight,
                PWidth,
                PHeight
            );
            // Draw directly onto the form's graphics at the calculated screen position
            g.DrawImage(
                _spriteSheet,
                new Rectangle(screenX, screenY, PWidth, PHeight),
                sourceRect,
                GraphicsUnit.Pixel
            );
        }

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

            // --- MODIFIED JUMP LOGIC ---
            if (jump && !_isJumping)
            {
                _isJumping = true;

                // Check if the player is moving horizontally at the moment of the jump
                if (_isMovingLeft || _isMovingRight)
                {
                    // If they are moving, perform a running jump (lower)
                    _velocity.Y = -RunningJumpSpeed;
                }
                else
                {
                    // If they are standing still, perform a standing jump (higher)
                    _velocity.Y = -StraightJumpSpeed;
                }
            }
        }

        // In Tralala.cs

        public override void Update()
        {
            // Apply Horizontal Velocity from input
            _velocity.X = 0;
            if (_isMovingLeft)
            {
                _velocity.X = _isRunning ? -RunSpeed : -WalkSpeed;
            }
            if (_isMovingRight)
            {
                _velocity.X = _isRunning ? RunSpeed : WalkSpeed;
            }

            // Apply Gravity
            _velocity.Y += Gravity;

            // Store original position for collision testing
            Point originalPosition = this.Box.Location;

            // Apply vertical movement first
            this.Box.Top += _velocity.Y;
            bool onGround = false;
            bool hitCeiling = false;

            // Check vertical collisions
            foreach (var tile in _levelTiles)
            {
                Rectangle playerBounds = this.Box.Bounds;
                Rectangle tileBounds = tile.Bounds;

                if (playerBounds.IntersectsWith(tileBounds))
                {
                    // Check if we're hitting from below (ceiling collision)
                    if (_velocity.Y < 0 && originalPosition.Y >= tileBounds.Bottom)
                    {
                        this.Box.Top = tileBounds.Bottom;
                        _velocity.Y = 0;
                        hitCeiling = true;
                        break;
                    }
                    // Check if we're landing on top of a tile
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

            // Only apply horizontal movement if we didn't hit a ceiling
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
                        // Only handle side collisions if we're not hitting from above/below
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

            // Level boundaries
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
            // Reset the character's location to its starting point
            this.Box.Location = _startPosition;

            // Reset velocity to stop them from instantly falling again
            _velocity = Point.Empty;
            _isJumping = false;
            _animationState = PlayerState.IdleRight;
            _facingDirection = 'R';
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

            this.Box.Invalidate();
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