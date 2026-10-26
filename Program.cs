using Microsoft.Extensions.DependencyInjection;

// Elke dienst krijgt bij het aanmaken een korte, willekeurige code (Id). Door die
// codes te vergelijken zie je precies wanneer er een nieuw object gemaakt wordt.
ServiceCollection services = new();
services.AddTransient<TransientDienst>();
services.AddScoped<ScopedDienst>();
services.AddSingleton<SingletonDienst>();

using ServiceProvider provider = services.BuildServiceProvider();

ToonScope("Scope 1", provider);
ToonScope("Scope 2", provider);

// Binnen elke scope vragen we elke dienst twee keer op.
static void ToonScope(string naam, ServiceProvider provider)
{
    Console.WriteLine($"== {naam} ==");
    using IServiceScope scope = provider.CreateScope();
    IServiceProvider sp = scope.ServiceProvider;

    Console.WriteLine($"  transient: {sp.GetRequiredService<TransientDienst>().Id} / {sp.GetRequiredService<TransientDienst>().Id}");
    Console.WriteLine($"  scoped   : {sp.GetRequiredService<ScopedDienst>().Id} / {sp.GetRequiredService<ScopedDienst>().Id}");
    Console.WriteLine($"  singleton: {sp.GetRequiredService<SingletonDienst>().Id} / {sp.GetRequiredService<SingletonDienst>().Id}");
    Console.WriteLine();
}

public sealed class TransientDienst
{
    public string Id { get; } = Guid.NewGuid().ToString()[..4];
}

public sealed class ScopedDienst
{
    public string Id { get; } = Guid.NewGuid().ToString()[..4];
}

public sealed class SingletonDienst
{
    public string Id { get; } = Guid.NewGuid().ToString()[..4];
}
