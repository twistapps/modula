using UnityEngine;

namespace Modula.Common
{
    public class Logger
    {
        
        private static string GetModuleRemoveReasonSlug(ModuleRemoveReason reason)
        {
            switch (reason)
            {
                case ModuleRemoveReason.NotSupportedByThisBehaviour:
                    return "Module is not supported by ModularNetBehaviour of this type";
                case ModuleRemoveReason.RemovedFromGUI:
                    return "Module is removed using Editor GUI";
                case ModuleRemoveReason.ClearingBehaviour:
                    return "Clearing ModularBehaviour...";
                case ModuleRemoveReason.NotSpecified:
                default:
                    return "Reason is not specified.";
            }
        }
        
        public static void LogRemovingModule(IModule module, ModuleRemoveReason reason, GameObject obj, bool shouldLog=true)
        {
            if (!shouldLog) return;
            if (reason == ModuleRemoveReason.__EMPTY) return;
            
            var message = string.Concat(
                "Removing module: ", module.GetType().Name,
                ", reason: ", GetModuleRemoveReasonSlug(reason),
                " (class ", obj.GetType().Name, ")");
            Debug.Log(message, obj);
        }
    }
}