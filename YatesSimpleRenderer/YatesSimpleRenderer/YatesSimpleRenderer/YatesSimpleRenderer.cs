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

        /// <summary>
        /// 画点函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        private void DrawPoint(int x, int y, Color color)
        {
            this.frontBuffer.SetPixel(x, y, color);
        }

        /// <summary>
        /// 画线函数
        /// </summary>
        /// <param name="pointStart"></param>
        /// <param name="pointEnd"></param>
        /// <param name="color"></param>
        private void DrawLine(Vector3 pointStart, Vector3 pointEnd, Color color)
        {
            int y1 = (int)pointStart.y;
            int x1 = (int)pointStart.x;
            int y2 = (int)pointEnd.y;
            int x2 = (int)pointEnd.x;
            bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);

            if (steep)
            {
                Mathf.Swap(ref x1, ref y1);
                Mathf.Swap(ref x2, ref y2);
            }

            if (x1 > x2)
            {
                Mathf.Swap(ref x1, ref x2);
                Mathf.Swap(ref y1, ref y2);
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

        /// <summary>
        /// 画三角面
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <param name="pointC"></param>
        /// <param name="color"></param>
        private void DrawTriangle(Vector3 pointA, Vector3 pointB, Vector3 pointC, Color color, bool wireframe = false)
        {
            // 顶点根据y排序
            if (pointA.y > pointB.y)
            {
                Mathf.Swap(ref pointA, ref pointB);
            }

            if (pointA.y > pointC.y)
            {
                Mathf.Swap(ref pointA, ref pointC);
            }

            if (pointB.y > pointC.y)
            {
                Mathf.Swap(ref pointB, ref pointC);
            }

            if (wireframe)
            {
                //线框模式
                this.DrawLine(pointA, pointB, color);
                this.DrawLine(pointB, pointC, color);
                this.DrawLine(pointC, pointA, color);
            }
            else
            {
                // 将三角面分成上下两块来画
                var acHeight = pointC.y - pointA.y + 1f;
                var abHeight = pointB.y - pointA.y + 1f;
                for (int y = (int)pointA.y; y <= (int)pointB.y; y++)
                {
                    var dac = (y - pointA.y) / acHeight;
                    var dab = (y - pointA.y) / abHeight;
                    Vector3 a = pointA + ((pointC - pointA) * dac);
                    Vector3 b = pointA + ((pointB - pointA) * dab);
                    this.DrawLine(a, b, color);
                }

                var bcHeight = pointC.y - pointB.y + 1;
                for (int y = (int)pointB.y; y <= (int)pointC.y; y++)
                {
                    var dac = (y - pointA.y) / acHeight;
                    var dbc = (y - pointB.y) / bcHeight;
                    Vector3 a = pointA + ((pointC - pointA) * dac);
                    Vector3 b = pointB + ((pointC - pointB) * dbc);
                    this.DrawLine(a, b, color);
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

                this.DrawTriangle(new Vector3(300, 50), new Vector3(250, 200), new Vector3(320, 160), Color.White);
                this.DrawTriangle(new Vector3(400, 50), new Vector3(500, 200), new Vector3(550, 160), Color.Green);
                this.DrawTriangle(new Vector3(550, 50), new Vector3(500, 100), new Vector3(600, 160), Color.Red);

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