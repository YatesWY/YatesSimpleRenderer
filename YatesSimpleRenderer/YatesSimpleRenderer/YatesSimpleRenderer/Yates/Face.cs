namespace YatesSimpleRenderer.Yates
{
    using System;

    class Face
    {
        public Face(Model3D model, string[] details)
        {
            for (int i = 0; i < 3; i++)
            {
                var detail = details[i].Split('/');
                this.Vertices[i] = model.Vertices[int.Parse(detail[0]) - 1];
            }
        }

        public Vertex[] Vertices { get; } = new Vertex[3];

        public Vertex GetVertex(int idx)
        {
            if (idx < 0 || idx >= this.Vertices.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            return this.Vertices[idx];
        }
    }
}
