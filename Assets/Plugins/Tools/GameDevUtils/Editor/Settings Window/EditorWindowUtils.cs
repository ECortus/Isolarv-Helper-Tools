using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using GameDevUtils.Editor;
using UnityEngine;

namespace GameSaveKit.Editor.CustomWindows
{
    internal static class EditorWindowUtils
    {
        public static T OpenWindow<T>(string label, params Type[] desiredDockNextTo) where T : EditorWindow
        {
            T wnd = UnityEditor.EditorWindow.GetWindow<T>(label, desiredDockNextTo);
            wnd.titleContent = DevUtilsEditorUtils.GetWindowTitle(label);
            
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
    }
}