using SlimDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshBufferRender.SlimDX
{
    class SlimProjectionBuilder : MeshBufferRender.Base.IProjectionBuilder
    {
        public MeshBufferRender.Base.Matrix4x4 BuildProjectionMatrix(MeshBufferRender.Base.Camera camera, MeshBufferRender.Base.IRenderSurface renderSurface)
        {
            float zNear = 1.0f;
            if (camera.CameraNear != null)
                zNear = camera.CameraNear;
            var projection = Matrix.PerspectiveFovLH(camera.ViewAngle,
                (float)renderSurface.Width / renderSurface.Height, zNear, (float)(camera.Target - camera.Position).Length * 2.0f);
            return projection.ToMatrix4x4();
        }
    }
}
