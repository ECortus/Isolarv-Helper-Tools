using System;
using System.Collections.Generic;
using System.Linq;
using GameDevUtils.Runtime;
using GameSaveKit.Runtime.Prefs;
using GameSaveKit.Runtime.Settings;
using UnityEngine;

namespace GameSaveKit.Runtime.Saveable
{
    public sealed class SaveableSupervisor : UnitySingleton<SaveableSupervisor>
    {
        readonly HashSet<object> saveableBehaviours = new HashSet<object>();
        
        public static bool Exist() => HasOrFindInstance;
        
        protected override void OnAwake()
        {
            ObjectHelper.DontDestroyOnLoad(this.gameObject);
        }
        
        public static void AddBehaviour<T>(ISaveableBehaviour<T> behaviour) where T : GamePrefs, new()
        {
            if (!Exist())
                throw new NonExistedException();
            
            var instance = GetInstance;
            instance.saveableBehaviours.Add(behaviour);

            if (GameSaveParametersSettings.LoadBehavioursOnAdding)
            {
                SaveablePrefs.LoadBehaviour<T>(behaviour);
            }
        }
        
        public static void RemoveBehaviour<T>(ISaveableBehaviour<T> behaviour) where T : GamePrefs, new()
        {
            if (!Exist())
                throw new NonExistedException();
            
            var instance = GetInstance;
            instance.saveableBehaviours.Remove(behaviour);
        }
        
        public static HashSet<ISaveableBehaviour<T>> GetSaveableBehaviours<T>() where T : GamePrefs, new()
        {
            if (!Exist())
                throw new NonExistedException();
            
            var instance = GetInstance;
            return instance.saveableBehaviours.OfType<ISaveableBehaviour<T>>().ToHashSet();
        }

        public class NonExistedException : Exception
        {
            public override string Message => "[Save-Load Tool] SaveableSupervisor haven't exist on scene.";
        }
    }
}