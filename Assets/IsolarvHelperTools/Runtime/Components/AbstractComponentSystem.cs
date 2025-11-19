using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IsolarvHelperTools.Runtime
{
    public interface ISystemComponent
    {
        public bool IsDisabled { get; }
    }
    
    public abstract class AbstractComponentSystem<T> : AbstractCommonSystem 
        where T : ISystemComponent
    {
        List<T> allElements;
        
        void OnDestroy()
        {
            if (allElements != null)
            {
                allElements.Clear();
            }
        }
        
        protected override async UniTask UpdateSystem()
        {
            allElements = GetUnitList();

            ResetTier();

            float systemDelta = DeltaTime;
            int systemDeltaInMilliseconds = (int)(systemDelta * 1000);
                
            for (int i = 0; i < allElements.Count; i++)
            {
                var component = allElements[i];
                if (component == null || component.IsDisabled)
                {
                    continue;
                }

                var result = UpdateFunction(component, systemDelta);
                if (result)
                {
                    await UpdateTier();
                }
            }

            await UniTask.Delay(systemDeltaInMilliseconds, cancellationToken: this.GetCancellationTokenOnDestroy());
        }

        protected abstract List<T> GetUnitList();
        protected abstract bool UpdateFunction(T unit, float delta);
    }
}