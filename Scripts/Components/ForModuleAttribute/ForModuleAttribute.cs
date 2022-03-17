using System;
using UnityEngine;

namespace Modula
{
    /// <summary>
    ///     Based on: https://forum.unity.com/threads/draw-a-field-only-if-a-condition-is-met.448855/
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ForModuleAttribute : PropertyAttribute
    {
        public ForModuleAttribute(Type targetComponent, DisabledDrawType disabledDrawType = DisabledDrawType.ReadOnly)
        {
            this.targetComponent = targetComponent;
            disabledType = disabledDrawType;
        }

        #region Fields

        public DisabledDrawType disabledType { get; }
        public readonly Type targetComponent;

        #endregion
    }
}