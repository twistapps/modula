namespace Modula.Examples
{
    public class ThirdModule : Module
    {
        public override TypeList RequiredOtherModules { get; } = new TypeList()
            .Add(
                typeof(ModuleFoo),
                typeof(SecondModule)
            );
    }
}