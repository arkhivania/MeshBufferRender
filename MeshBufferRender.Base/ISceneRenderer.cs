using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public interface ISceneRenderer
    {
        void Render(Camera camera, IScene scene, IRenderSurface renderSurface);
    }
}
