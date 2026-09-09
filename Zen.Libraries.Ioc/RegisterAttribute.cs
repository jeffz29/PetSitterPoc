namespace Zen.Libraries.Ioc;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class RegisterAttribute : Attribute
{
    public RegisterAttribute()
    {
    }

    public RegisterAttribute(Type serviceType)
    {
        ServiceType = serviceType;
    }

    public Type? ServiceType { get; }

    public RegistrationLifestyle Lifestyle { get; set; } = RegistrationLifestyle.Transient;
}