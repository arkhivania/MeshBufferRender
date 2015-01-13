using MeshBufferRender.Base;
using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDx = SlimDX;

namespace MeshBufferRender.SlimDX
{
    class RenderSurface : MeshBufferRender.Base.IRenderSurface
    {
        SlimDx.Direct3D9.Surface renderSurface;
        private readonly SlimDx.Direct3D9.MultisampleType multiSampleType;

        private readonly DeviceType deviceType;

        private readonly ILog logService;

        public SlimDx.Direct3D9.Surface Surface
        {
            get { return renderSurface; }
        }

        SlimDx.Direct3D9.Surface offscreenDownsampledRenderTarget;

        public SlimDx.Direct3D9.Surface OffscreenDownsampledRenderTarget
        {
            get { return offscreenDownsampledRenderTarget; }
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

        private readonly MeshBufferRender.Base.PixelFormat pixelFormat;

        private readonly Device device;

        public RenderSurface(Device device, int width, int height, MeshBufferRender.Base.PixelFormat pixelFormat, SlimDx.Direct3D9.MultisampleType multiSampleType, DeviceType deviceType, ILog logService)
        {
            this.multiSampleType = multiSampleType;
            this.deviceType = deviceType;
            this.logService = logService;
            this.device = device;
            this.pixelFormat = pixelFormat;
            this.height = height;
            this.width = width;

            device.FreeResources += device_FreeResources;
            device.ReloadResources += device_ReloadResources;

            if (pixelFormat != MeshBufferRender.Base.PixelFormat.BGRA)
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

            int qualityLevel = 0;
            int qualityLevels;

            if (device.Direct.CheckDeviceMultisampleType(0, deviceType, SlimDx.Direct3D9.Format.A8R8G8B8, false, multiSampleType, out qualityLevels))
                qualityLevel = qualityLevels - 1;
            else
                throw new MultiSampleNotSupportedException();

            this.logService.WriteInformation(string.Concat("Device multisample qualityLevel:", qualityLevel));
            renderSurface = SlimDx.Direct3D9.Surface.CreateRenderTarget(device.D3DDevice, 
                width, height, 
                SlimDx.Direct3D9.Format.A8R8G8B8, 
                multiSampleType, 
                qualityLevel, false);

            depthBuffer = SlimDx.Direct3D9.Surface.CreateDepthStencil(device.D3DDevice, 
                width, height, 
                SlimDx.Direct3D9.Format.D24S8,
                multiSampleType, 
                qualityLevel, false);

            offscreenDownsampledRenderTarget = SlimDx.Direct3D9.Surface.CreateRenderTarget(device.D3DDevice,
                width, height,
                SlimDx.Direct3D9.Format.A8R8G8B8,
                SlimDx.Direct3D9.MultisampleType.None, 0, false);

            offscreenSurface = SlimDx.Direct3D9.Surface.CreateOffscreenPlain(device.D3DDevice, width, height, SlimDx.Direct3D9.Format.A8R8G8B8, SlimDx.Direct3D9.Pool.SystemMemory);
            logService.WriteInformation(string.Format("Render surface: {0}", new { MultisampleType = multiSampleType, MultisampleQuality = renderSurface.Description.MultisampleQuality, Width = width, Height = renderSurface.Description.Height }));
        }
  
        private void FreeSurface()
        {
            if (renderSurface != null)
            {
                renderSurface.Dispose();
                depthBuffer.Dispose();
                offscreenDownsampledRenderTarget.Dispose();
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

        class Scope : MeshBufferRender.Base.IBufferScope
        {
            private readonly SlimDx.Direct3D9.Surface surface;

            readonly MeshBufferRender.Base.BufferData data;

            public Scope(RenderSurface renderSurface)
            {
                this.surface = renderSurface.offscreenSurface;

                var rect = surface.LockRectangle(SlimDx.Direct3D9.LockFlags.ReadOnly);

                data = new MeshBufferRender.Base.BufferData
                {
                    Width = renderSurface.width, 
                    Height = renderSurface.height,
                    Format = MeshBufferRender.Base.PixelFormat.BGRA, 
                    Pointer = rect.Data.DataPointer, 
                    Stride = rect.Pitch
                };
                
            }


            public MeshBufferRender.Base.BufferData Data
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

        public MeshBufferRender.Base.IBufferScope GetScope()
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
