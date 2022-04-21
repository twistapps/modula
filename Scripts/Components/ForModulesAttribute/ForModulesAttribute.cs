using System;
using UnityEngine;

namespace Modula
{
    /// <summary>
    ///     Based on: https://forum.unity.com/threads/draw-a-field-only-if-a-condition-is-met.448855/
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ForModulesAttribute : PropertyAttribute
    {
        public ForModulesAttribute(DrawDisabledAs drawDisabledAs, params Type[] targetComponents)
        {
            this.targetComponents = targetComponents;
            DrawDisabledType = drawDisabledAs;
        }

        public ForModulesAttribute(params Type[] targetComponents)
        {
            this.targetComponents = targetComponents;
            DrawDisabledType = DrawDisabledAs.Hide;
        }

        #region Fields

        public DrawDisabledAs DrawDisabledType { get; }
        public readonly Type[] targetComponents;

        #endregion
    }
}