using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ExampleProject.Data
{
    public static class ImplementedClasses<T> where T : class
    {
        public static List<TypeInfo> ClassList
        {
            get
            {
                if (typeof(T).GetTypeInfo().IsInterface)
                    return typeof(T).GetTypeInfo().Assembly.DefinedTypes.Where(x => x.ImplementedInterfaces.Any(y => y == typeof(T))).ToList();
                else
                    throw new Exception("Only allowed interface types!");
            }
        }
    }
}
