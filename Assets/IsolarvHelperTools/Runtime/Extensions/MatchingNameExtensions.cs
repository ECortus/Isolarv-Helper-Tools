using UnityEngine;

namespace IsolarvHelperTools.Runtime
{
    public static class MatchingNameExtensions
    {
        public static bool SearchStringMatches(this Object obj, string searchText)
        {
            return obj && obj.name.ToLower().Contains(searchText);
        }
        
        public static bool SearchStringMatches(this ScriptableObject obj, string searchText)
        {
            return obj && obj.name.ToLower().Contains(searchText);
        }
    }
}