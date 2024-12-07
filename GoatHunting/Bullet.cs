using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoatHunting
{
    public class Bullet
    {
        private const int BulletWidth = 20;
        private const int BulletHeight = 10;
        private const int Speed = 24;

        private System.Windows.Forms.Timer _movementTimer;
        private PictureBox _bulletPictureBox;
        private int _directionX;
        private int _directionY;

        public Bullet(Point startPosition, int directionX, int directionY)
        {
            _bulletPictureBox = new PictureBox
            {
                Size = new Size(BulletWidth, BulletHeight),
                Location = startPosition,
                BackColor = Color.Black // You can change this to an image if you have a sprite for the bullet
            };

            _directionX = directionX;
            _directionY = directionY;

            _movementTimer = new System.Windows.Forms.Timer { Interval = 30 }; // Adjust interval for smoothness
            _movementTimer.Tick += Move;
            _movementTimer.Start();
        }

        public PictureBox GetPictureBox() => _bulletPictureBox;

        private void Move(object sender, EventArgs e)
        {
            _bulletPictureBox.Left += _directionX * Speed;
            _bulletPictureBox.Top += _directionY * Speed;

            // Remove the bullet when it goes off-screen
            if (_bulletPictureBox.Right < 0 || _bulletPictureBox.Left > Screen.PrimaryScreen.Bounds.Width ||
                _bulletPictureBox.Bottom < 0 || _bulletPictureBox.Top > Screen.PrimaryScreen.Bounds.Height)
            {
                _movementTimer.Stop();
                _movementTimer.Dispose();
                _bulletPictureBox.Dispose();
            }
        }
    }
}