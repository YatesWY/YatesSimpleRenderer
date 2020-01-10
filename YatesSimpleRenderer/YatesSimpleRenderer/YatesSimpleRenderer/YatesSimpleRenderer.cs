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
        private Bitmap frontBuffer;

        private Graphics screen;

        private Model3D model;

        private float[] zBuffer;

        private int width;

        private int height;

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
            if (x < 0 || y < 0 || x >= this.width || y >= this.height)
            {
                return;
            }

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
                Vector3 bboxmin = new Vector3(this.width - 1, this.height - 1);
                Vector3 bboxmax = Vector3.zero;
                Vector3 clamp = new Vector3(this.width - 1, this.height - 1);
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
                        if (this.IsPointInTriangle(points, p) && this.zBuffer[(int)p.x + ((int)p.y * this.width)] < p.z)
                        {
                            this.zBuffer[(int)p.x + ((int)p.y * this.width)] = p.z;
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

        private void Start()
        {
            
            this.model = ObjLoader.LoadModel(
                @"..\_obj\african_head.obj");
            this.width = this.ClientSize.Width;
            this.height = this.ClientSize.Height;
            this.frontBuffer = new Bitmap(this.width, this.height);
            this.screen = this.CreateGraphics();
            this.zBuffer = new float[this.width * this.height];
            var mainTimer = new System.Timers.Timer(1000 / 60f);
            mainTimer.Elapsed += new ElapsedEventHandler(this.Update);
            mainTimer.AutoReset = true;
            mainTimer.Enabled = true;
            mainTimer.Start();          
        }

        private void ClearBuffer()
        {
            for (int i = 0; i < this.zBuffer.Length; i++)
            {
                this.zBuffer[i] = float.MinValue;
            }
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            lock (this.frontBuffer)
            {          
                this.ClearBuffer();
                for (int i = 0; i < this.model.Faces.Count; i++)
                {
                    var face = this.model.Faces[i];
                    Vector3[] screenPoints = new Vector3[3];
                    Vector3[] worldPoints = new Vector3[3]; 
                    for (int j = 0; j < screenPoints.Length; j++)
                    {
                        worldPoints[j] = face.Vertices[j].Pos;
                        screenPoints[j].x = (worldPoints[j].x + 1) * this.width / 2;
                        screenPoints[j].y = this.height - ((worldPoints[j].y + 1) * this.height / 2);
                        screenPoints[j].z = worldPoints[j].z;
                    }

                    // 求三角面法线方向
                    var n = Vector3.Cross(worldPoints[2] - worldPoints[0], worldPoints[1] - worldPoints[0]).normalized;
                    var lightDir = Vector3.back;
                    var intensity = Vector3.Dot(n, lightDir.normalized);
                    if (intensity < 0)
                    {
                        // 剔除掉点积为负的面
                        continue;
                    }

                    this.DrawTriangle(screenPoints, Color.FromArgb(255, (int)(intensity * 255), (int)(intensity * 255), (int)(intensity * 255)));
                }

                this.screen.Clear(Color.Black);
                this.screen.DrawImage(this.frontBuffer, 0, 0);
            }           
        }
    }
}