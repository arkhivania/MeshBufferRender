using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MeshBufferRender.Base.StreamSources
{
    public class FileStreamSource : IStreamSource
    {
        private readonly string fileName;

        public FileStreamSource(string fileName)
        {
            this.fileName = fileName;
        }

        public System.IO.Stream StartRead()
        {
            return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}
