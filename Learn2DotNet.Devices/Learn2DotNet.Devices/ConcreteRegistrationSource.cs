using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;

namespace Learn2DotNet.Devices
{
    public class ConcreteRegistrationSource : IRegistrationSource
    {
        public bool IsAdapterForIndividualComponents => false;

        public List<string> AllowedNamespaces { get; } = new List<string>();

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            IServiceWithType swt = service as IServiceWithType;

            bool canHandle = swt != null &&
                             swt.ServiceType.IsClass &&
                             IsAllowedNamespace(swt.ServiceType.Namespace);

            if (!canHandle)
                return Enumerable.Empty<IComponentRegistration>();

            ComponentRegistration registration = new ComponentRegistration(
                Guid.NewGuid(),
                new DelegateActivator(swt.ServiceType, (c, p) => ResolveType(c, swt.ServiceType)),
                new CurrentScopeLifetime(),
                InstanceSharing.None,
                InstanceOwnership.OwnedByLifetimeScope,
                new[] { service },
                new Dictionary<string, object>());

            return new IComponentRegistration[] { registration };
        }

        private bool IsAllowedNamespace(string @namespace)
        {
            return AllowedNamespaces.Count == 0 || AllowedNamespaces.All(@namespace.StartsWith);
        }

        private static object ResolveType(IComponentContext componentContext, Type serviceType)
        {
            ParameterInfo[][] constructors = serviceType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .Select(x => x.GetParameters())
                .ToArray()
                .OrderByDescending(o => o.Length)
                .ToArray();

            if (!constructors.Any())
                return null;

            object[] arguments = ResolveParameters(componentContext, constructors);

            return arguments == null
                ? null
                : Activator.CreateInstance(serviceType, arguments);
        }

        private static object[] ResolveParameters(IComponentContext componentContext, IEnumerable<ParameterInfo[]> constructors)
        {
            foreach (ParameterInfo[] constructor in constructors)
            {
                bool hasNull = false;
                object[] values = new object[constructor.Length];

                for (int i = 0; i < constructor.Length; i++)
                {
                    object value = componentContext.Resolve(constructor[i].ParameterType);
                    values[i] = value;

                    if (value == null)
                    {
                        hasNull = true;
                        break;
                    }
                }

                if (!hasNull)
                {
                    // found a constructor we can create.
                    return values;
                }
            }

            return null;
        }
    }
}