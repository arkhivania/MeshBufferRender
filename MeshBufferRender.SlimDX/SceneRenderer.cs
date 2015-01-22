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
    public class SceneRenderer : MeshBufferRender.Base.ISceneRenderer
    {
        private readonly Device device;

        public SceneRenderer(IDevice device)
        {
            this.device = (Device)device;
        }

        public void Render(Camera camera, IScene scene, IRenderSurface renderSurface)
        {
            var device3d = device.D3DDevice;

            var slimRenderSurface = (RenderSurface)renderSurface;

            device3d.DepthStencilSurface = slimRenderSurface.DepthBuffer;
            device3d.SetRenderTarget(0, slimRenderSurface.Surface).Assert();

            device3d.Clear(ClearFlags.ZBuffer | ClearFlags.Target, scene.Color, 1.0f, 0).Assert();

            float zNear = 1.0f;
            if (camera.CameraNear != null)
                zNear = camera.CameraNear;

            var projection = Matrix.PerspectiveFovLH(camera.ViewAngle,
                (float)renderSurface.Width / renderSurface.Height, zNear, (float)(camera.Target - camera.Position).Length * 2.0f);

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

            device3d.BeginScene().Assert();

            using(var objectsScope = scene.GetScope())
            foreach (var obj in objectsScope.MeshObjects.OfType<Base.ISlimDrawObject>())
                obj.Draw(device);

            device3d.EndScene().Assert();

            device3d.StretchRectangle(slimRenderSurface.Surface, slimRenderSurface.OffscreenDownsampledRenderTarget, TextureFilter.Linear).Assert();
            device3d.GetRenderTargetData(slimRenderSurface.OffscreenDownsampledRenderTarget, slimRenderSurface.OffscreenSurface).Assert();

            //foreach (var i in Enumerable.Range(0, scene.Lights.Count()))
            //    device3d.EnableLight(i, false);
        }
    }
}
