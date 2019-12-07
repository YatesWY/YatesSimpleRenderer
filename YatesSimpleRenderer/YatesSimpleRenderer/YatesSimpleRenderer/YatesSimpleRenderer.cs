#region Yates

// ┌──────────────────────────────────────────────────────────────┐
// │    描   述：                                                    
// │    作   者：Yates                                           
// │    版   本：1.0                                                 
// │    创建时间：2019-12-05-8:49 
// │    修改时间：2019-12-07-13:58                   
// └──────────────────────────────────────────────────────────────┘
// ┌──────────────────────────────────────────────────────────────┐
// │    命名空间：Yates                           
// │    文件名：YatesSimpleRenderer.cs                                    
// └──────────────────────────────────────────────────────────────┘

#endregion

namespace YatesSimpleRenderer
{
    using System;
    using System.Drawing;
    using System.Timers;
    using System.Windows.Forms;

    using global::YatesSimpleRenderer.Yates;

    public partial class YatesSimpleRenderer : Form
    {
        private Bitmap backBuffer;

        private Bitmap frontBuffer;

        private Graphics screen;

        public YatesSimpleRenderer()
        {
            this.InitializeComponent();
            this.Start();
        }

        private void DrawPoint(int x, int y, Color color)
        {
            this.frontBuffer.SetPixel(x, y, color);
        }

        private void DrawLine(Vector3 posStart, Vector3 posEnd, Color color)
        {
            int y1 = (int)posStart.y;
            int x1 = (int)posStart.x;
            int y2 = (int)posEnd.y;
            int x2 = (int)posEnd.x;
            bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            int swapTemp;

            if (steep)
            {
                swapTemp = x1;
                x1 = y1;
                y1 = swapTemp;

                swapTemp = x2;
                x2 = y2;
                y2 = swapTemp;
            }

            if (x1 > x2)
            {
                swapTemp = x1;
                x1 = x2;
                x2 = swapTemp;

                swapTemp = y1;
                y1 = y2;
                y2 = swapTemp;
            }

            int error = (x2 - x1) / 2;
            int yStep = y1 < y2 ? 1 : -1;
            int deltaY = Math.Abs(y2 - y1);
            int deltaX = x2 - x1;

            int x = x1, y = y1;

            while (x <= x2)
            {
                if (steep)
                {
                    this.DrawPoint(y, x, color);
                }
                else
                {
                    this.DrawPoint(x, y, color);
                }
                
                x += 1;
                error -= deltaY;
                if (error < 0)
                {
                    y += yStep;
                    error += deltaX;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void Log(string msg)
        {
            lock (this.label1)
            {
                this.label1.Text = msg;
            }
        }

        private void Start()
        {
            this.frontBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.backBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.screen = this.CreateGraphics();
            var mainTimer = new System.Timers.Timer(1000 / 60f);
            mainTimer.Elapsed += new ElapsedEventHandler(this.Update);
            mainTimer.AutoReset = true;
            mainTimer.Enabled = true;
            mainTimer.Start();
        }

        private Vector3 clickPoint;

        private void Update(object sender, ElapsedEventArgs e)
        {
            // TODO double buffer
            lock (this.frontBuffer)
            {
                if (this.clickPoint != default(Vector3))
                {
                    this.DrawLine(new Vector3(400, 225), this.clickPoint, Color.White);
                }
                               
                this.screen.Clear(Color.Black);
                this.screen.DrawImage(this.frontBuffer, 0, 0);
            }           
        }

        private void YatesSimpleRenderer_MouseClick(object sender, MouseEventArgs e)
        {
           this.clickPoint = new Vector3(e.Location.X, e.Location.Y);
        }
    }
}