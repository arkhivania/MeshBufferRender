using SlimDX;
using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.SlimDX
{
    class SlimObject : Base.IMeshObject
    {
        Mesh mesh = null;

        private readonly Device device;

        private readonly Base.IStreamSource meshStreamSorce;

        private readonly ITextureProvider textureProvider;

        public ITextureProvider TextureProvider
        {
            get { return textureProvider; }
        } 


        public Mesh Mesh
        {
            get 
            {
                if (mesh == null)
                    throw new ResourceUnloadedException();
                return mesh; 
            }
        }

        public SlimObject(Device device, Base.IStreamSource meshStreamSorce, ITextureProvider textureProvider)
        {
            this.textureProvider = textureProvider;
            this.meshStreamSorce = meshStreamSorce;
            this.device = device;

            device.FreeResources += device_FreeResources;
            device.ReloadResources += device_ReloadResources;

            LoadMesh();
        }

        void device_ReloadResources(object sender, EventArgs e)
        {            
            LoadMesh();
        }
  
        private void LoadMesh()
        {
            if(mesh != null)
                FreeMesh();

            using (var stream = meshStreamSorce.StartRead())
                mesh = Mesh.FromStream(device.D3DDevice, stream, MeshFlags.Managed);
        }

        void device_FreeResources(object sender, EventArgs e)
        {
            FreeMesh();
        }

        public void Dispose()
        {
            FreeMesh();
        }
  
        private void FreeMesh()
        {
            if (mesh != null)
            {
                mesh.Dispose();
                mesh = null;
            }
        }

        Base.Matrix4x4 worldTransform = new Base.Matrix4x4(1);
        public Base.Matrix4x4 WorldTransform
        {
            get
            {
                return worldTransform;
            }
            set
            {
                worldTransform = value;
            }
        }
    }
}
