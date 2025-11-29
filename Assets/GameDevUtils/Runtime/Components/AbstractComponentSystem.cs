using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameDevUtils.Runtime
{
    public interface ISystemComponent
    {
        public bool IsDisabled { get; }
    }
    
    public abstract class AbstractComponentSystem<T> : AbstractCommonSystem 
        where T : ISystemComponent
    {
        [SerializeField] private bool enableTier = false;
        
        [DrawIf("enableTier", true)]
        [SerializeField] private int tierMaxIterationPerFrame = 10;
        
        List<T> allElements;
        int componentTier = 0;
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (allElements != null)
            {
                allElements.Clear();
            }
        }
        
        protected override async UniTask UpdateSystem(float deltaTime)
        {
            allElements = GetUnitList();

            ResetTier();
           
            for (int i = 0; i < allElements.Count; i++)
            {
                var component = allElements[i];
                if (component == null || component.IsDisabled)
                {
                    continue;
                }

                var result = UpdateFunction(component, deltaTime);
                if (result)
                {
                    await UpdateTier();
                }
            }
        }

        protected abstract List<T> GetUnitList();
        protected abstract bool UpdateFunction(T unit, float delta);

        async UniTask UpdateTier()
        {
            if (enableTier)
            {
                componentTier++;
                if (componentTier % tierMaxIterationPerFrame == 0)
                {
                    componentTier = 0;
                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken: this.GetCancellationTokenOnDestroy());
                }
            }
        }

        void ResetTier()
        {
            componentTier = 0;
        }
    }
}