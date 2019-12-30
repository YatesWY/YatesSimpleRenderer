using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatesSimpleRenderer.Yates
{
    using System.IO;

    class ObjLoader
    {
        public static Model3D LoadModel(string path)
        {
            StreamReader file = new StreamReader(path);
            return new Model3D(file);
        }
    }
}
