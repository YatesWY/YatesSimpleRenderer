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
        private List<Vertex> vertices = new List<Vertex>();

        public List<Vertex> Vertices
        {
            get
            {
                return this.vertices;
            }
        }

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
                        case "v":
                            var pos = new Vector3(float.Parse(splits[1]), float.Parse(splits[2]), float.Parse(splits[3]));
                            this.vertices.Add(new Vertex(pos));
                            break;                           
                    }
                }
            }
        }
    }
}
