using System.Collections.Generic;
using GameSaveKit.Runtime.Prefs;
using GameSaveKit.Runtime.Saveable;

namespace GameSaveKit.Runtime.Serializer
{
    [System.Serializable]
    internal sealed class GameSerializer
    {
        public static void SerializeAll<T>(ref T record) where T : GamePrefs, new()
        {
            if (!SaveableSupervisor.Exist())
                throw new SaveableSupervisor.NonExistedException();
            
            HashSet<ISaveableBehaviour<T>> objectsToRecord = SaveableSupervisor.GetSaveableBehaviours<T>();
            foreach (var obj in objectsToRecord)
            {
                obj.Serialize(ref record);
            }
        }
        
        public static void DeserializeAll<T>(T record) where T : GamePrefs, new()
        {
            if (!SaveableSupervisor.Exist())
                throw new SaveableSupervisor.NonExistedException();
            
            HashSet<ISaveableBehaviour<T>> objectsToRecord = SaveableSupervisor.GetSaveableBehaviours<T>();
            foreach (var obj in objectsToRecord)
            {
                obj.Deserialize(record);
            }
        }
    }
}