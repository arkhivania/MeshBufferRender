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
    public class SceneRenderer : Base.ISceneRenderer
    {
        private readonly Device device;

        public SceneRenderer(Device device)
        {
            this.device = device;
        }

        public void Render(Camera camera, IScene scene, IRenderSurface renderSurface)
        {
            var device3d = device.D3DDevice;

            var slimRenderSurface = (RenderSurface)renderSurface;

            device3d.DepthStencilSurface = slimRenderSurface.DepthBuffer;
            device3d.SetRenderTarget(0, slimRenderSurface.Surface);

            device3d.Clear(ClearFlags.ZBuffer | ClearFlags.Target, scene.Color, (float)(camera.Target - camera.Position).Length * 2.0f, 0);

            var projection = Matrix.PerspectiveFovLH(camera.ViewAngle,
                (float)renderSurface.Width / renderSurface.Height, 1, (float)(camera.Target - camera.Position).Length * 2.0f);

            var view = Matrix.LookAtLH(camera.Position.ToSlimDXVector(), camera.Target.ToSlimDXVector(), camera.CameraUp.ToSlimDXVector());

            device3d.SetLight(0, new Light
            {
                Type = LightType.Point,
                Ambient = Color.LightGray,
                Position = new Direct.Vector3(0, 0, -100f),
                Diffuse = Color.White,
                Range = 200f,
                Attenuation0 = 0.1f
            });

            device3d.EnableLight(0, true);

            using(var objectsScope = scene.GetScope())
            foreach (var obj in objectsScope.MeshObjects.OfType<SlimObject>())
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
                    if(!string.IsNullOrEmpty(material.Material.TextureFileName))
                    {
                        device3d.SetTexture(0, obj.TextureProvider.GetTexture(material.Material.TextureFileName).Texture);
                        device3d.Material = material.Material.MaterialD3D;                    
                        obj.Mesh.DrawSubset(material.index);

                        device3d.SetTexture(0, null);
                    }
                }
                device3d.EndScene();
                device3d.GetRenderTargetData(slimRenderSurface.Surface, slimRenderSurface.OffscreenSurface);
            }
        }
    }
}
