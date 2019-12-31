using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatesSimpleRenderer.Yates
{
    using System.Drawing.Drawing2D;
    using System.IO;

    class Model3D
    {
        private List<Vertex> vertices = new List<Vertex>(64);
        private List<Face> faces = new List<Face>(64);

        public Model3D(StreamReader stream)
        {
            string line;
            while ((line = stream.ReadLine()) != null)
            {
                var splits = line.Split(' ');
                if (splits.Length > 0)
                {
                    switch (splits[0])
                    {
                        // 顶点位置
                        case "v":
                            var pos = new Vector3(float.Parse(splits[1]), float.Parse(splits[2]), float.Parse(splits[3]));
                            this.vertices.Add(new Vertex(pos));
                            break;
                        // 面
                        case "f":
                            this.faces.Add(new Face(this, new string[] { splits[1], splits[2], splits[3] }));
                            break;
                    }
                }
            }
        }

        public List<Vertex> Vertices => this.vertices;

        public List<Face> Faces => this.faces;
    }
}
