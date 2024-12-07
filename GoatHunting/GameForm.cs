using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoatHunting
{
    public class GameForm : Form
    {
        private const int PlayerInitialPositionX = 50;
        private const int PlayerInitialPositionY = 50;
        private const int AnimationInterval = 100;

        private Player _player;
        private System.Windows.Forms.Timer _animationTimer;

        public GameForm()
        {
            InitializeComponent();
            InitializeLevel();
        }

        private void InitializeComponent()
        {
            // Add any additional form initialization if needed
            this.SuspendLayout();

            this.ClientSize = new Size(800, 600);
            this.Name = "GameForm";
            this.Text = "Goat Hunting";

            this.ResumeLayout(false);
        }

        private void InitializeLevel()
        {
            this.Text = "Level 1";
            this.BackColor = Color.LightGray;

            // Initialize player
            _player = new Player(new Point(PlayerInitialPositionX, PlayerInitialPositionY));
            this.Controls.Add(_player.GetPictureBox());

            // Subscribe to the bullet firing event
            _player.OnBulletFired += AddBulletToForm;

            // Timer for animations
            _animationTimer = new System.Windows.Forms.Timer { Interval = AnimationInterval };
            _animationTimer.Tick += (sender, e) => Render();
            _animationTimer.Start();

            // Movement and shooting key bindings
            this.KeyPreview = true;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
        }

        private void Render()
        {
            _player.Animate();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            //switch (e.KeyCode)
            //{
            //    case Keys.Space:
            //        _player.FireBulletBasedOnDirection(); // Use the method from the corrected code
            //        break;
            //    case Keys.Left:
            //    case Keys.Right:
            //    case Keys.Up:
            //    case Keys.Down:
            //        _player.Walk(e.KeyCode, this.ClientSize);
            //        break;
            //}
            if (e.KeyCode == Keys.Space)
            {
                _player.FireBulletBasedOnDirection(); // Shoot without stopping movement

            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                _player.Walk(e.KeyCode, this.ClientSize); // Move the player
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            _player.StopWalking();
        }

        private void AddBulletToForm(Bullet bullet)
        {
            this.Controls.Add(bullet.GetPictureBox());
        }
    }
}