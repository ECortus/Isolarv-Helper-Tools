using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IsolarvHelperTools
{
    public static class UnitaskHelper
    {
        public static void Create(Func<UniTask> factory)
        {
            UniTask.Create(async () =>
            {
                try
                {
                    await factory();
                }
                catch (OperationCanceledException e)
                {
                    Debug.LogWarning(e);
                }
                catch (ObjectDisposedException e)
                {
                    Debug.LogWarning(e);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            });
        }
    }
}