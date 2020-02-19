using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSWF_MineSweeper
{
    public partial class Form1 : Form
    {
        private PictureBox pictureBox1 = new PictureBox();
        private Label MineNumLabel = new Label();
        private Label TimeLabel = new Label();
        private Font fnt = new Font("Arial", 10);
        private Image bg = Properties.Resources.back;
        private TileManager tm = new TileManager();

        public Form1()
        {
            InitializeComponent();

            tm.Init();

            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.BackColor = Color.White;
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_OnPaint);
            pictureBox1.MouseDown += new MouseEventHandler(this.Form_MouseDown);
            this.Controls.Add(pictureBox1);

            MineNumLabel.Font = fnt;
            MineNumLabel.BackColor = Color.Transparent;
            MineNumLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            MineNumLabel.ForeColor = Color.White;
            MineNumLabel.Text = tm.MineMax.ToString();
            MineNumLabel.Location = new Point(668, 480);
            MineNumLabel.Size = new Size(MineNumLabel.PreferredWidth, MineNumLabel.PreferredHeight);
            MineNumLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.MineNum_OnPaint);
            pictureBox1.Controls.Add(MineNumLabel);

            Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(this.Time_tick);
            timer.Start();

            TimeLabel.Font = fnt;
            TimeLabel.BackColor = Color.Transparent;
            TimeLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            TimeLabel.ForeColor = Color.White;
            TimeLabel.Text = "0";
            TimeLabel.Location = new Point(175, 480);
            //TimeLabel.Size = new Size(TimeLabel.PreferredWidth, TimeLabel.PreferredHeight);
            TimeLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.MineNum_OnPaint);
            pictureBox1.Controls.Add(TimeLabel);
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            bool Gameover = tm.Input(e);
            pictureBox1.Invalidate();
            MineNumLabel.Invalidate();
            if (Gameover)
            {
                System.Windows.Forms.MessageBox.Show("GameOver!!");
                tm.Init();
                pictureBox1.Invalidate();
                MineNumLabel.Invalidate();
                TimeLabel.Text = "0";
            }
        }

        private void pictureBox1_OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawImage(bg, 0, 0);
            tm.Draw(e);
        }

        private void MineNum_OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            MineNumLabel.Text = (tm.MineMax - tm.FlagNum).ToString();
        }

        private void Time_tick(object sender, EventArgs e)
        {
            int time = int.Parse(TimeLabel.Text);
            time++;
            TimeLabel.Text = time.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
