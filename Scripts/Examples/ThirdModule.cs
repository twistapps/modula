using Modula.Common;

namespace Modula.Examples
{
    public class ThirdModule : Module
    {
        public override TypedList<IModule> RequiredOtherModules { get; } = new TypedList<IModule>();
        //.Add<ModuleFoo>();
        //.Add<SecondModule>(); 
    }
}