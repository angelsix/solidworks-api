namespace AngelSix.SolidDna
{
    /// <summary>
    /// Used to flag a method inside an assembly that should be used to configure services
    /// in the IoC. The method must have a single parameter of <see cref="System.Action{FrameworkConstruction}"/>
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class ConfigureServiceAttribute : System.Attribute
    {
    }
}
