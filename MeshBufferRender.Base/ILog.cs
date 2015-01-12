using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base
{
    public interface ILog
    {
        void WriteError(string error);
        void WriteInformation(string information);
    }
}
