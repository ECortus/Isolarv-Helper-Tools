using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IsolarvHelperTools
{
    public interface ISystemComponent
    {
        public bool IsDisabled { get; }
    }
    
    [Serializable]
    public enum EDeltaType
    {
        Delta, Fixed, Manual
    }
    
    public abstract class AbstractComponentSystem<T> : MonoBehaviour 
        where T : ISystemComponent
    {
        
        [SerializeField] private EDeltaType deltaType = EDeltaType.Delta;
        
        [DrawIf("deltaType", EDeltaType.Manual)]
        [SerializeField] private float manualDelta = 0.1f;
        
        [SerializeField] private bool enableTier = false;
        
        [DrawIf("enableTier", true)]
        [SerializeField] private int tierMaxIterationPerFrame = 10;
        
        List<T> allElements;
        
        float DeltaTime
        {
            get
            {
                float delta = 0;
                if (deltaType == EDeltaType.Delta)
                {
                    delta = Time.deltaTime;
                }
                else if (deltaType == EDeltaType.Fixed)
                {
                    delta = Time.fixedDeltaTime;
                }
                else if (deltaType == EDeltaType.Manual)
                {
                    delta = manualDelta;
                }
                else
                {
                    throw new NotImplementedException();
                }

                return delta;
            }
        }
        
        void Start()
        {
            StartSystem();
        }
        
        void OnDestroy()
        {
            if (allElements != null)
            {
                allElements.Clear();
            }
        }

        protected virtual void StartSystem()
        {
            AsyncTaskHelper.CreateTask(async () => await UpdateElementLogic());
        }
        
        async UniTask UpdateElementLogic()
        {
            while (true)
            {
                allElements = GetUnitList();

                int unitTier = 0;

                float systemDelta = DeltaTime;
                int systemDeltaInMilliseconds = (int)(systemDelta * 1000);
                
                for (int i = 0; i < allElements.Count; i++)
                {
                    var unit = allElements[i];
                    if (unit == null || unit.IsDisabled)
                    {
                        continue;
                    }

                    var result = UpdateFunction(unit, systemDelta);
                    if (enableTier && result)
                    {
                        unitTier++;
                        if (unitTier % tierMaxIterationPerFrame == 0)
                        {
                            unitTier = 0;
                            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken: this.GetCancellationTokenOnDestroy());
                        }
                    }
                }

                await UniTask.Delay(systemDeltaInMilliseconds, cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }

        protected abstract List<T> GetUnitList();
        protected abstract bool UpdateFunction(T unit, float delta);
    }
}