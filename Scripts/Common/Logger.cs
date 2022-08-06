using System;
using UnityEngine;

namespace Modula.Common
{
    public enum ErrorCode
    {
        __EMPTY,
        MissingModule,
        PropertyNULL
    }

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

        public static void LogRemovingModule(IModule module, ModuleRemoveReason reason, GameObject obj,
            bool shouldLog = true)
        {
            if (!shouldLog) return;
            if (reason == ModuleRemoveReason.__EMPTY) return;

            var message = string.Concat(
                "Removing module: ", module.GetType().Name,
                ", reason: ", GetModuleRemoveReasonSlug(reason),
                " (class ", obj.GetType().Name, ")");
            Debug.Log(message, obj);
        }

        private static string GetErrorMessageByCode(ErrorCode code, GameObject obj, string context)
        {
            string msg;
            switch (code)
            {
                case ErrorCode.__EMPTY:
                default:
                    msg = "Unhandled error.";
                    break;

                case ErrorCode.MissingModule:
                    msg = "Missing Module";
                    break;
                case ErrorCode.PropertyNULL:
                    msg = "Required Property is NULL";
                    break;
            }

            if (obj != null) msg += string.Concat("(on GameObject:", obj.name, ")");
            if (context != null) msg += string.Concat(": ", context);
            return msg;
        }

        public static void LogError(ErrorCode code, GameObject obj, string context = null, bool shouldLog = true)
        {
            if (!shouldLog) return;
            Debug.LogError(GetErrorMessageByCode(code, obj, context), obj);
        }

        public static void LogError(ErrorCode code, GameObject obj, Type context = null, bool shouldLog = true)
        {
            LogError(code, obj, context?.Name, shouldLog);
        }

        public static void LogWarning(ErrorCode code, GameObject obj, string context = null, bool shouldLog = true)
        {
            if (!shouldLog) return;
            Debug.LogWarning(GetErrorMessageByCode(code, obj, context), obj);
        }
        
        public static void LogWarning(ErrorCode code, GameObject obj, Type context = null, bool shouldLog = true)
        {
            LogWarning(code, obj, context?.Name, shouldLog);
        }
    }
}