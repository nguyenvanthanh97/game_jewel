using System;
using System.Drawing;
using System.Windows.Forms;
namespace game_jewel
{
    public class Game
    {
        public Gem[,] gem;
        public int x1 = -1, y1 = -1, x2 = 0, y2 = 0, click = 0, size_gem = 100, combo = 0;
        public int sumCol = 8, sumRow = 8;
        public Boolean isSwap = false, isMoving = false, refresh = true, isPlay = true;
        public Panel panelGame;
        public Label labelScore;
        public Random rand;
        public Sound sound = new Sound();
        public int score = 0, totalScore = 0;

        public Game(Panel panelGame)
        {
            gem = new Gem[sumRow, sumCol];
            rand = new Random();
            for (int i = 0; i < sumRow; i++)
            {
                for (int j = 0; j < sumCol; j++)
                {
                    gem[i, j] = new Gem(i, j, rand.Next(1, 7), size_gem);
                    gem[i, j].game = this;
                    panelGame.Controls.Add(gem[i, j]);
                }
            }
        }

        public void Swap(Gem a, Gem b)
        {
            int temp;
            temp = a.col;
            a.col = b.col;
            b.col = temp;

            temp = a.row;
            a.row = b.row;
            b.row = temp;

            gem[a.row, a.col] = a;
            gem[b.row, b.col] = b;
        }

        public void Update()
        {
            if (click == 2)
            {
                int distance = Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
                if (distance == 1)
                {
                    Swap(gem[y1,x1], gem[y2,x2]);
                    isSwap = true;
                    
                    gem[y1, x1].BorderStyle = BorderStyle.None;
                    gem[y2, x2].BorderStyle = BorderStyle.None;
                    click = 0;
                    combo = 0;
                    refresh = true;
                }
                else
                {
                    click = 1;
                    if (distance > 0)
                    {
                        gem[y1, x1].BorderStyle = BorderStyle.None;
                        x1 = x2;
                        y1 = y2;
                    }
                }
            }

            //Find match
            if (refresh)
            {
                for (int i = 0; i < sumRow; i++)
                    for (int j = 0; j < sumCol; j++)
                    {
                        if (i > 0 && i < sumRow - 1 && gem[i, j].type == gem[i + 1, j].type && gem[i, j].type == gem[i - 1, j].type)
                        {
                            gem[i - 1, j].match = gem[i, j].match = gem[i + 1, j].match = true;
                        }

                        if (j > 0 && j < sumCol - 1 && gem[i, j].type == gem[i, j + 1].type && gem[i, j].type == gem[i, j - 1].type)
                        {
                            gem[i,j-1].match = gem[i, j].match = gem[i,j+1].match = true;
                        }
                    }
                //Get score
                for (int i = 0; i < sumRow; i++)
                    for (int j = 0; j < sumCol; j++)
                        if(gem[i, j].match)
                            score += 1;
                if (score == 0)
                {
                    if (combo > 2) sound.so_attractive.Play();
                    if (combo > 4) sound.its_magic.Play();
                }
            }
            refresh = false;

            isMoving = false;
            for(int i = 0; i < sumRow; i++)
                for(int j = 0; j < sumCol; j++)
                {
                    Gem p = gem[i,j];
                    int dx = 0, dy = 0;
                    for (int n = 0; n < 10; n++) // This is speed
                    {
                        dx = p.x - p.col * size_gem;
                        dy = p.y - p.row * size_gem;
                        if (dx != 0) p.x -= dx > 0 ? 1 : -1;
                        if (dy != 0) p.y -= dy > 0 ? 1 : -1;
                        p.Location = new Point(p.x, p.y);
                    }
                    if (dx != 0 || dy != 0) isMoving = true;
                }


            if (!isMoving && score > 0 && isPlay)
            {
                sound.match.Play();
                isPlay = false;
            }
            //Deleting amimation
            if (!isMoving)
                for (int i = 0; i < sumRow; i++)
                    for (int j = 0; j < sumCol; j++)
                        if (gem[i,j].match)
                            if (gem[i,j].fadetime < 50)
                            {
                                if (gem[i, j].fadetime == 0)
                                    gem[i, j].pictureBox1.Image = Properties.Resources.fade;
                                gem[i, j].fadetime += 1;
                                isMoving = true;
                            }

            //Second swap if no match
            if (isSwap && !isMoving)
            {
                if(score == 0) Swap(gem[y1,x1], gem[y2,x2]);
                isSwap = false;
                //x1 = y1 = -1;
            }

            //Update grid
            if (!isMoving && score > 0)
            {
                combo++;
                totalScore += score;
                labelScore.Text = "Score: " + totalScore;
                score = 0;
                for (int i = sumRow-1; i >= 0; i--)
                    for (int j = 0; j < sumCol; j++)
                        if (gem[i,j].match)
                            for (int n = i; n >= 0; n--)
                                if (!gem[n,j].match)
                                {
                                    Swap(gem[n,j], gem[i,j]);
                                    break;
                                };
                
                for (int j = 0; j < sumCol; j++)
                    for (int i = sumRow - 1, n = 0; i >= 0; i--)
                        if (gem[i, j].match)
                        {
                            refresh = true;
                            isPlay = true;
                            gem[i, j].SetType(rand.Next(1, 7));
                            gem[i, j].SetY(-size_gem * ++n);
                            gem[i, j].match = false;
                            gem[i, j].fadetime = 0;
                        }
            }
        }
    }
}
