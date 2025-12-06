using GameSaveKit.Runtime.Prefs;
using GameSaveKit.Runtime.Saveable;
using GameSaveKit.Runtime.Serializer;
using UnityEngine;

namespace GameSaveKit.Runtime
{
    [System.Serializable]
    public class SaveablePrefs
    {
        public static int SlotID
        {
            get => PlayerPrefs.GetInt("SlotID", 0);
            set => PlayerPrefs.SetInt("SlotID", value);
        }
        
        public static void Save<T>() where T : GamePrefs, new()
        {
            T prefs = FilePrefsRecorder.ReadRecord<T>(SlotID);
            GameSerializer.SerializeAll(ref prefs);
            
            FilePrefsRecorder.WriteRecord(SlotID, prefs);
        }
        
        public static void LoadAll<T>() where T : GamePrefs, new()
        {
            T prefs = FilePrefsRecorder.ReadRecord<T>(SlotID);
            GameSerializer.DeserializeAll(prefs);
        }
        
        public static void LoadBehaviour<T>(ISaveableBehaviour<T> behaviour) where T : GamePrefs, new()
        {
            T prefs = FilePrefsRecorder.ReadRecord<T>(SlotID);
            behaviour.Deserialize(prefs);
        }
    }
}