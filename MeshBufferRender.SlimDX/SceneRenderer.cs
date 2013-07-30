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
            device3d.SetRenderTarget(0, slimRenderSurface.Surface).Assert();

            device3d.Clear(ClearFlags.ZBuffer | ClearFlags.Target, scene.Color, (float)(camera.Target - camera.Position).Length * 2.0f, 0).Assert();

            var projection = Matrix.PerspectiveFovLH(camera.ViewAngle,
                (float)renderSurface.Width / renderSurface.Height, 1, (float)(camera.Target - camera.Position).Length * 2.0f);

            var view = Matrix.LookAtLH(camera.Position.ToSlimDXVector(), camera.Target.ToSlimDXVector(), camera.CameraUp.ToSlimDXVector());

            foreach (var l in scene.Lights.Select((l, index) => new { Light = l, Index = index }))
            {
                device3d.SetLight(l.Index, new Direct3D.Light
                {
                    Type = LightType.Point,
                    Position = l.Light.Position.ToSlimDXVector(),
                    Diffuse = l.Light.Diffuse,
                    Range = l.Light.Range,
                    Attenuation0 = l.Light.Attenuation0, 
                    Ambient = l.Light.Ambient
                }).Assert();

                device3d.EnableLight(l.Index, true).Assert();
            }

            device3d.SetTransform(TransformState.Projection, projection).Assert();
            device3d.SetTransform(TransformState.View, view).Assert();

            using(var objectsScope = scene.GetScope())
            foreach (var obj in objectsScope.MeshObjects.OfType<SlimObject>())
            {
                device3d.SetRenderState(RenderState.ShadeMode, ShadeMode.Gouraud).Assert();
                device3d.SetRenderState(RenderState.CullMode, Cull.Clockwise).Assert();
                device3d.SetRenderState(RenderState.Lighting, true).Assert();
                device3d.SetRenderState(RenderState.ZEnable, true).Assert();
                
                device3d.SetTransform(TransformState.World, obj.WorldTransform.ToSlimDXMatrix()).Assert();

                device3d.BeginScene().Assert();

                foreach (var material in obj.Mesh.GetMaterials().Select((m, index) => new { Material = m, index }))
                {
                    if(!string.IsNullOrEmpty(material.Material.TextureFileName))
                    {
                        device3d.SetTexture(0, obj.TextureProvider.GetTexture(material.Material.TextureFileName).Texture).Assert();
                        device3d.Material = material.Material.MaterialD3D;
                        obj.Mesh.DrawSubset(material.index).Assert();

                        device3d.SetTexture(0, null).Assert();
                    }
                }
                device3d.EndScene().Assert();

                device3d.StretchRectangle(slimRenderSurface.Surface, slimRenderSurface.OffscreenDownsampledRenderTarget, TextureFilter.None).Assert();
                device3d.GetRenderTargetData(slimRenderSurface.OffscreenDownsampledRenderTarget, slimRenderSurface.OffscreenSurface).Assert();
            }

            //foreach (var i in Enumerable.Range(0, scene.Lights.Count()))
            //    device3d.EnableLight(i, false);
        }
    }
}
