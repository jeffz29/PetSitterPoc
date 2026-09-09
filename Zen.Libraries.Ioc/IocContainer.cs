using System.Reflection;
using SimpleInjector;

namespace Zen.Libraries.Ioc;

public sealed class IocContainer
{
    private readonly Container container;

    public IocContainer()
        : this(new Container())
    {
    }

    public IocContainer(Container container)
    {
        this.container = container ?? throw new ArgumentNullException(nameof(container));
    }

    public Container Container => container;

    public void RegisterAssemblies(params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        foreach (var assembly in assemblies)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            foreach (var implementationType in assembly.GetTypes())
            {
                RegisterType(implementationType);
            }
        }
    }

    public T GetInstance<T>() where T : class => container.GetInstance<T>();

    public object GetInstance(Type serviceType)
    {
        ArgumentNullException.ThrowIfNull(serviceType);
        return container.GetInstance(serviceType);
    }

    public void Verify() => container.Verify();

    private void RegisterType(Type implementationType)
    {
        if (!implementationType.IsClass || implementationType.IsAbstract)
        {
            return;
        }

        var registrations = implementationType
            .GetCustomAttributes<RegisterAttribute>(inherit: false)
            .ToArray();

        foreach (var registration in registrations)
        {
            var serviceType = registration.ServiceType ?? GetDefaultServiceType(implementationType);
            if (!serviceType.IsAssignableFrom(implementationType))
            {
                throw new InvalidOperationException(
                    $"'{implementationType.FullName}' cannot be registered as '{serviceType.FullName}'.");
            }

            container.Register(serviceType, implementationType, ToLifestyle(registration.Lifestyle));
        }
    }

    private static Type GetDefaultServiceType(Type implementationType)
    {
        var serviceTypes = implementationType.GetInterfaces();
        return serviceTypes.Length == 1
            ? serviceTypes[0]
            : implementationType;
    }

    private static Lifestyle ToLifestyle(RegistrationLifestyle lifestyle) => lifestyle switch
    {
        RegistrationLifestyle.Transient => Lifestyle.Transient,
        RegistrationLifestyle.Singleton => Lifestyle.Singleton,
        RegistrationLifestyle.Scoped => Lifestyle.Scoped,
        _ => throw new ArgumentOutOfRangeException(nameof(lifestyle), lifestyle, null)
    };
}