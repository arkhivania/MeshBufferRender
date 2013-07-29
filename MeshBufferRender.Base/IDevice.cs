using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public interface IDevice : IDisposable
    {
        IRenderSurface CreateRenderSurface(int width, int height, PixelFormat format);
        ISceneRenderer CreateSceneRenderer();
    }
}
