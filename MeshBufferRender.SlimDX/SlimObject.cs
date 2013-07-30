using System;
using System.Linq;
using MeshBufferRender.Base;
using SlimDX.Direct3D9;

namespace MeshBufferRender.SlimDX
{
    public class SlimObject : MeshBufferRender.Base.IMeshObject, Base.ISlimDrawObject
    {
        Mesh mesh = null;

        private readonly Device device;

        private readonly MeshBufferRender.Base.IStreamSource meshStreamSorce;

        private readonly ITextureProvider textureProvider;

        public SlimObject(IDevice device, MeshBufferRender.Base.IStreamSource meshStreamSorce, ITextureProvider textureProvider)
        {
            this.textureProvider = textureProvider;
            this.meshStreamSorce = meshStreamSorce;
            this.device = (Device)device;

            this.device.FreeResources += device_FreeResources;
            this.device.ReloadResources += device_ReloadResources;

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

        MeshBufferRender.Base.Matrix4x4 worldTransform = new MeshBufferRender.Base.Matrix4x4(1);
        public MeshBufferRender.Base.Matrix4x4 WorldTransform
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

        public void Draw(Device device)
        {
            var device3D = device.D3DDevice;

            device3D.SetRenderState(RenderState.ShadeMode, ShadeMode.Gouraud).Assert();
            device3D.SetRenderState(RenderState.CullMode, Cull.Clockwise).Assert();
            device3D.SetRenderState(RenderState.Lighting, true).Assert();
            device3D.SetRenderState(RenderState.ZEnable, true).Assert();
            device3D.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Linear);
            device3D.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Linear);

            device3D.SetTransform(TransformState.World, WorldTransform.ToSlimDXMatrix()).Assert();

            foreach (var material in mesh.GetMaterials().Select((m, index) => new { Material = m, index }))
            {
                if (!string.IsNullOrEmpty(material.Material.TextureFileName))
                {
                    device3D.SetTexture(0, textureProvider.GetTexture(material.Material.TextureFileName).Texture).Assert();
                    device3D.Material = material.Material.MaterialD3D;
                    mesh.DrawSubset(material.index).Assert();

                    device3D.SetTexture(0, null).Assert();
                }
            }
        }
    }
}
