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
        public ForModuleAttribute(Type targetComponent, DrawDisabledAs drawDisabledAs = DrawDisabledAs.ReadOnly)
        {
            this.targetComponent = targetComponent;
            DrawDisabledType = drawDisabledAs;
        }

        #region Fields

        public DrawDisabledAs DrawDisabledType { get; }
        public readonly Type targetComponent;

        #endregion
    }
}