using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoatHunting
{
    public class Goat
    {
        private const int GoatWidth = 60;
        private const int GoatHeight = 75;
        private const int MovementSpeed = 5; // Speed at which the goat moves
        private const int DamageAmount = 10; // Amount of damage the goat deals

        public event Action<int> OnPlayerDamaged; // Event to notify player damage

        private PictureBox _goatPictureBox;
        private Image _goatImage;
        private Point _targetPosition; // Player's position to be attracted to
        private bool _isActive = true; // Indicates if the goat is active

        public Goat(Point initialPosition)
        {
            _goatPictureBox = new PictureBox
            {
                Size = new Size(GoatWidth, GoatHeight),
                Location = initialPosition,
                BackColor = Color.Transparent,
                Image = Resource.goat,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            //try
            //{
            //    _goatPictureBox.Image = Resource.goat;
            //}
            //catch (Exception ex)
            //{
            //    _goatPictureBox.BackColor = Color.Black;
            //    Console.WriteLine("Gagal load image basket: " + ex.Message);
            //}
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
                // Notify player damage
                OnPlayerDamaged?.Invoke(DamageAmount);
                _isActive = false; // Goat stops moving after dealing damage
            }
        }

        public void Reactivate(Point newPosition)
        {
            _isActive = true;
            _goatPictureBox.Location = newPosition;
        }
    }
}
