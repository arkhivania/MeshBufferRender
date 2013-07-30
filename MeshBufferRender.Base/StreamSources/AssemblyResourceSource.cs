using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MeshBufferRender.Base.StreamSources
{
    public class AssemblyResourceSource : IStreamSource
    {
        private readonly Assembly assembly;

        private readonly string resourceName;

        public AssemblyResourceSource(Assembly assembly, string resourceName)
        {
            this.resourceName = resourceName;
            this.assembly = assembly;
        }

        public System.IO.Stream StartRead()
        {
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
