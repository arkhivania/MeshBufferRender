using MeshBufferRender.Base;
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
        private readonly ILog logService;

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

        readonly DeviceType deviceType;        


        public Device(IntPtr windowHandle, ILog logService)
        {
            this.logService = logService;
            PresentParameters presentParameter = new PresentParameters()
            {
                Windowed = true,
                SwapEffect = SwapEffect.Discard
            };

            try
            {
                this.deviceType = DeviceType.Hardware;
                this.device = new Direct3D.Device(direct3D, 0, deviceType, windowHandle, CreateFlags.NoWindowChanges | CreateFlags.HardwareVertexProcessing | CreateFlags.FpuPreserve, presentParameter);
            }
            catch (Exception exception)
            {
                logService.WriteError(string.Concat("Device initialization failed (fallback to software device):", exception.ToString()));
                this.deviceType = DeviceType.Software;
                this.device = new Direct3D.Device(direct3D, 0, deviceType, windowHandle, CreateFlags.NoWindowChanges | CreateFlags.HardwareVertexProcessing | CreateFlags.FpuPreserve, presentParameter);
            }
        }

        public MeshBufferRender.Base.IRenderSurface CreateRenderSurface(int width, int height, MeshBufferRender.Base.PixelFormat format)
        {
            if (TryMultisample)
            {
                try
                {
                    return new RenderSurface(this, width, height, format, Direct3D.MultisampleType.FourSamples, deviceType, logService);
                }
                catch (MultiSampleNotSupportedException multiSampleNotSupportedException) 
                {
                    this.logService.WriteError(string.Concat("Multisample surface render error, creating without mutlisample:", multiSampleNotSupportedException));
                    return new RenderSurface(this, width, height, format, Direct3D.MultisampleType.None, deviceType, logService);
                }
            }else
                return new RenderSurface(this, width, height, format, Direct3D.MultisampleType.None, deviceType, logService);
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
