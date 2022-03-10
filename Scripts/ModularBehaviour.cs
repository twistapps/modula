using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Modula
{
    public abstract class ModularBehaviour : MonoBehaviour
    {
        //public PropertyModule properties;
        private List<IModule> _modules;

        public abstract TypeList AvailableModules { get; }
        //public abstract Type[] GetAvailableModules();


        public DataLayer GetData()
        {
            return GetComponent<DataLayer>();
        }

        // public PropertyModule GetProperties()
        // {
        //     return properties;
        // }

        public virtual Type GetDataLayerType()
        {
            return null;
        }

        public virtual void OnDataComponentCreated()
        {
        }

        public void AddModule(IModule module)
        {
            _modules.Add(module);
            module.OnAdd();
            OnModulesUpdate();
        }

        public void AddModule(Type moduleType)
        {
            IModule module;
            if (Application.isEditor && !Application.isPlaying)
            {
                module = Undo.AddComponent(gameObject, moduleType) as IModule;
            }
            else
            {
                module = gameObject.AddComponent(moduleType) as IModule;
            }
            AddModule(module);
        }

        public List<IModule> GetModules()
        {
            OnModulesUpdate();
            return _modules;
        }

        public T GetModule<T>() where T : IModule
        {
            return (T) _modules.First(m => m.GetType() == typeof(T));
        }

        private string GetReasonString(ModuleRemoveReason reason)
        {
            switch (reason)
            {
                case ModuleRemoveReason.NotSupportedByThisBehaviour:
                    return "Module is not supported by ModularNetBehaviour of this type";
                case ModuleRemoveReason.RemovedFromGUI:
                    return "Module is removed using Editor GUI";
                case ModuleRemoveReason.NotSpecified:
                default:
                    return "Reason is not specified.";
            }
        }

        public bool CanRemove(IModule module)
        {
            return _modules.All(m =>
            {
                if (m.RequiredOtherModules == null)
                    return true;
                return !m.RequiredOtherModules.Contains(module.GetType());
            });
        }

        public IModule[] FindDependent(IModule module)
        {
            return _modules.Where(m =>
            {
                if (m.RequiredOtherModules == null)
                    return false;
                return m.RequiredOtherModules.Contains(module.GetType());
            }).ToArray();
        }

        public string[] FindDependentModuleNames(IModule module)
        {
            var dependent = FindDependent(module);
            var names = new List<string>();
            foreach (var netModule in dependent) names.Add(netModule.GetName());

            return names.ToArray();
        }

        public void RemoveModule(IModule module, ModuleRemoveReason reason = ModuleRemoveReason.NotSpecified)
        {
            _modules.Remove(module);
            if (Application.isEditor && !Application.isPlaying)
            {
                Debug.Log(string.Concat("Removing module: ", module.GetType().Name,
                    ", reason: ", GetReasonString(reason), " (class ", GetType().Name, ")"), gameObject);
                Undo.DestroyObjectImmediate(module as MonoBehaviour);
                //DestroyImmediate(module as MonoBehaviour);
            }
            else
            {
                Destroy(module as MonoBehaviour);
            }
        }

        private void OnModulesUpdate()
        {
            _modules = gameObject.FindComponents<IModule>();
            //_modules = GetComponents<Module>().ToList();
            foreach (var module in _modules)
            {
                if (AvailableModules.Contains(module.GetType())) continue;
                if (CanRemove(module)) RemoveModule(module);
            }
        }
    }
}