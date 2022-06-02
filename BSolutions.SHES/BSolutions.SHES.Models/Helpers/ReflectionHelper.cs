using System;

namespace BSolutions.SHES.Models.Helpers
{
    public static class ReflectionHelper
    {
        public static T GetInstance<T>(string fullyQualifiedName)
        {
            try
            {
                Type type = Type.GetType(fullyQualifiedName);

                if (type != null)
                {
                    return (T)Activator.CreateInstance(type);
                }

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = assembly.GetType(fullyQualifiedName);

                    if (type != null)
                    {
                        return (T)Activator.CreateInstance(type);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return default;
        }
    }
}
