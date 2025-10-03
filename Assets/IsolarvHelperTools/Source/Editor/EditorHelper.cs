﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace IsolarvHelperTools.Editor
{
    public interface IValidationObject
    {
        public void OnCreateValidate<T>(T oldObject) where T : ScriptableObject, IValidationObject;
        public void OnUpdateValidate<T>(T oldObject) where T : ScriptableObject, IValidationObject;
    }
    
    public static class EditorHelper
    {
        public static Object CreateOrGetNewScriptableAsset<T>(string newName, string path, 
            SerializedProperty newDataProperty, T oldData) where T : ScriptableObject, IValidationObject
        {
            var loaded = GetScriptableAsset<T>(newName, path);
            if (loaded != null)
            {
                return loaded;
            }
            
            return CreateNewScriptableAsset(newName, path, newDataProperty, oldData);
        }
        
        public static T GetScriptableAsset<T>(string newName, string path) where T : ScriptableObject, IValidationObject
        {
            if (!IsExistsDirectory(path, newName, out path))
            {
                return null;
            }

            var loadedData = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
            if (loadedData != null)
            {
                return loadedData;
            }
            
            return null;
        }
        
        public static Object CreateNewScriptableAsset<T>(string newName, string path, 
            SerializedProperty newDataProperty, T oldData) where T : ScriptableObject, IValidationObject
        {
            if (!IsExistsDirectory(path, newName, out path))
            {
                return null;
            }
            
            Type type = oldData.GetType();
            var asset = ScriptableObject.CreateInstance(type);
            
            AssetDatabase.CreateAsset(asset, path);
            EditorUtility.CopySerialized(oldData, asset);
            
            asset.name = newName;
            
            var data = asset as T;
            data?.OnCreateValidate(oldData);
            
            newDataProperty.objectReferenceValue = AssetDatabase.LoadAssetAtPath(path, typeof(T));
            
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            
            return data;
        }

        static bool IsExistsDirectory(string path, string newName, out string fullPath)
        {
            if (!Directory.Exists(Path.GetFullPath(path)))
            {
                DebugHelper.LogError($"Not found directory {path}");

                fullPath = "";
                return false;
            }
            
            path += $"/{newName}.asset";
            
            fullPath = path;
            return true;
        }
        
        public static bool IsArrayElement(SerializedProperty property)
        {
            return property.propertyPath.Contains("Array.data[");
        }
        
        public static int GetArrayIndex(SerializedProperty property)
        {
            Match match = Regex.Match(property.propertyPath, @"Array\.data\[(\d+)\]");
            return match.Success ? int.Parse(match.Groups[1].Value) : -1; // -1 if not an array element
        }
    }
}