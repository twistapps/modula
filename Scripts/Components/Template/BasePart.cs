﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modula
{
    [CreateAssetMenu(fileName = "Basepart", menuName = "Modula/Templates/BasePart", order = 2)]
    [Serializable]
    public class BasePart : ScriptableObject
    {
        public new string name = "Basepart";
        [HideInInspector] public List<string> supports = new List<string>();

        /// <summary>
        ///     Set this to true if this part can be set to "None" thus module is removed.
        /// </summary>
        public bool optional;
    }
}