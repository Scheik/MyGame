using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MyGame.Model;

namespace MyGame
{
    internal partial class RenderControl : UserControl
    {
        private int SPRITE_WIDTH = 57;
        private int SPRITE_HEIGHT = 57;

        private Stopwatch watch = new Stopwatch();

        private readonly Game game;

        private readonly Image grass;
        private readonly Image dirt;
        private readonly Image sprite;

        public RenderControl(Game game)
        {
            InitializeComponent();
            this.game = game;

            dirt = Image.FromFile("Assets/dirt.png");
            grass = Image.FromFile("Assets/grass.png");
            sprite = Image.FromFile("Assets/sprite.png");

            watch.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            float direction = (game.Player.Angle * 360) / (float)(2 * Math.PI);
            direction += 230;
            int sector = (int)(direction / 90);

            int offsety = 0;
            switch (sector)
            {
                case 1: offsety = 3 * SPRITE_HEIGHT; break;
                case 2: offsety = 2 * SPRITE_HEIGHT; break;
                case 3: offsety = 0 * SPRITE_HEIGHT; break;
                case 4: offsety = 1 * SPRITE_HEIGHT; break;
            }

            e.Graphics.Clear(Color.Black);

            int offsetX = (int)game.Camera.Center.X - (this.ClientSize.Width / 2);
            int offsetY = (int)game.Camera.Center.Y - (this.ClientSize.Height / 2);

            int cellX1 = Math.Max(0, (int)(offsetX / dirt.Width));
            int cellY1 = Math.Max(0, (int)(offsetY / dirt.Height));

            int cellCountX = (ClientSize.Width / dirt.Width) + 2;
            int cellCountY = (ClientSize.Height / dirt.Height) + 2;

            int cellX2 = Math.Min(cellX1 + cellCountX, (int)game.PlaygroundSize.X / dirt.Width);
            int cellY2 = Math.Min(cellY1 + cellCountY, (int)game.PlaygroundSize.Y / dirt.Height);

            for (int x = cellX1; x < cellX2 + cellCountX ; x ++)
            {
                for (int y = cellY1; y < cellY2; y ++)
                {
                    e.Graphics.DrawImage(dirt, new Point(
                        x* dirt.Width -offsetX, 
                        y* dirt.Height - offsetY));
                }
            }           

            if (game == null)
                return;
            using (Brush brush = new SolidBrush(Color.Yellow))
            {
                int frame = (int)(watch.ElapsedMilliseconds / 250) % 4;

                int offsetx = 0;
                if (game.Player.State == Player.PlayerState.Walk)
                {
                    switch (frame)
                    {
                        case 0: offsetx = 0; break;
                        case 1: offsetx = SPRITE_WIDTH; break;
                        case 2: offsetx = 2 * SPRITE_WIDTH; break;
                        case 3: offsetx = SPRITE_WIDTH; break;
                    }
                }
                else
                {
                    offsetx = SPRITE_WIDTH;
                }
                
                e.Graphics.DrawImage(sprite, 
                    new RectangleF(game.Player.Position.X - offsetX, game.Player.Position.Y - offsetY, SPRITE_WIDTH, SPRITE_HEIGHT), 
                    new RectangleF(offsetx, offsety, SPRITE_WIDTH, SPRITE_HEIGHT), 
                    GraphicsUnit.Pixel);
            }
        }
    }
}
