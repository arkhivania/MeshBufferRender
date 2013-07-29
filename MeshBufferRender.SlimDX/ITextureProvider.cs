using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.SlimDX
{
    public interface ITextureProvider
    {
        SlimTexture GetTexture(string textureName);
    }
}
