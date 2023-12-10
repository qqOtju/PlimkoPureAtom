using UnityEngine;

namespace MyAssets.Scripts.DesignPatterns.ObjPool
{
    public class ParticleSystemPool
    {
        protected readonly Transform Container;
        protected readonly ParticleSystem Obj;
        
        private ObjectPool<ParticleSystem> ObjPool { get; }
        
        public ParticleSystemPool(ParticleSystem obj, Transform container)
        {
            this.Obj = obj;
            this.Container = container;
            ObjPool = new ObjectPool<ParticleSystem>(CreateFunc, ActionOnGet, ActionOnRelease);
        }

        public ParticleSystem Get() => ObjPool.Get();
        
        public void Release(ParticleSystem obj) => ObjPool.Release(obj);
        
        public void Initialize(int size) => ObjPool.InitializePool(size);

        private ParticleSystem CreateFunc()
        {
            var obj = Object.Instantiate(Obj, Container);
            obj.gameObject.SetActive(true);
            return obj;
        }

        private void ActionOnGet(ParticleSystem obj) =>
            obj.gameObject.SetActive(true);

        private void ActionOnRelease(ParticleSystem obj) =>
            obj.gameObject.SetActive(false);
    }
}