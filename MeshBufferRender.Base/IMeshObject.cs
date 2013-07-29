using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public interface IMeshObject : IDisposable
    {
        Matrix4x4 WorldTransform { get; set; }
    }
}
