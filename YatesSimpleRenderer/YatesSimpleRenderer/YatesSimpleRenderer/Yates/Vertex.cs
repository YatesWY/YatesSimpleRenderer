using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatesSimpleRenderer.Yates
{
    class Vertex
    {
        public Vertex(Vector3 pos)
        {
            this.Pos = pos;
        }

        /// <summary>
        /// 顶点坐标
        /// </summary>
        public Vector3 Pos { get; set; }
    }
}
