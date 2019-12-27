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
        public void DrawPoint(int x, int y, Color color)
        {
            this.frontBuffer.SetPixel(x, y, color);
        }

        /// <summary>
        /// 画线函数
        /// </summary>
        /// <param name="pointStart"></param>
        /// <param name="pointEnd"></param>
        /// <param name="color"></param>
        public void DrawLine(Vector3 pointStart, Vector3 pointEnd, Color color)
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
        public void DrawTriangle(Vector3[] points, Color color, bool wireframe = false)
        {
            if (wireframe)
            {
                this.DrawLine(points[0], points[1], color);
                this.DrawLine(points[1], points[2], color);
                this.DrawLine(points[2], points[0], color);
            }
            else
            {
                Vector3 bboxmin = new Vector3(this.frontBuffer.Width - 1, this.frontBuffer.Height - 1);
                Vector3 bboxmax = Vector3.zero;
                Vector3 clamp = new Vector3(this.frontBuffer.Width - 1, this.frontBuffer.Height - 1);
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        bboxmin[j] = Math.Max(0, Math.Min(bboxmin[j], points[i][j]));
                        bboxmax[j] = Math.Min(clamp[j], Math.Max(bboxmax[j], points[i][j]));
                    }
                }

                Vector3 p = default(Vector3);
                for (p.x = bboxmin.x; p.x <= bboxmax.x; p.x++)
                {
                    for (p.y = bboxmin.y; p.y <= bboxmax.y; p.y++)
                    {
                        if (this.IsPointInTriangle(points, p))
                        {
                            this.DrawPoint((int)p.x, (int)p.y, color);
                        }
                    }
                }
            }            
        }

        public bool IsPointInTriangle(Vector3[] points, Vector3 target)
        {
            Vector3 v0 = points[2] - points[0];
            Vector3 v1 = points[1] - points[0];
            Vector3 v2 = target - points[0];

            float dot00 = Vector3.Dot(v0, v0);
            float dot01 = Vector3.Dot(v0, v1);
            float dot02 = Vector3.Dot(v0, v2);
            float dot11 = Vector3.Dot(v1, v1);
            float dot12 = Vector3.Dot(v1, v2);

            float inverDeno = 1 / ((dot00 * dot11) - dot01 * dot01);

            float u = (dot11 * dot02 - dot01 * dot12) * inverDeno;

            if (u < 0 || u > 1) // if u out of range, return directly
            {
                return false;
            }

            float v = (dot00 * dot12 - dot01 * dot02) * inverDeno;

            if (v < 0 || v > 1) // if v out of range, return directly
            {
                return false;
            }

            return u + v <= 1;
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

                this.DrawTriangle(
                    new[] { new Vector3(300, 50), new Vector3(250, 200), new Vector3(320, 160) },
                    Color.White);
                this.DrawTriangle(
                    new[] { new Vector3(400, 50), new Vector3(500, 200), new Vector3(550, 160) },
                    Color.Green);
                this.DrawTriangle(
                    new[] { new Vector3(550, 50), new Vector3(500, 100), new Vector3(600, 160) },
                    Color.Red);

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