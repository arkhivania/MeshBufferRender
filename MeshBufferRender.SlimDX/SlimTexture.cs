using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDx = SlimDX;

namespace MeshBufferRender.SlimDX
{
    public class SlimTexture : IDisposable
    {
        SlimDx.Direct3D9.Texture texture;

        public SlimDx.Direct3D9.Texture Texture
        {
            get 
            {
                if (texture == null)
                    throw new ResourceUnloadedException();
                return texture; 
            }
        }

        private readonly Base.IStreamSource streamSource;

        private readonly Device device;

        public SlimTexture(Device device, Base.IStreamSource streamSource)
        { 
            this.device = device;
            this.streamSource = streamSource;

            device.FreeResources += device_FreeResources;
            device.ReloadResources += device_ReloadResources;

            LoadTexture();
        }

        private void LoadTexture()
        {
            if (texture != null)
                FreeTexture();
            using (var stream = streamSource.StartRead())
                texture = SlimDx.Direct3D9.Texture.FromStream(device.D3DDevice, stream);
        }
  
        private void FreeTexture()
        {
            if (texture != null)
            {
                texture.Dispose();
                texture = null;
            }
        }

        void device_FreeResources(object sender, EventArgs e)
        {
            FreeTexture();
        }

        void device_ReloadResources(object sender, EventArgs e)
        {
            LoadTexture();
        }

        public void Dispose()
        {
            device.FreeResources -= device_FreeResources;
            device.ReloadResources -= device_ReloadResources;

            FreeTexture();
        }
    }
}
