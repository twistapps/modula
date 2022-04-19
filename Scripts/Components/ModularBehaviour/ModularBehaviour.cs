using System;
using System.Collections.Generic;
using System.Linq;
using Modula.Common;
using UnityEditor;
using UnityEngine;
using Logger = Modula.Common.Logger;

namespace Modula
{
    public abstract class ModularBehaviour : MonoBehaviour
    {
        private List<IModule> _modules;
        private static bool couldBeModifiedFromEditorUI => ModulaSettings.DebugMode;
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
            _modules.Add(module);
            module.OnAdd();
            UpdateModules();
        }

        private IModule CreateModuleComponent(Type moduleType)
        {
            IModule module;
            if (Application.isEditor && !Application.isPlaying)
                module = Undo.AddComponent(gameObject, moduleType) as IModule;
            else
                module = gameObject.AddComponent(moduleType) as IModule;
            
            return module;
        }

        public void AddModule(Type type)
        {
            if (_modules.Any(m => m.GetType() == type)) return;
            var module = CreateModuleComponent(type);
            OnModuleAdded(module);
        }

        public void AddModule<T>() where T : IModule
        {
            AddModule(typeof(T));
        }

        public void AddModules(Type[] types)
        {
            types = types.Where(m => _modules.All(a => a.GetType() != m)).ToArray();
            foreach (var type in types)
            {
                CreateModuleComponent(type);
            }
        }

        #endregion

        #region Getting

        public List<IModule> GetAttachments()
        {
            UpdateModules();
            return _modules;
        }

        public T GetModule<T>() where T : IModule
        {
            return (T)_modules.FirstOrDefault(m => m.GetType() == typeof(T));
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
        // IsRequiredByOtherAttachments
        public bool CanRemove(IModule module)
        {
            return _modules.All(m =>
            {
                var required = m.RequiredOtherModules;
                return required == null || !required.Contains(module.GetType());
            });
        }

        private void DestroyModule(IModule module, ModuleRemoveReason reason = ModuleRemoveReason.NotSpecified)
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                Logger.LogRemovingModule(module, reason, gameObject);
                Undo.DestroyObjectImmediate(module as MonoBehaviour);
            }
            else
            {
                Destroy(module as MonoBehaviour);
            }
        }
        
        public void RemoveModule(IModule module, ModuleRemoveReason reason = ModuleRemoveReason.NotSpecified)
        {
            _modules.Remove(module);
            DestroyModule(module, reason);
        }

        public void RemoveModules(IModule[] toRemove, ModuleRemoveReason reason = ModuleRemoveReason.NotSpecified)
        {
            _modules = _modules.Except(toRemove).ToList();
            foreach (var module in toRemove)
            {
                DestroyModule(module, reason);
            }
        }

        #endregion
        
        private void UpdateModules(bool shouldResolveDependencies=true)
        {
            _modules = gameObject.FindComponents<IModule>();
            
            if (shouldResolveDependencies || couldBeModifiedFromEditorUI)
                if (DependencyWorker.ResolveDependencies(_modules, this))
                    return;

            var toRemove = _modules.Where(m => !AvailableModules.Contains(m.GetType()) && CanRemove(m));
            RemoveModules(toRemove.ToArray());
        }
    }
}