using Modula.Common;

namespace Modula.Examples
{
    public class ThirdPersonCamera : Module
    {
        public override TypedList<IModule> RequiredOtherModules { get; } = new TypedList<IModule>()
            .Add<ModuleFoo>();
    }
}