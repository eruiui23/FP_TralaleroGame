using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SpriteAnimation
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
        private PlayerState _animationState; // Renamed for clarity

        // --- Physics & Movement Variables ---
        private Point _velocity; // Stores X and Y speed
        private bool _isJumping = false;
        private bool _isMovingRight = false;
        private bool _isMovingLeft = false;
        private bool _isRunning = false;

        // --- Physics Constants (Tweak these to change how the character feels!) ---
        private const int WalkSpeed = 6;
        private const int RunSpeed = 10;
        private const int JumpSpeed = 16; // Initial upward force of a jump
        private const int Gravity = 2;    // How strongly the player is pulled down
        private int _groundLevelY;        // The Y coordinate of the "floor"

        public Tralala(Point startPosition)
        {
            _spriteSheet = Solution.Resource.Assets;
            InitializeAnimationData();

            _animationState = PlayerState.IdleRight;
            _currentFrame = 0;
            _velocity = Point.Empty; // Start with zero velocity
            _groundLevelY = startPosition.Y; // The starting Y is our ground level

            _playerPictureBox = new PictureBox
            {
                Size = new Size(PWidth, PHeight),
                Location = startPosition,
                BackColor = Color.Transparent,
            };
            _playerPictureBox.Paint += PlayerPictureBox_Paint;
        }

        // The public method to get the PictureBox for the form
        public PictureBox GetPictureBox() => _playerPictureBox;

        // This is the NEW "heartbeat" of your player.
        // It should be called on every single Timer tick.
        public void Update()
        {
            // --- 1. Apply Horizontal Velocity ---
            // If not moving left or right, horizontal velocity is zero
            _velocity.X = 0;
            if (_isMovingLeft)
            {
                _velocity.X = _isRunning ? -RunSpeed : -WalkSpeed;
            }
            if (_isMovingRight)
            {
                _velocity.X = _isRunning ? RunSpeed : WalkSpeed;
            }

            // --- 2. Apply Gravity ---
            // Gravity constantly pulls the player down, increasing the Y velocity
            _velocity.Y += Gravity;

            // --- 3. Update Position based on Velocity ---
            _playerPictureBox.Left += _velocity.X;
            _playerPictureBox.Top += _velocity.Y;

            // --- 4. Check for Ground Collision ---
            if (_playerPictureBox.Top >= _groundLevelY)
            {
                _playerPictureBox.Top = _groundLevelY; // Snap to ground to prevent falling through
                _isJumping = false; // We have landed
                _velocity.Y = 0;    // Stop falling
            }

            // --- 5. Update the Animation ---
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            // Decide which animation to show based on the physics state
            if (_isJumping)
            {
                // --- THIS IS THE UPDATED LOGIC ---
                // If Y velocity is negative, we are moving UP (Jumping)
                if (_velocity.Y < 0)
                {
                    _animationState = (_velocity.X < 0) ? PlayerState.JumpLeft : PlayerState.JumpRight;
                }
                else // Otherwise, we are moving DOWN (Falling)
                {
                    _animationState = (_velocity.X < 0) ? PlayerState.FallLeft : PlayerState.FallRight;
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
            else // Not moving and not jumping
            {
                // This logic correctly sets the idle state based on last direction
                if (_animationState == PlayerState.IdleLeft || _animationState == PlayerState.RunLeft ||
                    _animationState == PlayerState.WalkLeft || _animationState == PlayerState.FallLeft || _animationState == PlayerState.JumpLeft)
                {
                    _animationState = PlayerState.IdleLeft;
                }
                else
                {
                    _animationState = PlayerState.IdleRight;
                }
            }

            // Advance the frame of the current animation
            int totalFrames = _animationFrames[_animationState];
            _currentFrame = (_currentFrame + 1) % totalFrames;
            _playerPictureBox.Invalidate(); // Tell the PictureBox to redraw
        }

        // --- Public methods to be called by the Form ---

        public void StartMove(char direction) // 'L' for Left, 'R' for Right
        {
            if (direction == 'L') _isMovingLeft = true;
            if (direction == 'R') _isMovingRight = true;
        }

        public void StopMove(char direction)
        {
            if (direction == 'L') _isMovingLeft = false;
            if (direction == 'R') _isMovingRight = false;
        }

        public void SetRunning(bool running)
        {
            _isRunning = running;
        }

        public void Jump()
        {
            // You can only jump if you are not already in the air
            if (!_isJumping)
            {
                _isJumping = true;
                _velocity.Y = -JumpSpeed; // Give a strong initial upward velocity
            }
        }

        // The rest of the class (Paint event, etc.) is the same as before
        private void InitializeAnimationData()
        {
            _animationFrames = new Dictionary<PlayerState, int>
            {
                { PlayerState.FallRight, 2 },
                { PlayerState.IdleRight, 4 },
                { PlayerState.RunRight, 5 },
                { PlayerState.WalkRight, 5 },
                { PlayerState.JumpRight, 3 },
                { PlayerState.IdleLeft, 4 },
                { PlayerState.RunLeft, 5 },
                { PlayerState.WalkLeft, 5 },
                { PlayerState.FallLeft, 2 },
                { PlayerState.JumpLeft, 3 }
            };
        }
        private void PlayerPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // This prevents the image from getting blurry when scaled
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            // Calculate which part of the big sprite sheet to copy from
            Rectangle sourceRect = new Rectangle(
                _currentFrame * PWidth,
                (int)_animationState * PHeight, // <-- THIS IS THE CORRECTED LINE
                PWidth,
                PHeight
            );

            // Define the destination area on our PictureBox (the whole thing)
            Rectangle destinationRect = new Rectangle(0, 0, PWidth, PHeight);

            // Draw the single frame onto the PictureBox
            e.Graphics.DrawImage(
                _spriteSheet,
                destinationRect,
                sourceRect,
                GraphicsUnit.Pixel
            );
        }

    }
}