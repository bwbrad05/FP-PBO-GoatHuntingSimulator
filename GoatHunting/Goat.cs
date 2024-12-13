using System;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace GoatHunting
{
    public class Goat
    {
        private const int GoatWidth = 60;
        private const int GoatHeight = 75;
        private const int MovementSpeed = 5; // Speed at which the goat moves
        private const int DamageAmount = 10; // Amount of damage the goat deals
        private const int SpawnInterval = 3000; // 3 seconds in milliseconds

        public event Action<int> OnPlayerDamaged; // Event to notify player damage
        public event Action<Goat> OnGoatSpawned; // Event to notify when a new goat is spawned

        private PictureBox _goatPictureBox;
        private Point _targetPosition; // Player's position to be attracted to
        private bool _isActive = true; // Indicates if the goat is active
        private Timer _spawnTimer;
        private Form _parentForm;

        public Goat(Point initialPosition, Form parentForm = null)
        {
            _parentForm = parentForm;

            _goatPictureBox = new PictureBox
            {
                Size = new Size(GoatWidth, GoatHeight),
                Location = initialPosition,
                BackColor = Color.YellowGreen,
                Tag = "Goat",
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Setup spawn timer if parent form is provided
            if (parentForm != null)
            {
                _spawnTimer = new Timer
                {
                    Interval = SpawnInterval
                };
                _spawnTimer.Tick += SpawnNewGoat;
                _spawnTimer.Start();
            }
        }

        private void SpawnNewGoat(object sender, EventArgs e)
        {
            if (!_isActive || _parentForm == null) return;

            // Generate a random position for the new goat
            Random rand = new Random();
            Point newPosition = new Point(
                rand.Next(0, _parentForm.ClientSize.Width - GoatWidth),
                rand.Next(0, _parentForm.ClientSize.Height - GoatHeight)
            );

            Goat newGoat = new Goat(newPosition, _parentForm);
            OnGoatSpawned?.Invoke(newGoat);
        }

        public PictureBox GetPictureBox() => _goatPictureBox;

        public void UpdatePosition(Point playerPosition)
        {
            if (!_isActive)
                return;

            _targetPosition = playerPosition;

            // Calculate direction towards player
            int directionX = _targetPosition.X - _goatPictureBox.Left;
            int directionY = _targetPosition.Y - _goatPictureBox.Top;

            // Normalize direction vector to get unit direction
            double magnitude = Math.Sqrt(directionX * directionX + directionY * directionY);
            if (magnitude > 0)
            {
                directionX = (int)(directionX / magnitude * MovementSpeed);
                directionY = (int)(directionY / magnitude * MovementSpeed);

                // Move goat towards the player
                _goatPictureBox.Left += directionX;
                _goatPictureBox.Top += directionY;
            }
        }

        public void CheckCollision(Player player)
        {
            if (!_isActive)
                return;

            if (_goatPictureBox.Bounds.IntersectsWith(player.GetPictureBox().Bounds))
            {
                // Stop the spawn timer when the goat deals damage
                _spawnTimer?.Stop();

                // Notify player damage
                OnPlayerDamaged?.Invoke(DamageAmount);
                _isActive = false; // Goat stops moving after dealing damage
            }
        }

        public void Reactivate(Point newPosition)
        {
            _isActive = true;
            _goatPictureBox.Location = newPosition;
            _spawnTimer?.Start();
        }

        // Cleanup method to stop the timer
        public void Dispose()
        {
            _spawnTimer?.Stop();
            _spawnTimer?.Dispose();
        }
    }
}