using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.SlimDX
{
    public class ResourcesBox : ITextureProvider, IDisposable
    {
        readonly Dictionary<string, SlimTexture> Textures = new Dictionary<string, SlimTexture>();

        public void Push(string textureName, SlimTexture texture)
        {
            lock (Textures)
                Textures[textureName] = texture;
        }

        public SlimTexture GetTexture(string textureName)
        {
            lock (Textures)
                return Textures[textureName];
        }

        public void Dispose()
        {
            foreach (var t in Textures)
                t.Value.Dispose();

            Textures.Clear();
        }
    }
}
