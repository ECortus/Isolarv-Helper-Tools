using GameSaveKit.Runtime.Prefs;

namespace GameSaveKit.Runtime.Saveable
{
    public interface ISaveableBehaviour<T> where T : GamePrefs, new()
    {
        void Serialize(ref T record);
        void Deserialize(T record);
    }
}