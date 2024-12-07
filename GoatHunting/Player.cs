﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GoatHunting
{
    public class Player
    {
        private const int PlayerWidth = 70;
        private const int PlayerHeight = 85;
        private const int MovementSpeed = 10;
        private const int TotalFrames = 4;

        public event Action<Bullet> OnBulletFired;

        private PictureBox _playerPictureBox;
        private Image _spriteSheet;
        private int _currentFrame;
        private int _currentRow;
        private Keys _currentDirection;
        private bool _isWalking;

        public Player(Point initialPosition)
        {
            // Load sprite sheet from resources
            // Note: You'll need to replace Resource.shooter with your actual resource
            using (MemoryStream ms = new MemoryStream(Resource.shooter))
            {
                _spriteSheet = Image.FromStream(ms);
            }

            // Initialize the player's picture box
            _playerPictureBox = new PictureBox
            {
                Size = new Size(PlayerWidth, PlayerHeight),
                Location = initialPosition,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Default starting position/direction
            _currentRow = 0;
            _currentFrame = 0;
            _isWalking = false;

            UpdateSprite();
        }

        public PictureBox GetPictureBox() => _playerPictureBox;

        public void Walk(Keys direction, Size formClientSize)
        {
            _isWalking = true;
            _currentDirection = direction;

            // Update current row based on direction
            switch (direction)
            {
                case Keys.Down:
                    _currentRow = 0;
                    if (_playerPictureBox.Bottom < formClientSize.Height)
                        _playerPictureBox.Top += MovementSpeed;
                    break;
                case Keys.Left:
                    _currentRow = 1;
                    if (_playerPictureBox.Left > 0)
                        _playerPictureBox.Left -= MovementSpeed;
                    break;
                case Keys.Right:
                    _currentRow = 2;
                    if (_playerPictureBox.Right < formClientSize.Width)
                        _playerPictureBox.Left += MovementSpeed;
                    break;
                case Keys.Up:
                    _currentRow = 3;
                    if (_playerPictureBox.Top > 0)
                        _playerPictureBox.Top -= MovementSpeed;
                    break;
            }
        }

        public void StopWalking()
        {
            _isWalking = false;
            _currentFrame = 0;
            UpdateSprite();
        }

        public void Animate()
        {
            if (_isWalking)
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
                _currentRow * frameHeight,   // Y-coordinate (row index * frame height)
                frameWidth,                  // Width of the frame
                frameHeight                  // Height of the frame
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

        public void FireBulletBasedOnDirection()
        {
            int directionX = 0;
            int directionY = 0;

            switch (_currentRow)
            {
                case 0: // Down
                    directionX = 0;
                    directionY = 1;
                    break;
                case 1: // Left
                    directionX = -1;
                    directionY = 0;
                    break;
                case 2: // Right
                    directionX = 1;
                    directionY = 0;
                    break;
                case 3: // Up
                    directionX = 0;
                    directionY = -1;
                    break;
            }

            FireBullet(directionX, directionY);
        }

        private void FireBullet(int directionX, int directionY)
        {
            Point bulletStartPosition = new Point(
                _playerPictureBox.Left + (_playerPictureBox.Width / 2) - (20 / 2), // Center the bullet horizontally
                _playerPictureBox.Top + (_playerPictureBox.Height / 2) - (10 / 2) // Center the bullet vertically
            );

            Bullet bullet = new Bullet(bulletStartPosition, directionX, directionY);

            // Raise an event to notify the game form to add the bullet to the controls
            OnBulletFired?.Invoke(bullet);
        }
    }
}