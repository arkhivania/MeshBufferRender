using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public interface IScene
    {
        ISceneObjectsScope GetScope();

        Color Color { get; set; }
        IEnumerable<Light> Lights { get; }        
    }
}
