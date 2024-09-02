using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.Frameworks.Utils.Reflections
{
    public static class ReflectionUtils
    {
        public static void ConstructFsm(this FSMOwner owner, params object[] dependencies)
        {
            owner.behaviour
                .GetAllNodesOfType<FSMState>()
                .CheckAttributes(dependencies);
            owner.behaviour
                .GetAllTasksOfType<ConditionTask>()
                .CheckAttributes(dependencies);
        }

        private static readonly List<Func<object, Type>> _targetTypes = new List<Func<object, Type>>()
        {
            obj => obj.GetType(),
            obj => obj.GetType().BaseType,
            obj => obj.GetType().BaseType?.BaseType,
            obj => obj.GetType().BaseType?.BaseType?.BaseType,
        };

        private static void CheckAttributes(this IEnumerable<object> objects, params object[] dependencies)
        {
            foreach (object state in objects)
            {
                ResolveDependencies(state, dependencies);
            }
        }

        private static void ResolveDependencies(object targetObject, object[] dependencies)
        {
            foreach (Func<object, Type> type in _targetTypes)
            {
                Resolve(targetObject, type.Invoke(targetObject), dependencies);
            }
        }

        private static void Resolve(object targetObject, Type type, object[] dependencies)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            foreach (MethodInfo method in type.GetMethods(flags))
            {
                if (method.GetCustomAttribute<ConstructAttribute>() != null)
                {
                    ParameterInfo[] parameterInfos = method.GetParameters();
                    CheckDependenciesLength(parameterInfos, method, dependencies);
                    object[] parameters = GetParameters(parameterInfos, method, dependencies);
                    CheckDependenciesLength(parameterInfos, method, parameters);

                    method.Invoke(targetObject, parameters);
                }
            }
        }

        private static object[] GetParameters(
            ParameterInfo[] parameters,
            MethodInfo method,
            object[] dependencies)
        {
            List<object> dependenciesList = new List<object>();
            List<Type> notFoundTypes = new List<Type>();

            foreach (ParameterInfo parameter in parameters)
            {
                bool isNotFondType = dependencies.Any(
                    dependency => dependency.GetType() == parameter.ParameterType);
                bool isNotFondInterfacesType = dependencies.Any(dependency => dependency
                    .GetType()
                    .GetInterfaces()
                    .Any(type => type == parameter.ParameterType));
                bool isNotFoundBaseType = dependencies.Any(dependency => dependency
                    .GetType().BaseType == parameter.ParameterType);

                if (isNotFondType == false && isNotFondInterfacesType == false && isNotFoundBaseType == false)
                {
                    notFoundTypes.Add(parameter.ParameterType);
                    continue;
                }

                foreach (object dependency in dependencies)
                {
                    if (dependency.GetType().GetInterfaces().ToList().Contains(parameter.ParameterType))
                    {
                        dependenciesList.Add(dependency);
                        continue;
                    }
                    
                    foreach (Func<object, Type> type in _targetTypes)
                    {
                        if (parameter.ParameterType != type.Invoke(dependency))
                            continue;

                        dependenciesList.Add(dependency);
                        break;
                    }
                }
            }

            if (parameters.Length < dependenciesList.Count)
            {
                throw new IndexOutOfRangeException(
                    $"Dependencies count more parameters length {method.Name} " +
                    $"({string.Join(", ", dependenciesList.Select(dependency => dependency.GetType().Name))})");
            }

            if (notFoundTypes.Count > 0)
            {
                throw new ArgumentNullException(
                    $"Not enough dependencies for {method.Name} " +
                    $"({string.Join(", ", notFoundTypes)})");
            }

            return dependenciesList.ToArray();
        }

        private static void CheckDependenciesLength(
            ParameterInfo[] parameters,
            MethodInfo method,
            object[] dependencies)
        {
            if (parameters.Length > dependencies.Length)
            {
                List<Type> targetTypes = new List<Type>();

                foreach (ParameterInfo parameter in parameters)
                {
                    foreach (object dependency in dependencies)
                    {
                        if (parameter.ParameterType == dependency.GetType())
                            continue;

                        targetTypes.Add(parameter.ParameterType);
                    }
                }

                throw new IndexOutOfRangeException(
                    $"Not enough dependencies for {method.Name} " +
                    $"({string.Join(", ", targetTypes)})");
            }
        }
        
        public static IEnumerable<Type> GetFilteredTypeList<T>()
        {
            return typeof(T).Assembly.GetTypes()
                .Where(type => type.IsAbstract == false)
                .Where(type => type.IsGenericTypeDefinition == false)
                .Where(type => typeof(T).IsAssignableFrom(type));
        }
    }
}