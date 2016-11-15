using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Game01
{
    public partial class Game01 : Form
    {
        Thread tr;
        GamePanel gamePanel = new GamePanel();
        Image[] animatie = new Image[4];
        ResourceManager resoureManager = new ResourceManager("Game01.Properties.Resources",
            Assembly.GetExecutingAssembly());

        private Point loc = new Point(0,0);
        private bool up, down, left, right;
        int i = 0;
        bool animating = false;
        public Game01()
        {
            InitializeComponent();
            Image asdf = (Image)resoureManager.GetObject("walking");
            gamePanel.Size = asdf.Size;
            ClientSize = asdf.Size;
            gamePanel.Location = new Point(0,0);
            
            Controls.Add(gamePanel);
            //SetAnimatie();
            
            FormClosing += sluit;
            gamePanel.Paint += (sender, args) =>
            {
                FrameDimension dimension = new FrameDimension(asdf.FrameDimensionsList[0]);
                // Number of frames
                int frameCount = asdf.GetFrameCount(dimension);
                asdf.SelectActiveFrame(dimension, i);
                Text = frameCount.ToString() + ", " + i;
                //Text = asdf.GetFrameCount(dimension).ToString();
                args.Graphics.FillRectangle(Brushes.Black, loc.X,loc.Y, 20,20);
            };
            KeyDown += (sender, args) => { animating = true;
                if (args.KeyCode == Keys.Up)
                    up = true;
                if (args.KeyCode == Keys.Down)
                    down = true;
                if (args.KeyCode == Keys.Left)
                    left = true;
                if (args.KeyCode == Keys.Right)
                    right = true;
                if (up && down)
                {
                    up = false;
                    down = false;
                }
                if (left && right)
                {
                    left = false;
                    right = false;
                }
            };
            KeyUp += (sender, args) =>
            {
                if (args.KeyCode == Keys.Up)
                    up = false;
                if (args.KeyCode == Keys.Down)
                    down = false;
                if (args.KeyCode == Keys.Left)
                    left = false;
                if (args.KeyCode == Keys.Right)
                    right = false;
            };
            tr = new Thread(Animate);
            tr.Start();
        }
        

        private void Invalidated()
        {
            gamePanel.Invalidate();
        }

        private void Animate()
        {
            while (true)
            {
                if (animating)
                {
                    if (i < 3)
                        i++;
                    else
                        i = 0;
                    //Text = i.ToString();
                    gamePanel.Invalidate();
                }
                if (up)
                    loc.Y--;
                if (down)
                    loc.Y++;
                if (left)
                    loc.X--;
                if (right)
                    loc.X++;
                Thread.Sleep(110);
            }
        }

        private void sluit(object o, EventArgs ea)
        {
            tr.Abort();
        }
    
        private void SetAnimatie()
        {
            Image gifImg = (Image)resoureManager.GetObject("walking");
            FrameDimension dimension = new FrameDimension(gifImg.FrameDimensionsList[0]);
            // Number of frames
            int frameCount = gifImg.GetFrameCount(dimension);
            // Return an Image at a certain index
            for (int i = 0; i < frameCount; i++)
            {
                gifImg.SelectActiveFrame(dimension, i);
                animatie[i] = new Bitmap(gifImg);
            }
            MessageBox.Show(frameCount.ToString());
        }
    }

    public class GamePanel : Panel
    {
        public GamePanel()
        {
            DoubleBuffered = true;
            BackColor = Color.ForestGreen;

        }
    }
}

