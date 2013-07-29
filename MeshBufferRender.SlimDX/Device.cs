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
        readonly Direct3D.Device device;

        public event EventHandler FreeResources;
        public event EventHandler ReloadResources;

        public Direct3D.Device D3DDevice
        {
            get { return device; }
        } 


        public Device(IntPtr windowHandle)
        {
            var prmtrs = new Direct3D.PresentParameters()
            {
                AutoDepthStencilFormat = Direct3D.Format.D16,
                EnableAutoDepthStencil = true,
                Windowed = true,
                SwapEffect = Direct3D.SwapEffect.Discard
            };

            device = new Direct3D.Device(direct3D, 0, Direct3D.DeviceType.Hardware, windowHandle
                , Direct3D.CreateFlags.NoWindowChanges 
                | Direct3D.CreateFlags.FpuPreserve 
                | Direct3D.CreateFlags.HardwareVertexProcessing, prmtrs);
        }

        public Base.IRenderSurface CreateRenderSurface(int width, int height, Base.PixelFormat format)
        {
            return new RenderSurface(this, width, height, format);
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


        public Base.IScene CreateScene()
        {
            return new ConcurrentScene();
        }
    }
}
