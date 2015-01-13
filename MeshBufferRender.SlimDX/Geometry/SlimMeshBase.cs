using MeshBufferRender.Base;
using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.SlimDX.Geometry
{
    public class SlimMeshBase<T> : IMeshObject, Base.ISlimDrawObject 
        where T : global::SlimDX.Direct3D9.Mesh
    {
        private readonly Device device;        
        private readonly ITextureProvider textureProvider;
        private readonly ExtendedMaterial material;

        T mesh;

        private readonly Func<T> createMesh;

        public SlimMeshBase(Device device,
            ITextureProvider textureProvider,
            ExtendedMaterial material, Func<T> createMesh)
        {
                this.device = device;
                this.textureProvider = textureProvider;
                this.material = material;

            device.FreeResources += device_FreeResources;
            device.ReloadResources += device_ReloadResources;

            this.createMesh = createMesh;
            WorldTransform = new Matrix4x4(1);

            device_ReloadResources(null, null);
        }

        void device_ReloadResources(object sender, EventArgs e)
        {
            mesh = createMesh();
            mesh.SetMaterials(new[] { material });
        }

        void device_FreeResources(object sender, EventArgs e)
        {
            mesh.Dispose();
        }

        public Matrix4x4 WorldTransform { get; set; } 
        
        public void Dispose()
        {
            mesh.Dispose();

            device.FreeResources -= device_FreeResources;
            device.ReloadResources -= device_ReloadResources;
        }

        public void Draw(Device device)
        {
            var device3D = device.D3DDevice;

            device3D.SetRenderState(RenderState.ShadeMode, ShadeMode.Gouraud).Assert();
            device3D.SetRenderState(RenderState.CullMode, Cull.Counterclockwise).Assert();
            device3D.SetRenderState(RenderState.Lighting, true).Assert();
            device3D.SetRenderState(RenderState.ZEnable, true).Assert();
            device3D.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Linear);
            device3D.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Linear);
            device3D.SetRenderState(RenderState.ZFunc, Compare.LessEqual);
            device3D.SetRenderState(RenderState.ZWriteEnable, true);

            device3D.SetTransform(TransformState.World, WorldTransform.ToSlimDXMatrix()).Assert();

            foreach (var material in mesh.GetMaterials().Select((m, index) => new { Material = m, index }))
            {
                if (!string.IsNullOrEmpty(material.Material.TextureFileName))
                {
                    device3D.SetTexture(0, textureProvider.GetTexture(material.Material.TextureFileName).Texture).Assert();
                    device3D.Material = material.Material.MaterialD3D;
                    mesh.DrawSubset(material.index).Assert();
                    device3D.SetTexture(0, null).Assert();
                }else
                {
                    device3D.SetTexture(0, null).Assert();
                    device3D.Material = material.Material.MaterialD3D;
                    mesh.DrawSubset(material.index).Assert();
                }
            }
        }
    }
}
