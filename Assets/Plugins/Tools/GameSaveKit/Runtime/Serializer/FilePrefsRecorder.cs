using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GameSaveKit.Runtime.Prefs;
using GameSaveKit.Runtime.Settings;
using UnityEngine;

namespace GameSaveKit.Runtime.Serializer
{
    [Serializable]
    internal sealed class FilePrefsRecorder
    {
        const string UserFileName = "user";
        static string ApplicationVersion => Application.version;
        
        public static void WriteRecord<T>(int id, T prefs) where T : GamePrefs, new()
        {
            var saveFilePath = SaveKitPathSettings.GetDirectory();
            if (!Directory.Exists(saveFilePath))
            {
                if (saveFilePath == null)
                    throw new ArgumentNullException(nameof(saveFilePath));
                
                Directory.CreateDirectory(saveFilePath);
            }

            var fileName = UserFileName + id;
            if (CheckFileIsOld(prefs))
            {
                WriteOldFile(prefs);
            }

            WriteAdditionalData(ref prefs);

            WriteFile();
            return;

            // local functions
            void WriteAdditionalData(ref T dataPrefs)
            {
                dataPrefs.version = ApplicationVersion;
            }

            void WriteFile()
            {
                switch (SaveKitParametersSettings.SavingType)
                {
                    case SaveKitParametersSettings.ESavingType.Json:
                        WriteOnJson();
                        break;
                    case SaveKitParametersSettings.ESavingType.Binary:
                        WriteOnBinary();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            
            void WriteOnJson()
            {
                saveFilePath = SaveKitPathSettings.GetOrCreateFilePath(fileName, "json");
                
                if (File.Exists(saveFilePath))
                {
                    WriteBackup(saveFilePath);
                }
                
                var json = JsonUtility.ToJson(prefs);
                File.WriteAllText(saveFilePath, json);
            }

            void WriteOnBinary()
            {
                saveFilePath = SaveKitPathSettings.GetOrCreateFilePath(fileName, "dat");
                
                if (File.Exists(saveFilePath))
                {
                    WriteBackup(saveFilePath);
                }
                
                var binaryFormatter = new BinaryFormatter();
                
                var fileStream = File.Create(saveFilePath);
                binaryFormatter.Serialize(fileStream, prefs);
                
                fileStream.Close();
            }
            
            void WriteBackup(string path)
            {
                var backupPath = path + ".bak";
                File.Copy(path, backupPath, true);
            }

            void WriteOldFile(T oldPrefs)
            {
                var oldFileName = fileName;
                fileName += $"_{oldPrefs.version}";
                
                WriteFile();
                
                fileName = oldFileName;
            }
        }
        
        public static T ReadRecord<T>(int id) where T : GamePrefs, new()
        {
            var saveFilePath = SaveKitPathSettings.GetDirectory();
            if (!Directory.Exists(saveFilePath))
            {
                Debug.LogWarning("[Save-Load Tool] Save file does not exist");
                return new T();
            }

            T prefs;

            var fileName = UserFileName + id;
            switch (SaveKitParametersSettings.SavingType)
            {
                case SaveKitParametersSettings.ESavingType.Json:
                    prefs = ReadOnJson();
                    break;
                case SaveKitParametersSettings.ESavingType.Binary:
                    prefs = ReadOnBinary();
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            if (CheckFileIsOld(prefs))
            {
                WriteRecord(id, prefs);
            }

            return prefs;
            
            // local functions
            T ReadOnJson()
            {
                saveFilePath = SaveKitPathSettings.GetOrCreateFilePath(fileName, "json");
                
                if (!File.Exists(saveFilePath))
                {
                    Debug.LogWarning("[Save-Load Tool] Save file does not exist");
                    return new T();
                }
                
                var json = File.ReadAllText(saveFilePath);
                return JsonUtility.FromJson<T>(json);
            }
            
            T ReadOnBinary()
            {
                saveFilePath = SaveKitPathSettings.GetOrCreateFilePath(fileName, "dat");
                
                if (!File.Exists(saveFilePath))
                {
                    Debug.LogWarning("[Save-Load Tool] Save file does not exist");
                    return new T();
                }
                
                var binaryFormatter = new BinaryFormatter();
                
                var fileStream = File.OpenRead(saveFilePath);
                var binaryPrefs = (T)binaryFormatter.Deserialize(fileStream);
                
                fileStream.Close();
                return binaryPrefs;
            }
        }
        
        static bool CheckFileIsOld<T>(T prefs) where T : GamePrefs, new()
        {
            var versionIsOlder = prefs.version != ApplicationVersion;
            return versionIsOlder;
        }
    }
}