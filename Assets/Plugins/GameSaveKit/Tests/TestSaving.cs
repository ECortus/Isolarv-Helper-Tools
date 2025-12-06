using GameSaveKit.Runtime;
using GameSaveKit.Runtime.Saveable;
using GameSaveKit.Runtime.Prefs;
using UnityEngine;

namespace GameSaveKit.Tests
{
    public sealed class TestSaving : MonoBehaviour, ISaveableBehaviour<TestPrefs>
    {
        public float testValue = 5;

        void Start()
        {
            SaveableSupervisor.AddBehaviour(this);
            SaveablePrefs.LoadAll<TestPrefs>();
        }
        
        void OnDestroy()
        {
            SaveableSupervisor.RemoveBehaviour(this);
        }

        public void Serialize(ref TestPrefs record)
        {
            record.testValue = testValue;
        }
        
        public void Deserialize(TestPrefs record)
        {
            testValue = record.testValue;
        }
    }
}