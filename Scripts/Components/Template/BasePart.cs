using System.Collections.Generic;
using UnityEngine;

namespace Modula
{
    [CreateAssetMenu(fileName = "Basepart", menuName = "Modula/Templates/BasePart", order = 2)]
    public class BasePart : ScriptableObject
    {
        public new string name = "Basepart";
        public List<string> supports = new List<string>();

        /// <summary>
        ///     Set this to true if this part can be set to "None" thus module is removed.
        /// </summary>
        public bool optional;
    }
}