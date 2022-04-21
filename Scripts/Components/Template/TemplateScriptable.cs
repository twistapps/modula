using System.Collections.Generic;
using UnityEngine;

namespace Modula
{
    [CreateAssetMenu(fileName = "Template", menuName = "Modula/Templates/Template", order = 1)]
    public class TemplateScriptable : ScriptableObject
    {
        public new string name = "Template";
        public Texture banner;
        public string dataLayerType = "";
        public List<BasePart> baseparts = new();
    }
}