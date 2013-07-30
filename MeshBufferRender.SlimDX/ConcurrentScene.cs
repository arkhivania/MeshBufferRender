using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using MeshBufferRender.Base;

namespace MeshBufferRender.SlimDX
{
    public class ConcurrentScene : IScene
    {
        readonly List<IMeshObject> meshObjects = new List<IMeshObject>();

        public IEnumerable<IMeshObject> MeshObjects
        {
            get
            {
                return meshObjects;
            }
        }

        readonly object monitorObject = new object();

        class Scope : ISceneObjectsScope, IDisposable
        {
            private readonly ConcurrentScene scene;

            public Scope(ConcurrentScene scene)
            {
                this.scene = scene;
                Monitor.Enter(scene.monitorObject);
            }

            public IEnumerable<IMeshObject> MeshObjects
            {
                get
                {
                    return scene.MeshObjects;
                }
            }

            public void Dispose()
            {
                Monitor.Exit(scene.monitorObject);
            }
        }

        public ISceneObjectsScope GetScope()
        {
            return new Scope(this);
        }

        public void AddObject(IMeshObject meshObject)
        {
            lock (monitorObject)
                meshObjects.Add(meshObject);
        }

        public bool RemoveObject(IMeshObject meshObject)
        {
            lock (monitorObject)
                return meshObjects.Remove(meshObject);
        }

        public System.Drawing.Color Color { get; set; }

        public readonly ObservableCollection<Light> LightsCollection = new ObservableCollection<Light>();

        public IEnumerable<Light> Lights
        {
            get
            {
                return LightsCollection;
            }
        }
    }
}