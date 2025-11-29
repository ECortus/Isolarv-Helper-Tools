using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameDevUtils.Editor
{
    public struct SavePanelInfoEditor
    {
        public bool IsValid;
        public string Path;
        public string Name;
    }

    public static class ActorDataScriptableObjectHelper
    {
        public static void OnGUIFoldout(ref Dictionary<Type, bool> foldoutByType, ref UnityEditor.Editor editor, Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
            
            if (property.objectReferenceValue == null)
                return;

            var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var foldout = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);
            if (foldout != property.isExpanded)
            {
                property.isExpanded = foldout;
            }
            
            if (property.objectReferenceValue != null)
            {
                // Store foldout values in a dictionary per object type
                bool foldoutExists = foldoutByType.TryGetValue(property.objectReferenceValue.GetType(), out foldout);
                if (foldoutExists)
                    foldoutByType[property.objectReferenceValue.GetType()] = foldout;
                else
                    foldoutByType.Add(property.objectReferenceValue.GetType(), foldout);
            }
            
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                
                if (!editor)
                    UnityEditor.Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);
                
                EditorGUI.BeginChangeCheck();
                if (editor)
                    editor.OnInspectorGUI();
                
                if (EditorGUI.EndChangeCheck())
                    property.serializedObject.ApplyModifiedProperties();
                
                EditorGUI.indentLevel--;
            }
        }
    }

    public static class ActorAssetButtonsEditorDrawer
    {
        delegate Object ButtonMethod(SavePanelInfoEditor panelInfo, SerializedProperty property, ActorAssetEditorHelper.InitFunction init);
        
        public static Object DrawAddDataButton(string buttonTitle, string fieldName, string dataName, 
            SerializedProperty property, string prefabPath, ActorAssetEditorHelper.InitFunction init)
        {
            ButtonMethod method = new ButtonMethod(ActorAssetEditorHelper.CreateNewScriptableObject);
            return DrawButton(buttonTitle, fieldName, dataName, property, method, init, prefabPath);
        }

        public static Object DrawDuplicateDataButton(string buttonTitle, string fieldName, string dataName, 
            SerializedProperty property, string dataPath, ActorAssetEditorHelper.InitFunction init)
        {
            ButtonMethod method = new ButtonMethod(ActorAssetEditorHelper.DuplicateExistedScriptableObject);
            return DrawButton(buttonTitle, fieldName, dataName, property, method, init, dataPath);
        }
        
        public static Object DrawDuplicatePrefabAndDataButton(string buttonTitle, string fieldName, 
            string dataName, SerializedProperty property, string prefabPath, ActorAssetEditorHelper.InitFunction init)
        {
            ButtonMethod method = new ButtonMethod(ActorAssetEditorHelper.DuplicatePrefabAndDataScriptableObject);
            return DrawButton(buttonTitle, fieldName, dataName, property, method, init, prefabPath, "prefab");
        }

        static Object DrawButton(string buttonTitle, string fieldName, string dataName, 
            SerializedProperty property, ButtonMethod requireMethod, ActorAssetEditorHelper.InitFunction init, 
            string customPath, string extension = "asset")
        {
            bool button = GUILayout.Button(buttonTitle);
                
            if (fieldName.Length == 0)
            {
                return null;
            }

            Object asset = null;
                
            if (button)
            {
                string openPath = customPath;
                SavePanelInfoEditor panelInfo = ActorAssetEditorHelper.GetSavePanelInfoWithoutPanel(openPath, dataName, extension);

                asset = requireMethod(panelInfo, property, init);
                    
                GUIUtility.ExitGUI();
            }

            return asset;
        }
    }
    
    public static class ActorAssetEditorHelper
    {
        // Init function (placed inside methods for correct work)
        public delegate void InitFunction(string path);
        
        public static Object CreateNewScriptableObject(SavePanelInfoEditor panelInfo, SerializedProperty property, InitFunction init)
        {
            if (!panelInfo.IsValid) return null;
            
            Object asset = CreateNewInstanceScriptableObject(panelInfo, property);
            
            asset.name = panelInfo.Name;
            
            if (init != null) init(panelInfo.Path);
            
            property.objectReferenceValue = asset;
            
            AfterCreateCopyOperation(property, asset);
            
            return asset;
        }
        
        public static Object DuplicateExistedScriptableObject(SavePanelInfoEditor panelInfo, SerializedProperty property, InitFunction init)
        {
            if (!panelInfo.IsValid) return null;
            
            Object asset = CreateNewInstanceScriptableObject(panelInfo, property);
            
            EditorUtility.CopySerialized(property.objectReferenceValue, asset);
            
            asset.name = panelInfo.Name;
            
            if (init != null) init(panelInfo.Path);
            
            property.objectReferenceValue = asset;
            
            AfterCreateCopyOperation(property, asset);
            
            return asset;
        }
        
        public static Object DuplicatePrefabAndDataScriptableObject(SavePanelInfoEditor panelInfo, SerializedProperty property, InitFunction init)
        {
            if (!panelInfo.IsValid) return null;

            string path = panelInfo.Path;
            CreateNewPrefab(path, property);
            
            // Create new serialized object/property of new script on new prefab
            Type type = property.serializedObject.targetObject.GetType();
            string propertyName = property.name;
            
            Object newObjectScript = AssetDatabase.LoadAssetAtPath(path, type);
            SerializedObject newSerialized = new SerializedObject(newObjectScript);
            SerializedProperty newProperty = newSerialized.FindProperty(propertyName);
            
            Object oldReference = property.objectReferenceValue;
            
            // Create new asset info
            string openObjectPath = AssetDatabase.GetAssetPath(oldReference);
            string assetName = newSerialized.targetObject.name + " Data";
            
            SavePanelInfoEditor newScriptableObjectInfo = GetSavePanelInfoWithoutPanel(openObjectPath, assetName, "asset");
                        
            if (!newScriptableObjectInfo.IsValid) return null;
            
            // Create new asset
            Object newAsset = CreateNewInstanceScriptableObject(newScriptableObjectInfo, property);
            
            // Copy old asset if present
            if (oldReference)
            {
                string newPath = newScriptableObjectInfo.Path;
                newAsset = AssetDatabase.LoadAssetAtPath(newPath, typeof(Object));
                
                EditorUtility.CopySerialized(oldReference, newAsset);
                
                newAsset.name = newScriptableObjectInfo.Name;
                
                if (init != null) init(newPath);
                
                newProperty.objectReferenceValue = newAsset;
            }
            else
            {
                newProperty.objectReferenceValue = null;
            }

            AfterCreateCopyOperation(newProperty, newObjectScript);

            return newAsset;
        }

        static void CreateNewPrefab(string path, SerializedProperty property)
        {
            string toClonePath = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
            AssetDatabase.CopyAsset(toClonePath, path);
        }

        static Object CreateNewInstanceScriptableObject(SavePanelInfoEditor panelInfo, SerializedProperty property)
        {
            Type type = GetPropertyFieldType(property);
            string path = panelInfo.Path;

            Object asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        /// <param name="property">Property to applyModifiedProperties.</param>
        /// <param name="toSelect">Object to select after apply and save assets.</param>
        static void AfterCreateCopyOperation(SerializedProperty property, Object toSelect)
        {
            property.serializedObject.ApplyModifiedProperties();
            
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
                        
            if (toSelect) Selection.activeObject = toSelect;
        }
        
        public static Type GetPropertyFieldType(SerializedProperty property)
        {
            if (property == null) return null;
            
            Type parentType = property.serializedObject.targetObject.GetType();
            string propertyPath = property.propertyPath;

            FieldInfo fi = null;

            if (propertyPath.Contains("."))
            {
                FieldInfo[] objField = parentType.GetFields();

                char[] chars = { '.', ',' };
                string str = propertyPath.Substring(0,propertyPath.IndexOfAny(chars));
                string field = propertyPath.Substring(propertyPath.IndexOfAny(chars) + 1);
            
                foreach(var obj in objField)
                {
                    if (obj.Name == str)
                    {
                        fi = parentType.GetField(obj.Name).FieldType.GetField(field);
                    }
                }
            }
            else
            {
                fi = parentType.GetField(propertyPath);
            }

            if (fi == null) return null;
            return fi.FieldType;
        }
        
        public static SavePanelInfoEditor OpenSaveFilePanel(string title, string filePath, string assetName, string extension = "")
        {
            string ext = extension == "" ? Path.GetExtension(filePath).Replace(".", "") : extension;
            
            var path = EditorUtility.SaveFilePanel(
                title,
                Path.GetDirectoryName(filePath),
                assetName + $".{ext}",
                ext);

            return GetSavePanelInfo(path);
        }
        
        public static SavePanelInfoEditor GetSavePanelInfoWithoutPanel(string filePath, string assetName, string extension = "")
        {
            string ext = extension == "" ? Path.GetExtension(filePath).Replace(".", "") : extension;

            var main = Application.dataPath.Replace("Assets", "");
            var path = main 
                       + (Path.GetDirectoryName(filePath) + "\\" 
                                                          + assetName + $".{ext}").Replace("\\", "/");
            
            return GetSavePanelInfo(path);
        }

        static SavePanelInfoEditor GetSavePanelInfo(string path)
        {
            // Debug.Log(path);
            if (path.Length == 0) return new SavePanelInfoEditor { IsValid = false, Path = "" };

            string directory = Path.GetDirectoryName(path);
            if (directory == null) return new SavePanelInfoEditor();
            
            directory = directory.Replace("\\", "/");
            directory = directory.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                    
            path = path.Replace(Application.dataPath, "Assets");

            return new SavePanelInfoEditor
            {
                IsValid = AssetDatabase.IsValidFolder(directory), 
                Path = path,
                Name = path
                    .Replace(directory, "")
                    .Replace("/" , "")
                    .Replace(".asset", "")
            };
        }
    }
}