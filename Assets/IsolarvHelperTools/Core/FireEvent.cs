using System;
using System.Collections.Generic;
using UnityEngine;

namespace IsolarvHelperTools
{
    public class FireEvent
    {
        List<Action> _actions = new List<Action>();

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
                    Debug.LogError("Fire event actions changed during performance");
                }
            }
        }

        void SafeRemovalForIteration()
        {
            _actions.Reverse();

            // Iterate through the reversed list from the end, so its like from the beginning
            for (int i = _actions.Count - 1; i >= 0; i--)
            {
                try
                {
                    _actions[i]();
                }
                catch (Exception e)
                {
                    Debug.LogError($"An error occurred while executing action at index {i}: {e}");
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

        bool Invoked { get; set; }

        public void Reset()
        {
            Invoked = false;
            _actions.Clear();
        }
    }
}