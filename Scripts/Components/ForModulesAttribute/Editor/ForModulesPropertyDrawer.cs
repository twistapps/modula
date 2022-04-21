using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Modula.Editor
{
    /// <summary>
    ///     Based on: https://forum.unity.com/threads/draw-a-field-only-if-a-condition-is-met.448855/
    /// </summary>
    [CustomPropertyDrawer(typeof(ForModulesAttribute))]
    public class ForModulesPropertyDrawer : PropertyDrawer
    {
        #region Fields

        // Reference to the attribute on the property.
        private ForModulesAttribute forModule;

        #endregion

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!ShowMe(property) && forModule.DrawDisabledType == DrawDisabledAs.Hide)
                return -EditorGUIUtility.standardVerticalSpacing;

            if (property.propertyType != SerializedPropertyType.Generic)
                return EditorGUI.GetPropertyHeight(property, label);

            var numChildren = 0;
            var totalHeight = 0.0f;

            var children = property.GetEnumerator();

            while (children.MoveNext())
            {
                var child = children.Current as SerializedProperty;
                var childLabel = new GUIContent(child.displayName);

                totalHeight += EditorGUI.GetPropertyHeight(child, childLabel) +
                               EditorGUIUtility.standardVerticalSpacing;
                numChildren++;
            }

            // Remove extra space at end, (we only want spaces between items)
            totalHeight -= EditorGUIUtility.standardVerticalSpacing;

            return totalHeight;
        }

        /// <summary>
        ///     Errors default to showing the property.
        /// </summary>
        private bool ShowMe(SerializedProperty property)
        {
            forModule = attribute as ForModulesAttribute;

            var obj = (MonoBehaviour)property.serializedObject.targetObject;
            var hasAtLeastOneOfComponents =
                forModule!.targetComponents.Any(component => obj.GetComponent(component) != null);
            return hasAtLeastOneOfComponents;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShowMe(property))
            {
                if (property.propertyType == SerializedPropertyType.Generic)
                {
                    var children = property.GetEnumerator();

                    var offsetPosition = position;

                    while (children.MoveNext())
                    {
                        var child = children.Current as SerializedProperty;

                        var childLabel = new GUIContent(child.displayName);

                        var childHeight = EditorGUI.GetPropertyHeight(child, childLabel);
                        offsetPosition.height = childHeight;

                        EditorGUI.PropertyField(offsetPosition, child, childLabel);

                        offsetPosition.y += childHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
                else
                {
                    EditorGUI.PropertyField(position, property, label);
                }
            }
            else if (forModule.DrawDisabledType == DrawDisabledAs.ReadOnly)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label);
                GUI.enabled = true;
            }
        }
    }
}