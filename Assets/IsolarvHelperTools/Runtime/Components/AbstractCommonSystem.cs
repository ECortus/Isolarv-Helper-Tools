using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IsolarvHelperTools.Runtime
{
    [Serializable]
    public enum EDeltaType
    {
        Delta, Fixed, Manual
    }
    
    public abstract class AbstractCommonSystem : MonoBehaviour
    {
        [SerializeField] private EDeltaType deltaType = EDeltaType.Delta;
        
        [DrawIf("deltaType", EDeltaType.Manual)]
        [SerializeField] private float manualDelta = 0.1f;
        
        [SerializeField] private bool enableTier = false;
        
        [DrawIf("enableTier", true)]
        [SerializeField] private int tierMaxIterationPerFrame = 10;
        
        protected float DeltaTime
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
        
        protected virtual void StartSystem()
        {
            AsyncTaskHelper.CreateTask(async () =>
            {
                while (true)
                {
                    await UpdateSystem();
                }
            });
        }

        protected abstract UniTask UpdateSystem();
        
        int componentTier = 0;

        protected async UniTask UpdateTier()
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

        protected void ResetTier()
        {
            componentTier = 0;
        }
    }
}