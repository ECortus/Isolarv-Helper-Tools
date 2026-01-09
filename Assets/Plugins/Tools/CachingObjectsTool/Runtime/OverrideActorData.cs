using System;
using GameDevUtils.Runtime;
using UnityEngine;

namespace CachingObjectsTool.Runtime
{
    [Serializable]
    public abstract class OverrideActorData<T>
    {
        [SerializeField] private T defaultData;
        [SerializeField, Expandable(true)] private T newData;

        public T GetData() => newData;
    }
}