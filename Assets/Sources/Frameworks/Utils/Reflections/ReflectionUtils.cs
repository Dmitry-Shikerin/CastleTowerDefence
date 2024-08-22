using System;
using System.Collections.Generic;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.Frameworks.Utils.Reflections
{
    public static class ReflectionUtils
    {
        public static void ConstructFsm<T, T2>(this FSMOwner owner, T target, T2 target2)
        {
            owner.behaviour
                .GetAllNodesOfType<FSMState>()
                .CheckAttributes(target, target2);
            owner.behaviour
                .GetAllTasksOfType<ConditionTask>()
                .CheckAttributes(target, target2);
        }

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
    }
}