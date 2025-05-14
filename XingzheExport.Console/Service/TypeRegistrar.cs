using Spectre.Console.Cli;

namespace XingzheExport.Console.Service;

internal class TypeRegistrar(IServiceProvider services) : ITypeRegistrar
{
    public ITypeResolver Build()
    {
        return new TypeResolver(services);
    }

    public void Register(Type service, Type implementation)
    {
        
    }

    public void RegisterInstance(Type service, object implementation)
    {

    }

    public void RegisterLazy(Type service, Func<object> factory)
    {

    }
}

file class TypeResolver(IServiceProvider services) : ITypeResolver
{
    public object? Resolve(Type? type)
    {
        if(type == null) return null;
        return services.GetService(type);
    }
}