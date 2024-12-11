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
        private Timer _movementTimer;
        private PictureBox _bulletPictureBox;
        private int _directionX;
        private int _directionY;
        private Form _parentForm;

        public event EventHandler BulletDestroyed;

        public Bullet(Point startPosition, int directionX, int directionY, Form parentForm)
        {
            _parentForm = parentForm;
            _bulletPictureBox = new PictureBox
            {
                Size = new Size(BulletWidth, BulletHeight),
                Location = startPosition,
                BackColor = Color.Black // You can change this to an image if you have a sprite for the bullet
            };

            _directionX = directionX;
            _directionY = directionY;

            _movementTimer = new Timer { Interval = 30 }; // Adjust interval for smoothness
            _movementTimer.Tick += Move;
            _movementTimer.Start();

            // Add the bullet to the form
            _parentForm.Controls.Add(_bulletPictureBox);
        }

        public PictureBox GetPictureBox() => _bulletPictureBox;

        private void Move(object sender, EventArgs e)
        {
            _bulletPictureBox.Left += _directionX * Speed;
            _bulletPictureBox.Top += _directionY * Speed;

            // Check for collision with goats
            CheckGoatCollision();

            // Remove the bullet when it goes off-screen
            if (_bulletPictureBox.Right < 0 || _bulletPictureBox.Left > _parentForm.ClientSize.Width ||
                _bulletPictureBox.Bottom < 0 || _bulletPictureBox.Top > _parentForm.ClientSize.Height)
            {
                DestroyBullet();
            }
        }

        private void CheckGoatCollision()
        {
            // Find all goat PictureBoxes in the form
            foreach (Control control in _parentForm.Controls)
            {
                if (control is PictureBox goat && goat.Tag?.ToString() == "Goat")
                {
                    if (_bulletPictureBox.Bounds.IntersectsWith(goat.Bounds))
                    {
                        // Destroy the goat
                        _parentForm.Controls.Remove(goat);
                        goat.Dispose();

                        // Destroy the bullet
                        DestroyBullet();
                        break;
                    }
                }
            }
        }

        private void DestroyBullet()
        {
            _movementTimer.Stop();
            _movementTimer.Dispose();

            // Remove the bullet from the form
            _parentForm.Controls.Remove(_bulletPictureBox);
            _bulletPictureBox.Dispose();

            // Trigger the destroyed event
            BulletDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }

    public class GoatSpawner
    {
        private Timer _spawnTimer;
        private Form _parentForm;
        private Random _random;

        public GoatSpawner(Form parentForm)
        {
            _parentForm = parentForm;
            _random = new Random();

            _spawnTimer = new Timer { Interval = 3000 }; // Spawn goats every 3 seconds
            _spawnTimer.Tick += SpawnGoat;
            _spawnTimer.Start();
        }

        private void SpawnGoat(object sender, EventArgs e)
        {
            PictureBox goat = new PictureBox
            {
                Size = new Size(40, 40), // Adjust goat size as needed
                Location = new Point(_random.Next(0, _parentForm.ClientSize.Width - 40), _random.Next(0, _parentForm.ClientSize.Height - 40)),
                BackColor = Color.Brown, // Use an image for better visuals
                Tag = "Goat"
            };

            _parentForm.Controls.Add(goat);
        }
    }
}
