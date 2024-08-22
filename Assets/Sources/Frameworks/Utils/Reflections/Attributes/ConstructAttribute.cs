using System;
using JetBrains.Annotations;

namespace Sources.Frameworks.Utils.Reflections.Attributes
{
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    [AttributeUsage(AttributeTargets.Method)]
    public class ConstructAttribute : Attribute
    {
    }
}