#if MIRROR
using System;
using Mirror;
using Modula.Optimizations;

namespace Modula
{
    public abstract class NetworkModule : NetworkBehaviour, IModule
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

        public ModularBehaviour Main { get; }

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
#endif