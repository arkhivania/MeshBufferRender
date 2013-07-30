using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.SlimDX.Base
{
    public interface ISlimDrawObject
    {
        void Draw(Device device);
    }
}
