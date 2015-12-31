using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MyGame.Model;
using MyGame.Components;

namespace MyGame
{
    public partial class MainForm : Form
    
    {
        private Input input = new Input();
        private Game game;
        private Stopwatch watch = new Stopwatch();
        private RenderControl renderControl;

        public MainForm()
        {
            InitializeComponent();

            game = new Game(input);

            renderControl = new RenderControl(game);
            renderControl.Dock = DockStyle.Fill;
            this.Controls.Add(renderControl);

            watch.Start();            
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            input.KeyDown(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            game.Update(watch.Elapsed);
            watch.Restart();
            renderControl.Invalidate();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            input.KeyUp(e.KeyCode);
            base.OnKeyUp(e);
        }
    }
}
