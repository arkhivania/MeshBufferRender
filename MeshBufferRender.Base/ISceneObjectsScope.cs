﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public interface ISceneObjectsScope : IDisposable
    {
        IEnumerable<IMeshObject> MeshObjects { get; }
    }
}
