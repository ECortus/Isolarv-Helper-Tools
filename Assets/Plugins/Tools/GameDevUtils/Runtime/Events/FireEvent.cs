using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevUtils.Runtime
{
    public class FireEvent
    {
        readonly List<Action> _actions = new List<Action>();

        public void AddListener(Action action)
        {
            if (Invoked)
            {
                action();
            }
            else
            {
                _actions.Add(action);
            }
        }

        public void Invoke()
        {
            if (!Invoked)
            {
                Invoked = true;

                int initialCount = _actions.Count;

                SafeRemovalForIteration();

                if (initialCount != _actions.Count)
                {
                    DebugHelper.LogError("Fire event actions changed during performance");
                }
            }
        }

        void SafeRemovalForIteration()
        {
            _actions.Reverse();
            for (int i = _actions.Count - 1; i >= 0; i--)
            {
                try
                {
                    _actions[i]();
                }
                catch (Exception e)
                {
                    DebugHelper.LogError($"An error occurred while executing action at index {i}: {e}");
                }
            }
        }

        public void RemoveListener(Action action)
        {
            if (_actions.Count > 0)
            {
                _actions.Remove(action);
            }
        }

        public void RemoveAllListeners()
        {
            _actions.Clear();
        }

        bool Invoked { get; set; }

        public void Reset()
        {
            Invoked = false;
            _actions.Clear();
        }
    }
}