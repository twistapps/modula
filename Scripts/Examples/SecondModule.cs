using UnityEngine;

namespace Modula.Examples
{
    public partial class ExampleData
    {
        [ForModule(typeof(SecondModule))] public string someAnotherString;

        [Header("Second Module")] [ForModule(typeof(SecondModule))] [Range(-1, 3)]
        public float someFloat;
    }

    public class SecondModule : Module
    {
        public override TypeList RequiredOtherModules { get; } = new TypeList()
            .Add(typeof(ModuleFoo));
    }
}