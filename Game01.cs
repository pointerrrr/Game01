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
                // Return an Image at a certain index
                asdf.SelectActiveFrame(dimension, i);
                Text = frameCount.ToString() + ", " + i;
                //Text = asdf.GetFrameCount(dimension).ToString();
                args.Graphics.DrawImage(asdf, 0,0, Width,Height);
                
                
            };
            KeyDown += (sender, args) => { animating = true;
                Text = "asdf";
            };
            KeyUp += (sender, args) => { animating = false; };
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
                    Thread.Sleep(110);
                }
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

