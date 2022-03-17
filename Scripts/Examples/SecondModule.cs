using UnityEngine;

namespace Modula.Examples
{
    public partial class ExampleData
    {
        [ForModule(typeof(SecondModule))] public string someAnotherString;

        [Header("Second Module")] [ForModule(typeof(SecondModule))]
        public float someFloat;
    }

    public class SecondModule : Module
    {
        public override TypedList<IModule> RequiredOtherModules { get; } = new TypedList<IModule>()
            .Add<ModuleFoo>();

        public bool Test()
        {
            return RequiredOtherModules.Contains<SecondModule>();
        }
    }
}