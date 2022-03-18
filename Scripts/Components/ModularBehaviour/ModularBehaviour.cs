using System;
using System.Collections.Generic;
using System.Linq;
using Modula.Common;
using UnityEditor;
using UnityEngine;

namespace Modula
{
    public abstract class ModularBehaviour : MonoBehaviour
    {
        private List<IModule> _modules;
        private bool couldBeModifiedFromEditorUI => ModulaSettings.DebugMode;
        public abstract TypedList<IModule> AvailableModules { get; }

        public List<IModule> attachments => _modules;

        #region Behaviour

        public DataLayer GetData()
        {
            return GetComponent<DataLayer>();
        }
        
        public virtual Type GetDataLayerType()
        {
            return null;
        }

        public virtual void OnDataComponentCreated()
        {
        }

        #endregion

        #region Adding

        private void OnModuleAdded(IModule module)
        {
            if (module == null) return;
            //Debug.Log("Added module: " + module.GetType().Name);
            _modules.Add(module);
            module.OnAdd();
            UpdateModules();
        }

        public void AddModule(Type moduleType)
        {
            if (_modules.Any(m => m.GetType() == moduleType)) return;
            IModule module;
            if (Application.isEditor && !Application.isPlaying)
                module = Undo.AddComponent(gameObject, moduleType) as IModule;
            else
                module = gameObject.AddComponent(moduleType) as IModule;
            OnModuleAdded(module);
        }

        #endregion

        #region Getting
        
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
        
        public List<IModule> GetModules()
        {
            UpdateModules();
            return _modules;
        }

        public T GetModule<T>() where T : IModule
        {
            return (T)_modules.First(m => m.GetType() == typeof(T));
        }

        public IModule GetModule(string typeName)
        {
            return _modules.Find(m => m.GetName() == typeName);
        }

        public IModule GetModule(Type moduleType)
        {
            return _modules.Find(m => m.GetType() == moduleType);
        }

        #endregion
        
        #region Removing

        //todo: recursively check dependencies of dependencies
        public bool CanRemove(IModule module)
        {
            return _modules.All(m =>
            {
                var required = m.RequiredOtherModules;
                return required == null || !required.Contains(module.GetType());
            });
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

        #endregion
        
        private void UpdateModules(bool shouldResolveDependencies=true)
        {
            _modules = gameObject.FindComponents<IModule>();
            
            foreach (var module in _modules)
            {
                if (shouldResolveDependencies || couldBeModifiedFromEditorUI)
                    // check if user did remove a module using Unity Editor GUI. If yes, return;
                    // because ResolveDependencies implicitly calls UpdateModules() again if _modules has been modified.
                    if (DependencyWorker.ResolveDependencies(module))
                        return;
                if (AvailableModules.Contains(module.GetType())) continue;
                else if (CanRemove(module))
                {
                    RemoveModule(module);
                    return;
                }
            }
        }
    }
}