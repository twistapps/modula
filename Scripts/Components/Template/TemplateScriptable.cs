using System.Collections.Generic;
using UnityEngine;

namespace Modula
{
    [CreateAssetMenu(fileName = "Template", menuName = "Modula/Templates/Template", order = 1)]
    public class TemplateScriptable : ScriptableObject
    {
        public List<BasePart> baseparts = new List<BasePart>();
    }
}