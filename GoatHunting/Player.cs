using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GoatHunting
{
    public class Player
    {
        private const int PlayerWidth = 70;
        private const int PlayerHeight = 85;
        private const int TotalFrames = 4;

        private PictureBox _playerPictureBox;
        private Image _spriteSheet;
        private int _currentFrame;
        private int _currentRow;
        private bool _isMoving;

        public Player(Point startPosition)
        {
            using (MemoryStream ms = new MemoryStream(Resource.shooter))
            {
                _spriteSheet = Image.FromStream(ms);
            }

            _currentFrame = 0;
            _currentRow = 0;

            _playerPictureBox = new PictureBox
            {
                Size = new Size(PlayerWidth, PlayerHeight),
                Location = startPosition,
                BackColor = Color.Transparent
            };

            UpdateSprite();
        }

        public PictureBox GetPictureBox() => _playerPictureBox;

        public void Walk(Keys key, Size boundary)
        {
            int speed = 10;
            _isMoving = true;

            switch (key)
            {
                case Keys.Down:
                    _currentRow = 0;
                    if (_playerPictureBox.Bottom < boundary.Height)
                        _playerPictureBox.Top += speed;
                    break;
                case Keys.Up:
                    _currentRow = 3;
                    if (_playerPictureBox.Top > 0)
                        _playerPictureBox.Top -= speed;
                    break;
                case Keys.Left:
                    _currentRow = 1;
                    if (_playerPictureBox.Left > 0)
                        _playerPictureBox.Left -= speed;
                    break;
                case Keys.Right:
                    _currentRow = 2;
                    if (_playerPictureBox.Right < boundary.Width)
                        _playerPictureBox.Left += speed;
                    break;
                default:
                    _isMoving = false;
                    break;
            }
        }

        public void StopWalking()
        {
            _isMoving = false;
            _currentFrame = 0;
            UpdateSprite();
        }

        public void Animate()
        {
            if (_isMoving)
            {
                _currentFrame = (_currentFrame + 1) % TotalFrames;
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            int frameWidth = _spriteSheet.Width / TotalFrames; // Width of a single frame
            int frameHeight = _spriteSheet.Height / 4;        // Height of a single row (4 directions)

            Rectangle srcRect = new Rectangle(
                _currentFrame * frameWidth,  // X-coordinate (frame index * frame width)
                _currentRow * frameHeight,  // Y-coordinate (row index * frame height)
                frameWidth,                 // Width of the frame
                frameHeight                 // Height of the frame
            );

            // Create a bitmap with the same size as the PictureBox
            Bitmap currentFrameImage = new Bitmap(PlayerWidth, PlayerHeight);

            using (Graphics g = Graphics.FromImage(currentFrameImage))
            {
                // Scale the sprite to fit the PictureBox dimensions
                g.DrawImage(
                    _spriteSheet,
                    new Rectangle(0, 0, PlayerWidth, PlayerHeight),
                    srcRect,                                
                    GraphicsUnit.Pixel
                );
            }

            _playerPictureBox.Image = currentFrameImage;
        }
    }
}