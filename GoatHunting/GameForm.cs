using System;
using System.Collections.Generic;
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
        private List<Goat> _goats = new List<Goat>(); // List to manage multiple goats
        private System.Windows.Forms.Timer _animationTimer;
        
        private GoatSpawner _goatSpawner;

        private bool goLeft, goRight, goUp, goDown; // Movement flags

        public GameForm()
        {
            InitializeLevel();
            InitializeComponent();
            _goatSpawner = new GoatSpawner(this);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(800, 600);
            this.Name = "GameForm";
            this.Text = "Goat Hunting";
            this.ResumeLayout(false);
        }

        private void InitializeLevel()
        {
            this.Text = "Level Newbie";
            this.BackColor = Color.LightGray;

            // Initialize player
            _player = new Player(new Point(PlayerInitialPositionX, PlayerInitialPositionY));
            this.Controls.Add(_player.GetPictureBox());

            // Subscribe to the bullet firing event
            _player.OnBulletFired += AddBulletToForm;

            // Initialize goats
            InitializeGoats();

            // Timer for animations and movement
            _animationTimer = new System.Windows.Forms.Timer { Interval = AnimationInterval };
            _animationTimer.Tick += (sender, e) => UpdateGame();
            _animationTimer.Start();

            // Movement and shooting key bindings
            this.KeyPreview = true;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
        }

        private void InitializeGoats()
        {
            // Add three goats at predefined positions
            var initialGoatPositions = new List<Point>
<<<<<<< HEAD
        {
            new Point(200, 150),
            new Point(400, 300),
            new Point(600, 450)
        };

            foreach (var position in initialGoatPositions)
            {
                var goat = new Goat(position, this);
                goat.OnGoatSpawned += AddGoatToForm;
=======
            {
                new Point(200, 150),
                new Point(400, 300),
                new Point(600, 450)
            };

            foreach (var position in initialGoatPositions)
            {
                var goat = new Goat(position);
>>>>>>> fbbeb7e9460b1d9f5709d3a22f820aae1b78445b
                _goats.Add(goat);
                this.Controls.Add(goat.GetPictureBox());
            }
        }

<<<<<<< HEAD
        private void AddGoatToForm(Goat goat)
        {
            _goats.Add(goat);
            this.Controls.Add(goat.GetPictureBox());
        }

=======
>>>>>>> fbbeb7e9460b1d9f5709d3a22f820aae1b78445b
        private void UpdateGame()
        {
            // Update player movement
            if (goLeft) _player.Walk(Keys.Left, this.ClientSize);
            if (goRight) _player.Walk(Keys.Right, this.ClientSize);
            if (goUp) _player.Walk(Keys.Up, this.ClientSize);
            if (goDown) _player.Walk(Keys.Down, this.ClientSize);

            // Render player animations
            _player.Animate();

<<<<<<< HEAD
            // Update goat movement and check collisions
            for (int i = _goats.Count - 1; i >= 0; i--)
            {
                var goat = _goats[i];
                goat.UpdatePosition(_player.GetPictureBox().Location);
                goat.CheckCollision(_player);
=======
            // Update goat movement
            foreach (var goat in _goats)
            {
                goat.UpdatePosition(_player.GetPictureBox().Location);
>>>>>>> fbbeb7e9460b1d9f5709d3a22f820aae1b78445b
            }
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
