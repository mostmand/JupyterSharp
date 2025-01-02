using System;
using JupyterSharp.DependencyInjection.Abstraction;

namespace JupyterSharp.DependencyInjection;

internal sealed class RandomNameGenerator : IRandomNameGenerator
{
    public string GenerateRandomName()
    {
        return Guid.NewGuid().ToString();
    }
}
