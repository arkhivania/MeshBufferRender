using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public interface IBufferScope : IDisposable
    {
        BufferData Data { get; }
    }
}
