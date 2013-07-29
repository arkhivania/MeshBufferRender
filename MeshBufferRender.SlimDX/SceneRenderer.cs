using MeshBufferRender.Base;
using SlimDX;
using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Direct = SlimDX;
using Direct3D = SlimDX.Direct3D9;

namespace MeshBufferRender.SlimDX
{
    class SceneRenderer : Base.ISceneRenderer
    {
        private readonly Device device;

        public SceneRenderer(Device device)
        {
            this.device = device;
        }

        public void Render(Camera camera, IScene scene, IRenderSurface renderSurface)
        {
            var device3d = device.D3DDevice;

            device3d.Clear(ClearFlags.ZBuffer | ClearFlags.Target, Color.Black, 10000.0f, 0);

            var projection = Matrix.PerspectiveFovLH((float)System.Math.PI / 4,
                (float)renderSurface.Width / renderSurface.Height, 1, (float)(camera.Target - camera.Position).Length * 2.0f);

            var view = Matrix.LookAtLH(camera.Position.ToSlimDXVector(), camera.Target.ToSlimDXVector(), camera.CameraUp.ToSlimDXVector());

            device3d.SetLight(0, new Light
            {
                Type = LightType.Point,
                Ambient = Color.Gray,
                Position = new Direct.Vector3(0, 0, -100f),
                Diffuse = Color.White,
                Range = 200f,
                Attenuation0 = 0.2f
            });

            device3d.EnableLight(0, true);

            foreach (var obj in scene.MeshObjects.OfType<SlimObject>())
            {

                device3d.SetRenderState(RenderState.CullMode, Cull.Clockwise);
                device3d.SetRenderState(RenderState.Lighting, true);
                device3d.SetRenderState(RenderState.ZEnable, true);

                device3d.SetTransform(TransformState.Projection, projection);
                device3d.SetTransform(TransformState.View, view);

                device3d.SetTransform(TransformState.World, obj.WorldTransform.ToSlimDXMatrix());

                device3d.BeginScene();

                foreach (var material in obj.Mesh.GetMaterials().Select((m, index) => new { Material = m, index }))
                {
                    device3d.Material = material.Material.MaterialD3D;
                    //device3d.SetTexture(0, manTexture);
                    obj.Mesh.DrawSubset(material.index);
                }
                device3d.EndScene();

                device3d.Present(Present.None);
            }
        }
    }
}
