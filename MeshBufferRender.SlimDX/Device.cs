using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Direct3D = SlimDX.Direct3D9;

namespace MeshBufferRender.SlimDX
{
    public class Device : MeshBufferRender.Base.IDevice
    {
        readonly Direct3D.Direct3D direct3D = new Direct3D.Direct3D();
        public Direct3D.Direct3D Direct
        {
            get { return direct3D; }
        } 

        readonly Direct3D.Device device;

        public event EventHandler FreeResources;
        public event EventHandler ReloadResources;

        public Direct3D.Device D3DDevice
        {
            get { return device; }
        }

        public bool TryMultisample { get; set; }
        


        public Device(IntPtr windowHandle)
        {
            var prmtrs = new Direct3D.PresentParameters()
            {
                Windowed = true,
                SwapEffect = Direct3D.SwapEffect.Discard
            };

            device = new Direct3D.Device(direct3D, 0, Direct3D.DeviceType.Hardware, windowHandle
                , Direct3D.CreateFlags.NoWindowChanges 
                | Direct3D.CreateFlags.FpuPreserve 
                | Direct3D.CreateFlags.HardwareVertexProcessing, prmtrs);
        }

        public MeshBufferRender.Base.IRenderSurface CreateRenderSurface(int width, int height, MeshBufferRender.Base.PixelFormat format)
        {
            if (TryMultisample)
            {
                try
                {
                    return new RenderSurface(this, width, height, format, Direct3D.MultisampleType.FourSamples);
                }
                catch(MultiSampleNotSupportedException) 
                {
                    return new RenderSurface(this, width, height, format, Direct3D.MultisampleType.None);
                }
            }else
                return new RenderSurface(this, width, height, format, Direct3D.MultisampleType.None);
        }

        bool disposed = false;

        public void Dispose()
        {
            if (!disposed)
            {
                device.Dispose();
                direct3D.Dispose();
            }
        }
    }
}
