using System.Linq;
using UnityEngine;

namespace Modula.Common
{
    public static class ModulaSettings
    {
        public const string Component = "Module.";
        public const string EMPTY_CHOICE = "<Empty>";
        public const string DATA_SUFFIX = "Data";

        private static bool _debugMode;

        public static bool DebugMode
        {
            get => _debugMode;
            set
            {
                _debugMode = value;
                CallDebugModeUpdate(value);
            }
        }

        public static bool EditMode
        {
            get;
            set;
            //CallEditModeUpdate(value);
        }

        private static void CallDebugModeUpdate(bool value)
        {
            var instances = Object.FindObjectsOfType<MonoBehaviour>().OfType<IDebugModeUpdateListener>();
            foreach (var instance in instances) instance.DebugModeUpdated(value);
        }
    }
}