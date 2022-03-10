using System;
using Modula.Optimizations;
using UnityEngine;

namespace Modula
{
    public abstract class Module : MonoBehaviour, IModule
    {
        private ModuleDefaultImplementation _defaultImplementation;

        protected ModuleDefaultImplementation DefaultImplementation
        {
            get { return _defaultImplementation ??= new ModuleDefaultImplementation(this); }
        }

        public virtual void Update()
        {
            DefaultImplementation.Update();
        }

        public TimingConstraints UpdateInvocationConstraints => DefaultImplementation.UpdateConstraints;
        public virtual TypeList RequiredOtherModules { get; } = TypeList.None;

        public ModularBehaviour Parent { get; }

        public void OnAdd()
        {
            DefaultImplementation.OnAdd();
        }

        public void AddModule(Type moduleType)
        {
            DefaultImplementation.AddModule(moduleType);
        }

        public string GetName()
        {
            return DefaultImplementation.GetName();
        }

        public DataLayer GetData()
        {
            return DefaultImplementation.GetData();
        }

        public virtual void ModuleUpdate()
        {
        }
    }
}