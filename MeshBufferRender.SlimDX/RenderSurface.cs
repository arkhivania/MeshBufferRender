using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDx = SlimDX;

namespace MeshBufferRender.SlimDX
{
    class RenderSurface : Base.IRenderSurface
    {
        SlimDx.Direct3D9.Surface renderSurface;
        public SlimDx.Direct3D9.Surface Surface
        {
            get { return renderSurface; }
        }

        SlimDx.Direct3D9.Surface offscreenSurface;
        public SlimDx.Direct3D9.Surface OffscreenSurface
        {
            get { return offscreenSurface; }
        }

        SlimDx.Direct3D9.Surface depthBuffer;
        public SlimDx.Direct3D9.Surface DepthBuffer
        {
            get { return depthBuffer; }
        }

        private readonly int width;

        private readonly int height;

        private readonly Base.PixelFormat pixelFormat;

        private readonly Device device;

        public RenderSurface(Device device, int width, int height, Base.PixelFormat pixelFormat)
        {
            this.device = device;
            this.pixelFormat = pixelFormat;
            this.height = height;
            this.width = width;

            device.FreeResources += device_FreeResources;
            device.ReloadResources += device_ReloadResources;

            if (pixelFormat != Base.PixelFormat.BGRA)
                throw new ArgumentException("BGRA format only supported");

            CreateTarget();
        }

        void device_ReloadResources(object sender, EventArgs e)
        {
            FreeSurface();
            CreateTarget();
        }
  
        private void CreateTarget()
        {
            if (renderSurface != null)
                FreeSurface();

            renderSurface = SlimDx.Direct3D9.Surface.CreateRenderTarget(device.D3DDevice, 
                width, height, 
                SlimDx.Direct3D9.Format.A8R8G8B8, 
                SlimDx.Direct3D9.MultisampleType.None, 
                0, true);

            depthBuffer = SlimDx.Direct3D9.Surface.CreateDepthStencil(device.D3DDevice, 
                width, height, 
                SlimDx.Direct3D9.Format.D16, 
                SlimDx.Direct3D9.MultisampleType.None, 
                0, true);

            offscreenSurface = SlimDx.Direct3D9.Surface.CreateOffscreenPlain(device.D3DDevice, width, height, SlimDx.Direct3D9.Format.A8R8G8B8, SlimDx.Direct3D9.Pool.SystemMemory);
        }
  
        private void FreeSurface()
        {
            if (renderSurface != null)
            {
                renderSurface.Dispose();
                depthBuffer.Dispose();
                offscreenSurface.Dispose();

                renderSurface = null;
                depthBuffer = null;
                offscreenSurface = null;
            }
        }

        void device_FreeResources(object sender, EventArgs e)
        {
            FreeSurface();
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        class Scope : Base.IBufferScope
        {
            private readonly SlimDx.Direct3D9.Surface surface;

            readonly Base.BufferData data;

            public Scope(RenderSurface renderSurface)
            {
                this.surface = renderSurface.offscreenSurface;

                var rect = surface.LockRectangle(SlimDx.Direct3D9.LockFlags.ReadOnly);

                data = new Base.BufferData
                {
                    Width = renderSurface.width, 
                    Height = renderSurface.height, 
                    Format = Base.PixelFormat.BGRA, 
                    Pointer = rect.Data.DataPointer, 
                    Stride = rect.Pitch
                };
                
            }


            public Base.BufferData Data
            {
                get 
                {
                    return data;
                }
            }

            public void Dispose()
            {
                var hr = surface.UnlockRectangle();
                if (!hr.IsSuccess)
                    throw new InvalidOperationException();
            }
        }

        public Base.IBufferScope GetScope()
        {
            return new Scope(this);
        }

        public void Dispose()
        {
            FreeSurface();

            device.FreeResources -= device_FreeResources;
            device.ReloadResources -= device_ReloadResources;
        }
    }
}
