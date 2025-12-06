using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using LocalizationModule.Runtime;
using UnityEngine;

namespace LocalizationModule.Editor
{
    internal static class EditorWindowUtils
    {
        public static T OpenWindow<T>(string label, params Type[] desiredDockNextTo) where T : EditorWindow
        {
            T wnd = UnityEditor.EditorWindow.GetWindow<T>(label, desiredDockNextTo);
            wnd.titleContent = EditorUtils.GetWindowTitle(label);
            
            var size = new Vector2(800, 600);
            wnd.minSize = size;

            if (desiredDockNextTo != null)
                SetDockerSize(size);

            return wnd;
        }

        static void SetDockerSize(Vector2 size)
        {
            var type = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.HostView");
            var fieldInfo = type.GetField("k_DockedMinSize", BindingFlags.Static | BindingFlags.NonPublic);

            fieldInfo!.SetValue(null, size);
        }

        public static void ShowWindow<T>(string label) where T : EditorWindow
        {
            T wnd = OpenWindow<T>(label);
            wnd.Show();
        }

        public static void FocusWindow<T>(string label) where T : EditorWindow
        {
            var wnd = OpenWindow<T>(label);
            wnd.Focus();
        }

        public static LanguageKeyCollection GetLanguages()
        {
            return AssetDatabase.LoadAssetAtPath<LanguageKeyCollection>(
                $"{EditorUtils.PACKAGE_BASE_PATH}/Data/Language Key Collection.asset");
        }
        
        public static void ValidateTable(TranslateTable table)
        {
            var dictionary = table.translation;
            var keysCollection = table.GetRelatedKeys();
                
            if (keysCollection.ScanForDuplicate())
            {
                keysCollection.RemoveDuplicates();
            }
            
            var keys = keysCollection.GetKeys();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary.Count; i++)
                {
                    var key = dictionary[i].GetKey();
                    if (keys.Contains(key))
                    {
                        keys.Remove(key);
                    }
                    else
                    {
                        dictionary.RemoveAt(i);
                        i--;
                    }
                }
            }

            if (keys.Count > 0)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    dictionary.Add(new TranslateTable.TranslationKey(keys[i], new TranslateInfo()));
                }
            }

            var languageKeys = GetLanguages();
            
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary.Count; i++)
                {
                    var textLanguages = new List<LanguageKey>();
                    textLanguages.AddRange(languageKeys.GetKeys());
                    
                    var info = dictionary[i].GetTranslateInfo();
                    for (int j = 0; j < info.text.Count; j++)
                    {
                        var text = info.text[j];
                        if (languageKeys.ContainKeyById(text.languageId))
                        {
                            textLanguages.Remove(languageKeys.GetKeyByID(text.languageId));
                        }
                        else
                        {
                            info.text.RemoveAt(j);
                            j--;
                        }
                    }
                    
                    if (textLanguages.Count > 0)
                    {
                        for (int j = 0; j < textLanguages.Count; j++)
                        {
                            info.text.Add(new TranslateInfo.TranslateInfoText(textLanguages[j].id, ""));
                            info.sprite.Add(new TranslateInfo.TranslateInfoSprite(textLanguages[j].id, null));
                            info.texture.Add(new TranslateInfo.TranslateInfoTexture(textLanguages[j].id, null));
                        }
                    }
                }
            }

            table.translation = dictionary;
            EditorUtility.SetDirty(table);
        }
    }
}