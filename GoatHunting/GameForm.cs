using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoatHunting
{
    public class GameForm : Form
    {
        private const int PlayerInitialPositionX = 50;
        private const int PlayerInitialPositionY = 50;
        private const int AnimationInterval = 16;

        private Player _player;
        private System.Windows.Forms.Timer _animationTimer;

        private bool goLeft, goRight, goUp, goDown; // Movement flags

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

            // Timer for animations and movement
            _animationTimer = new System.Windows.Forms.Timer { Interval = AnimationInterval };
            _animationTimer.Tick += (sender, e) => UpdateGame();
            _animationTimer.Start();

            // Movement and shooting key bindings
            this.KeyPreview = true;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
        }

        private void UpdateGame()
        {
            // Update player movement
            if (goLeft) _player.Walk(Keys.Left, this.ClientSize);
            if (goRight) _player.Walk(Keys.Right, this.ClientSize);
            if (goUp) _player.Walk(Keys.Up, this.ClientSize);
            if (goDown) _player.Walk(Keys.Down, this.ClientSize);

            // Render animations
            _player.Animate();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    goLeft = true;
                    break;
                case Keys.Right:
                    goRight = true;
                    break;
                case Keys.Up:
                    goUp = true;
                    break;
                case Keys.Down:
                    goDown = true;
                    break;
                case Keys.Space:
                    _player.FireBulletBasedOnDirection();
                    break;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    goLeft = false;
                    break;
                case Keys.Right:
                    goRight = false;
                    break;
                case Keys.Up:
                    goUp = false;
                    break;
                case Keys.Down:
                    goDown = false;
                    break;
            }
        }

        private void AddBulletToForm(Bullet bullet)
        {
            this.Controls.Add(bullet.GetPictureBox());
        }
    }
}