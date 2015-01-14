using MeshBufferRender.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D9;
using System.Drawing;

namespace MeshBufferRender.SlimDX.Geometry
{
    public class SlimSphere : SlimMeshBase
    {   
        public SlimSphere(Device device, 
            ITextureProvider textureProvider,
            float radius, int slices, int stacks, ExtendedMaterial material)
            :base(device, textureProvider, material, 
            () => global::SlimDX.Direct3D9.Mesh.CreateSphere(device.D3DDevice, radius, slices, stacks))
        {
        }
    }
}
