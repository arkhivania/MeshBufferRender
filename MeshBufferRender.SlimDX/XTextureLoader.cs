using MeshBufferRender.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.SlimDX
{
    public static class XTextureLoader
    {
        public static SlimTexture LoadTexture(Device device, string fileName)
        {
            return new SlimTexture(device, new Base.StreamSources.FileStreamSource(fileName));
        }
    }
}
