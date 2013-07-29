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
        readonly Mesh mesh;
        public Mesh Mesh
        {
            get { return mesh; }
        }

        public SlimObject()
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        Base.Matrix4x4 worldTransform = new Base.Matrix4x4();
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
