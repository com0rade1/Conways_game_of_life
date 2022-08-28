using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        int Scale;
        Random rnd = new Random();
        int[,] InputData = new int[301,301];
        int[,] TempData = new int[301,301];
        int[,] BordPixNum = new int[301,301];
        int Countt;
        int Count;
        int PrintStr;
        bool ScalePress = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Run_Click(object sender, EventArgs e)
        {
            this.Scale = 2 * (ScaleBar.Value + 1);
            Graphics g = GameField.CreateGraphics();
            g.Clear(Color.White);
            this.Countt = (int)(GameField.Width / this.Scale);
            textBox1.Text = Convert.ToString(this.Countt);
            int k = 0,j=0;
            for (int i = 0; i < GameField.Width; i += Scale)
            {
                BordPixNum[k,j] = i;
                BordPixNum[j, k] = i;
                k++;
                j++;
            }
            for (int i =0;i<GameField.Width;i+=Scale)
            {
                g.DrawLine(new Pen(Color.Black), i, 0, i, GameField.Height);
                g.DrawLine(new Pen(Color.Black), 0, i, GameField.Width, i);
            }
            ScalePress = true;
            for (int i = 0; i<Countt;i++)
            {
                for (int ksi =0;ksi<Countt;ksi++)
                {
                   this.InputData[i, ksi] = 0;
                }
            }
        }

        private void GameField_MouseDown(object sender, MouseEventArgs e)
        {
            if (ScalePress)
            {
                Graphics g = GameField.CreateGraphics();
                int k = 0;
                int i = 0;
                while (i*this.Scale < e.X)
                {
                    while (k*this.Scale < e.Y)
                    {
                        k ++;
                    }
                    i ++;
                }
                k --; i --;
                if (this.InputData[i, k] == 0)
                {
                    g.FillRectangle(Brushes.Black, i*this.Scale, k*this.Scale, this.Scale, this.Scale);
                    this.InputData[i, k] = 1;
                }
                else
                {
                    g.FillRectangle(Brushes.White, i * this.Scale, k * this.Scale, this.Scale, this.Scale);
                    g.DrawRectangle(new Pen(Color.Black), i*this.Scale, k*this.Scale, this.Scale, this.Scale);
                    this.InputData[i, k] = 0;
                }
            }
            else MessageBox.Show("Задайте масштаб перед тем, как рисовать","Ошибка!",MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void GameField_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Graphics g = GameField.CreateGraphics();
            Bitmap bmp = new Bitmap(GameField.Width, GameField.Height);
            Graphics g1 = Graphics.FromImage(bmp);
            for (int i = 0; i < GameField.Width; i += Scale)
            {
                g1.DrawLine(new Pen(Color.Black), i, 0, i, GameField.Height);
                g1.DrawLine(new Pen(Color.Black), 0, i, GameField.Width, i);
            }
            if (this.ScalePress)
            {
                for (int i = 0; i < Countt; i++)
                {
                    for (int j = 0; j < Countt; j++)
                    {
                        this.InputData[i, j] = rnd.Next(0,2);
                    }
                }
                for (int i = 0; i<Countt; i++)
                {
                    for (int j = 0; j<Countt;j++)
                    {
                        if (InputData[i, j] == 0)
                        {
                            g1.DrawRectangle(new Pen(Color.Black), i * Scale, j * Scale, Scale, Scale);
                        }
                        else g1.FillRectangle(Brushes.Black, i * Scale, j * Scale, Scale, Scale);
                    }
                }
            }
            GameField.Image = bmp;
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            Graphics g = GameField.CreateGraphics();
            Bitmap bmp = new Bitmap(GameField.Width,GameField.Height);
            Graphics g1 = Graphics.FromImage(bmp);
            int LiveNeigbour = 0;
            Count = Countt - 1;
            for (int i = 0; i < GameField.Width; i += Scale)
            {
                g1.DrawLine(new Pen(Color.Black), i, 0, i, GameField.Height);
                g1.DrawLine(new Pen(Color.Black), 0, i, GameField.Width, i);
            }
            
            for (int i = 0; i <= Count; i++)
            {
                for (int j = 0; j <= Count; j++)
                {
                    LiveNeigbour = 0;
                    if ((i == 0) && (j == 0))
                    {
                         LiveNeigbour = InputData[i + 1, j + 1] + InputData[i, j + 1] + InputData[i + 1, j] + InputData[i, Count] + InputData[Count, j] + InputData[Count, Count] + InputData[i+1, Count] + InputData[Count,j+1];
                        if ((LiveNeigbour == 3) && (InputData[i, j] == 0))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else if (((LiveNeigbour == 2) || (LiveNeigbour == 3)) && (InputData[i, j] == 1))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else this.TempData[i, j] = 0;
                        goto Next;
                    }
                    if ((i==0) && (j==Count))
                    {
                        LiveNeigbour = InputData[Count,Count-1] + InputData[0, Count-1] + InputData[1,Count-1] + InputData[1, Count] + InputData[1, 0] + InputData[0, 0] + InputData[Count, 0] + InputData[Count, Count];
                        if ((LiveNeigbour == 3) && (InputData[i, j] == 0))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else if (((LiveNeigbour == 2) || (LiveNeigbour == 3)) && (InputData[i, j] == 1))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else this.TempData[i, j] = 0;
                        goto Next;
                    }
                    if ((i==Count)&&(j==0))
                    {
                        LiveNeigbour = InputData[i-1, 0] + InputData[i-1, 1] + InputData[i, 1] + InputData[0, 1] + InputData[0, 0] + InputData[0, Count] + InputData[Count, Count] + InputData[Count - 1, Count];
                        if ((LiveNeigbour == 3) && (InputData[i, j] == 0))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else if (((LiveNeigbour == 2) || (LiveNeigbour == 3)) && (InputData[i, j] == 1))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else this.TempData[i, j] = 0;
                        goto Next;
                    }
                    if ((i==Count)&&(j==Count))
                    {
                        LiveNeigbour = InputData[i-1, j] + InputData[i-1, j-1] + InputData[i, j-1] + InputData[Count-1, 0] + InputData[Count, 0] + InputData[0, Count - 1] + InputData[0, Count] + InputData[0, 0];
                        if ((LiveNeigbour == 3) && (InputData[i, j] == 0))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else if (((LiveNeigbour == 2) || (LiveNeigbour == 3)) && (InputData[i, j] == 1))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else this.TempData[i, j] = 0;
                        goto Next;

                    }
                    if ((i == 0) && (j != 0) && (j != Count))
                    {
                        LiveNeigbour = InputData[i, j - 1] + InputData[i, j + 1] + InputData[i + 1, j + 1] + InputData[i + 1, j] + InputData[i + 1, j - 1] + InputData[Count, j - 1] + InputData[Count, j] + InputData[Count, j + 1];
                        if ((LiveNeigbour == 3) && (InputData[i, j] == 0))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else if (((LiveNeigbour == 2) || (LiveNeigbour == 3)) && (InputData[i, j] == 1))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else this.TempData[i,j] = 0;
                        goto Next;
                    }
                    if ((i == Count) && ((j != 0) && (j != Count)))
                    {
                        LiveNeigbour = InputData[Count, j - 1] + InputData[Count, j + 1] + InputData[Count - 1, j - 1] + InputData[Count - 1, j] + InputData[Count - 1, j + 1] + InputData[0, j - 1] + InputData[0, j] + InputData[0, j + 1];
                        if ((LiveNeigbour == 3) && (InputData[i, j] == 0))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else if (((LiveNeigbour == 2) || (LiveNeigbour == 3)) && (InputData[i, j] == 1))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else this.TempData[i, j] = 0;
                        goto Next;
                    }
                    if ((j == 0) && (i != 0) && (i != Count))
                    {
                        LiveNeigbour = InputData[i - 1, j] + InputData[i + 1, j] + InputData[i - 1, j + 1] + InputData[i, j + 1] + InputData[i + 1, j + 1] + InputData[i - 1, Count] + InputData[i , Count] + InputData[i + 1, Count];
                        if ((LiveNeigbour == 3) && (InputData[i, j] == 0))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else if (((LiveNeigbour == 2) || (LiveNeigbour == 3)) && (InputData[i, j] == 1))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else this.TempData[i, j] = 0;
                        goto Next;
                    }
                    if ((j==Count) && (i!=0) &&(i!=Count))
                    {
                        LiveNeigbour = InputData[i-1 , j-1] + InputData[i , j-1] + InputData[i + 1, j - 1] + InputData[i-1, j] + InputData[i + 1, j] + InputData[i - 1, 0] + InputData[i, 0] + InputData[i + 1, 0];
                        if ((LiveNeigbour == 3) && (InputData[i, j] == 0))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else if (((LiveNeigbour == 2) || (LiveNeigbour == 3)) && (InputData[i, j] == 1))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else this.TempData[i, j] = 0;
                        goto Next;
                    }
                    if ((i!=0) && (i!=Count) && (j!=0) && (j!=Count))
                    {
                        LiveNeigbour = InputData[i - 1, j - 1] + InputData[i, j - 1] + InputData[i + 1, j - 1] + InputData[i + 1, j] + InputData[i + 1, j+1] + InputData[i, j+1] + InputData[i-1, j+1] + InputData[i -1, j];
                        if ((LiveNeigbour == 3) && (InputData[i, j] == 0))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else if (((LiveNeigbour == 2) || (LiveNeigbour == 3)) && (InputData[i, j] == 1))
                        {
                            this.TempData[i, j] = 1;
                        }
                        else this.TempData[i, j] = 0;
                        goto Next;
                    }
                Next:;
                }
            }
            for (int i = 0; i<=Count;i++)
            {
                for (int j=0;j<=Count;j++)
                {
                    if (TempData[i, j] == 0)
                    {
                        g1.DrawRectangle(new Pen(Color.Black), i * Scale, j * Scale, Scale, Scale);
                    }
                    else g1.FillRectangle(Brushes.Black, i * Scale, j * Scale, Scale, Scale);
                    this.InputData[i, j] = TempData[i, j];
                   
                }
            }
            g.Clear(Color.White);
            GameField.Image = bmp; 
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(textBox2.Text);
            timer1.Start();
            timer1.Enabled = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.StepButton_Click(sender, e);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Graphics g = GameField.CreateGraphics();
            Bitmap bmp = new Bitmap(GameField.Width, GameField.Height);
            Graphics g1 = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            for (int i = 0; i < GameField.Width; i += Scale)
            {
                g1.DrawLine(new Pen(Color.Black), i, 0, i, GameField.Height);
                g1.DrawLine(new Pen(Color.Black), 0, i, GameField.Width, i);
            }
            for (int i = 0; i <= Countt; i++)
            {
                for (int j = 0; j <= Countt; j++)
                {
                    if ((i % 3 == 0) || (j % 3 == 0))
                    {
                        this.InputData[i, j] = 0;
                    }
                    else this.InputData[i, j] = 1;
                }
            }
            for (int i = 0; i < Countt; i++)
            {
                for (int j = 0; j < Countt; j++)
                {
                    if (this.InputData[i, j] == 0)
                    {
                        g1.DrawRectangle(new Pen(Color.Black), i * Scale, j * Scale, Scale, Scale);
                    }
                    else g1.FillRectangle(Brushes.Black, i * Scale, j * Scale, Scale, Scale);
                }
            }
                    GameField.Image = bmp;

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Graphics g = GameField.CreateGraphics();
            Bitmap bmp = new Bitmap(GameField.Width, GameField.Height);
            Graphics g1 = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            for (int i = 0; i < GameField.Width; i += Scale)
            {
                g1.DrawLine(new Pen(Color.Black), i, 0, i, GameField.Height);
                g1.DrawLine(new Pen(Color.Black), 0, i, GameField.Width, i);
            }
            for (int i = 0; i <= Countt; i++)
            {
                this.InputData[i, i] = 1;
                this.InputData[i, Countt - i] = 1;
            }
            for (int i = 0; i < Countt; i++)
            {
                for (int j = 0; j < Countt; j++)
                {
                    if (this.InputData[i, j] == 0)
                    {
                        g1.DrawRectangle(new Pen(Color.Black), i * Scale, j * Scale, Scale, Scale);
                    }
                    else g1.FillRectangle(Brushes.Black, i * Scale, j * Scale, Scale, Scale);
                }
            }
            GameField.Image = bmp;

        }
    }
}
