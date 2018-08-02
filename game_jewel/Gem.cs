using System;
using System.Drawing;
using System.Windows.Forms;

namespace game_jewel
{
    public partial class Gem : UserControl
    {
        public int x, y, col, row, type, size_gem, fadetime = 0;
        public Boolean match = false;
        public Game game;
        private void gem_Click(object sender, EventArgs e)
        {
            int px = x / size_gem, py = y / size_gem;
            if(!game.isMoving)
            {
                game.click += 1;
                if (game.click == 1)
                {
                    game.x1 = px;
                    game.y1 = py;
                }
                if (game.click == 2)
                {
                    game.x2 = px;
                    game.y2 = py;
                }
                BorderStyle = BorderStyle.FixedSingle;
            }
        }

        public Gem(int row, int col, int type, int size_gem)
        {
            InitializeComponent();
            x = col * size_gem;
            y = row * size_gem;
            this.col = col;
            this.row = row;
            this.size_gem = size_gem;
            Size = new Size(size_gem, size_gem);
            SetType(type);
            Location = new Point(x, y);
        }

        public void SetY(int y)
        {
            this.y = y;
            Location = new Point(x, y);
        }

        public void SetType(int type)
        {
            this.type = type;
            switch (type)
            {
                case 1: pictureBox1.Image = Properties.Resources.gem_1; break;
                case 2: pictureBox1.Image = Properties.Resources.gem_2; break;
                case 3: pictureBox1.Image = Properties.Resources.gem_3; break;
                case 4: pictureBox1.Image = Properties.Resources.gem_4; break;
                case 5: pictureBox1.Image = Properties.Resources.gem_5; break;
                default: pictureBox1.Image = Properties.Resources.gem_6; break;
            }
            Location = new Point(x, y);
        }
    }
}
