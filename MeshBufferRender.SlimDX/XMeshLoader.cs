using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.SlimDX
{
    public static class XMeshLoader
    {
        public static Base.IMeshObject LoadObject(Device device, string fileName, ITextureProvider textureProvider)
        {
            return new SlimObject(device, new Base.StreamSources.FileStreamSource(fileName), textureProvider);
        }
    }
}
