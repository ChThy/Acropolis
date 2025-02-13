﻿namespace Acropolis.Shared;

public static class TypeHelpers
{
    public static IEnumerable<(Type ServiceType, Type ImplementationType)> OpenGenericsFor(this IEnumerable<Type> types, Type openGenericType)
    {
        return types.Where(t => !t.IsInterface && !t.IsAbstract && t.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition().IsAssignableTo(openGenericType)))
            .SelectMany(t => t.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition().IsAssignableTo(openGenericType))
                .Select(i =>(i , t)));
    }
}
