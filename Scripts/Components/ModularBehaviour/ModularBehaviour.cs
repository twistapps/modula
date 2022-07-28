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
        private static bool couldBeModifiedFromEditorUI => ModulaSettings.DebugMode;
        public abstract TypedList<IModule> AvailableModules { get; }

        public List<IModule> attachments { get; private set; }

        #region Lifecycle

        private void Awake()
        {
            UpdateModules();
        }

        #endregion

        private void UpdateModules(bool shouldResolveDependencies = true)
        {
            if (isActiveAndEnabled)
                attachments = gameObject.FindComponents<IModule>();

            if (shouldResolveDependencies || couldBeModifiedFromEditorUI)
                if (DependencyWorker.ResolveDependencies(attachments, this))
                    return;

            var toRemove = attachments.Where(m => !AvailableModules.Contains(m.GetType()) && CanRemove(m)).ToList();
            foreach (var module in attachments)
            {
                var duplicate = attachments.FirstOrDefault(m => m.GetType() == module.GetType() && m != module);
                if (duplicate != null) toRemove.Add(duplicate);
            }

            RemoveModules(toRemove.ToArray());
        }

        #region Behaviour

        public DataLayer GetData()
        {
            return GetComponent<DataLayer>();
        }

        public virtual Type GetDataLayerType()
        {
            return ModulaUtilities.GetDerivedFrom<DataLayer>()
                .SingleOrDefault(type => type.Name == GetType().Name + ModulaSettings.DATA_SUFFIX);
            //return null;
        }

        public virtual void OnDataComponentCreated()
        {
        }

        #endregion

        #region Adding

        private void OnModuleAdded(IModule module)
        {
            if (module == null) return;
            attachments.Add(module);
            module.OnAdd();
            UpdateModules();
        }

        private void OnModulesAdded(IEnumerable<IModule> modules)
        {
            foreach (var module in modules)
            {
                attachments.Add(module);
                module.OnAdd();
            }

            UpdateModules();
        }

        private IModule CreateModuleComponent(Type moduleType)
        {
            IModule module;
            #if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
                module = Undo.AddComponent(gameObject, moduleType) as IModule;
            else
            #endif
                module = gameObject.AddComponent(moduleType) as IModule;

            return module;
        }

        public IModule AddModule(Type type)
        {
            if (attachments.Any(m => m.GetType() == type)) return null;
            var module = CreateModuleComponent(type);
            OnModuleAdded(module);
            return module;
        }

        public IModule AddModule<T>() where T : IModule
        {
            return AddModule(typeof(T));
        }

        public void AddModules(Type[] types)
        {
            types = types.Where(m => attachments.All(a => a.GetType() != m)).ToArray();
            foreach (var type in types) CreateModuleComponent(type);
        }

        #endregion

        #region Getting

        public List<IModule> GetAttachments()
        {
            UpdateModules();
            return attachments;
        }

        public T GetModule<T>() where T : IModule
        {
            return (T)attachments.FirstOrDefault(m => m.GetType() == typeof(T));
        }

        public IModule GetModule(string typeName)
        {
            return attachments.Find(m => m.GetName() == typeName);
        }

        public IModule GetModule(Type moduleType)
        {
            return attachments?.FirstOrDefault(m => m.GetType() == moduleType);
            //return _modules.Find(m => m.GetType() == moduleType);
        }

        public T GetModuleByBase<T>() where T : IModule
        {
            return (T)attachments.FirstOrDefault(m => m.GetType().IsSubclassOf(typeof(T)));
        }

        #endregion

        #region Removing

        //todo: recursively check dependencies of dependencies
        // IsRequiredByOtherAttachments
        public bool CanRemove(IModule module)
        {
            return attachments.All(m =>
            {
                var required = m.RequiredOtherModules;
                return required == null || !required.Contains(module.GetType());
            });
        }

        private void DestroyModule(IModule module, ModuleRemoveReason reason = ModuleRemoveReason.NotSpecified)
        {
            #if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                var mono = module as MonoBehaviour;
                if (mono == null) return;
                Logger.LogRemovingModule(module, reason, gameObject);
                Undo.DestroyObjectImmediate(mono);
            }
            else
            {
                Destroy(module as MonoBehaviour);
            }
            #else
                Destroy(module as MonoBehaviour);
            #endif
        }

        public void RemoveModule(IModule module, ModuleRemoveReason reason = ModuleRemoveReason.NotSpecified)
        {
            attachments.Remove(module);
            DestroyModule(module, reason);
        }

        public void RemoveModules(IModule[] toRemove, ModuleRemoveReason reason = ModuleRemoveReason.NotSpecified)
        {
            attachments = attachments.Except(toRemove).ToList();
            foreach (var module in toRemove) DestroyModule(module, reason);
        }

        #endregion

        #region Checks

        public bool HasModule<T>() where T : IModule
        {
            return GetModule<T>() != null;
        }

        public bool HasModule(Type type)
        {
            return GetModule(type) != null;
        }

        #endregion
    }
}