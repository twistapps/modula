using Modula.Common;

namespace Modula.Examples
{
    public class ModuleFoo : Module
    {
        // public override TypeList RequiredOtherModules { get; } = new TypeList()
        //     .Add(
        //         typeof(SecondModule),
        //         typeof(ThirdModule)
        //         );

        public override TypedList<IModule> RequiredOtherModules { get; } = new TypedList<IModule>()
            .Add<ThirdModule>();

        public void Awake()
        {
            UpdateInvocationConstraints.SetFrames(15);
            UpdateInvocationConstraints.SetSeconds(3.5f);
        }
    }
}