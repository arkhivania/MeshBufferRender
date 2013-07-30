using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public struct Light
    {
        public Vector3 Position { get; set; }
        public Color Diffuse { get; set; }
        public float Attenuation0 { get; set; }
        public float Range { get; set; }
        public Color Ambient { get; set; }
    }
}
