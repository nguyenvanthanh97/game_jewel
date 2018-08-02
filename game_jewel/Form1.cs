using System;
using System.Windows.Forms;

namespace game_jewel
{
    public partial class Form1 : Form
    {
        Game game;
        public Form1()
        {
            InitializeComponent();
            game = new Game(panelGame);
            game.labelScore = labelScore;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            game.Update();
        }
    }
}
