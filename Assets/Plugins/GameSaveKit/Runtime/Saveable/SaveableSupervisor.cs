using System;
using System.Collections.Generic;
using System.Linq;
using GameDevUtils.Runtime;
using GameSaveKit.Runtime.Prefs;
using GameSaveKit.Runtime.Settings;
using UnityEngine;

namespace GameSaveKit.Runtime.Saveable
{
    public sealed class SaveableSupervisor : MonoBehaviour
    {
        static SaveableSupervisor instance { get; set; }
        public static bool Exist() => instance != null;
        
        readonly HashSet<object> saveableBehaviours = new HashSet<object>();
        
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("[Save-Load Tool] SaveableSupervisor already exist on scene.");
                
                ObjectHelper.Destroy(this.gameObject);
                return;
            }
            
            instance = this;
            ObjectHelper.DontDestroyOnLoad(this.gameObject);
        }
        
        private void OnDestroy()
        {
            instance = null;
        }
        
        public static void AddBehaviour<T>(ISaveableBehaviour<T> behaviour) where T : GamePrefs, new()
        {
            if (!Exist())
                throw new NonExistedException();
            
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
            
            instance.saveableBehaviours.Remove(behaviour);
        }
        
        public static HashSet<ISaveableBehaviour<T>> GetSaveableBehaviours<T>() where T : GamePrefs, new()
        {
            if (!Exist())
                throw new NonExistedException();
            
            return instance.saveableBehaviours.OfType<ISaveableBehaviour<T>>().ToHashSet();
        }

        public class NonExistedException : Exception
        {
            public override string Message => "[Save-Load Tool] SaveableSupervisor haven't exist on scene.";
        }
    }
}