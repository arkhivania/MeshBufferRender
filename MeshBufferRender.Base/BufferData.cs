using System;

namespace MeshBufferRender.Base
{
    public struct BufferData
    {
        public IntPtr Pointer { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Stride { get; set; }

        public PixelFormat Format { get; set; }
    }
}