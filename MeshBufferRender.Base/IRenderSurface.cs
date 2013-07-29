using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public interface IRenderSurface
    {
        int Width { get; }
        int Height { get; }

        IBufferScope GetScope();
    }
}
