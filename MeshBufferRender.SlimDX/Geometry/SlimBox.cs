using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.SlimDX.Geometry
{
    public class SlimBox : SlimMeshBase
    {
        public SlimBox(Device device, 
            ITextureProvider textureProvider,
            float width, float height, float depth, ExtendedMaterial material)
            :base(device, textureProvider, material, 
            () => global::SlimDX.Direct3D9.Mesh.CreateBox(device.D3DDevice, width, height, depth))
        {
        }
    }
}
