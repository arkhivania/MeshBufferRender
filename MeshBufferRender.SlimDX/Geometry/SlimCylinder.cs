using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D9;

namespace MeshBufferRender.SlimDX.Geometry
{
    public class SlimCylinder : SlimMeshBase
    {
        public SlimCylinder(Device device, 
            ITextureProvider textureProvider,
            float radius1, float radius2, float length,
            int slices, int stacks, ExtendedMaterial material)
            :base(device, textureProvider, material, 
            () => global::SlimDX.Direct3D9.Mesh.CreateCylinder(device.D3DDevice, radius1, radius2, length, slices, stacks))
        {
        }
    }
}
