﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using Sirenix.Utilities;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.Frameworks.Utils.Reflections
{
    public static class ReflectionUtils
    {
        // public static void ConstructFsm<T, T2>(this FSMOwner owner, T target, T2 target2)
        // {
        //     owner.behaviour
        //         .GetAllNodesOfType<FSMState>()
        //         .CheckAttributes(target, target2);
        //     owner.behaviour
        //         .GetAllTasksOfType<ConditionTask>()
        //         .CheckAttributes(target, target2);
        // }

        private static void CheckAttributes<T, T2>(this IEnumerable<object> objects, T target, T2 target2)
        {
            foreach (object state in objects)
            {
                Type type = state.GetType();
                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                
                foreach (MethodInfo method in type.GetMethods(flags))
                {
                    if (method.GetCustomAttribute<ConstructAttribute>() != null)
                    {
                        ParameterInfo[] parameters = method.GetParameters();
                        
                        if (parameters.Length != 2)
                            continue;
                        
                        if (parameters[0].ParameterType != typeof(T))
                            continue;
                        
                        if (parameters[1].ParameterType != typeof(T2))
                            continue;
                        
                        method.Invoke(state, new object[] { target, target2 });
                    }
                }
            }
        }

        public static void ConstructFsm(this FSMOwner owner, params object[] dependencies)
        {
            owner.behaviour
                .GetAllNodesOfType<FSMState>()
                .CheckAttributes(dependencies);
            owner.behaviour
                .GetAllTasksOfType<ConditionTask>()
                .CheckAttributes(dependencies);
        }
        
        private static void CheckAttributes(this IEnumerable<object> objects, params object[] dependencies)
        {
            foreach (object state in objects)
            {
                Type type = state.GetType();
                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                
                foreach (MethodInfo method in type.GetMethods(flags))
                {
                    if (method.GetCustomAttribute<ConstructAttribute>() != null)
                    {
                        ParameterInfo[] parameterInfos = method.GetParameters();
                        CheckDependenciesLength(parameterInfos, method, dependencies);
                        object[] parameters = GetParameters(parameterInfos, method, dependencies);
                        
                        method.Invoke(state, parameters);
                    }
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
                bool isNotFondType = dependencies.Any(dependency => dependency.GetType() == parameter.ParameterType);
                bool isNotFondInterfacesType = dependencies.Any(dependency => dependency
                    .GetType()
                    .GetInterfaces()
                    .Any(type => type == parameter.ParameterType));
                
                if (isNotFondType == false && isNotFondInterfacesType == false)
                {
                    notFoundTypes.Add(parameter.ParameterType);
                    continue;
                }
                
                foreach (object dependency in dependencies)
                {
                    if (parameter.ParameterType == dependency.GetType())
                    {
                        dependenciesList.Add(dependency);
                        continue;
                    }

                    if (dependency.GetType().GetInterfaces().ToList().Contains(parameter.ParameterType))
                    {
                        dependenciesList.Add(dependency);
                    }
                }
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
    }
}