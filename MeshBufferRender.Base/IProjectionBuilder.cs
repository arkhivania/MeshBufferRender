using MeshBufferRender.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public interface IProjectionBuilder
    {
        Matrix4x4 BuildProjectionMatrix(Camera camera, IRenderSurface renderSurface);
    }
}
