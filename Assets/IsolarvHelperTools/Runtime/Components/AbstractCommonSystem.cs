using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IsolarvHelperTools.Runtime
{
    [Serializable]
    public enum EUpdateType
    {
        Delay, Yield, NextFrame, WaitForEndOfFrame
    }
    
    [Serializable]
    public enum EDeltaType
    {
        Delta, Fixed, Manual
    }
    
    public abstract class AbstractCommonSystem : MonoBehaviour
    {
        [SerializeField] private EUpdateType updateType = EUpdateType.Delay;
        
        [DrawIf("updateType", EUpdateType.Yield)]
        [SerializeField] private PlayerLoopTiming yieldTiming = PlayerLoopTiming.Update;
        
        [DrawIf("updateType", EUpdateType.Delay)]
        [SerializeField] private EDeltaType deltaType = EDeltaType.Delta;
        
        [DrawIf("updateType", EUpdateType.Delay)]
        [DrawIf("deltaType", EDeltaType.Manual)]
        [SerializeField] private float manualDelta = 0.1f;
        
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
        
        void StartSystem()
        {
            OnStart();
            
            AsyncTaskHelper.CreateTask(async () =>
            {
                while (true)
                {
                    float systemDelta = DeltaTime;
                    
                    await UpdateSystem(systemDelta);
                    await AwaitTask(systemDelta);
                }
            });
        }

        protected virtual void OnStart()
        {
            
        }
        
        protected virtual void OnDestroy()
        {
            
        }

        async UniTask AwaitTask(float deltaTime)
        {
            if (updateType == EUpdateType.Delay)
            {
                int systemDeltaInMilliseconds = (int)(deltaTime * 1000);
                await UniTask.Delay(systemDeltaInMilliseconds, cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            else if (updateType == EUpdateType.Yield)
            {
                await UniTask.Yield(yieldTiming, cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            else if (updateType == EUpdateType.NextFrame)
            {
                await UniTask.NextFrame(cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            else if (updateType == EUpdateType.WaitForEndOfFrame)
            {
                await UniTask.WaitForEndOfFrame(cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected abstract UniTask UpdateSystem(float deltaTime);
    }
}