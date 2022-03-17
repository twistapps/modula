using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

namespace Modula
{
    public class Template : ModularBehaviour
    {
        public TemplateScriptable scriptable;
        [ReadOnly] public string[] selections;

        public override TypedList<IModule> AvailableModules
        {
            get
            {
                var modules = new TypedList<IModule>();
                foreach (var basepart in scriptable.baseparts)
                {
                    var types = basepart.supports.ToTypesList<IModule>();
                    foreach (var type in types)
                    {
                        modules.Add(type);
                    }
                }

                return modules;
            }
        }
    }
}