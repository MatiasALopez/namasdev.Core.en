using System;
using System.Linq;
using System.Reflection;

using namasdev.Core.Validation;

namespace namasdev.Core.Reflection
{
    public class ReflectionHelper
    {
        public static bool ClassImplementsInterface<TEntity, TInterface>()
        {
            return ClassImplementsInterface(typeof(TEntity), typeof(TInterface));
        }

        public static bool ClassImplementsInterface(Type classType, Type interfaceType)
        {
            return interfaceType.IsAssignableFrom(classType);
        }

        public static object GetDefaultValue<T>()
        {
            return GetDefaultValue(typeof(T));
        }

        public static object GetDefaultValue(Type type)
        {
            Validator.ValidateRequiredArgumentAndThrow(type, nameof(type));

            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        public static bool AllObjectPropertiesMatch<T>(T obj1, T obj2) 
            where T : class
        {
            Validator.ValidateRequiredArgumentAndThrow(obj1, nameof(obj1));
            Validator.ValidateRequiredArgumentAndThrow(obj2, nameof(obj2));

            if (obj1 == obj2)
            {
                return true;
            }

            Type type = typeof(T);
            
            foreach (var pi in GetProperties(type))
            {
                object obj1Value = type.GetProperty(pi.Name).GetValue(obj1, null);
                object obj2Value = type.GetProperty(pi.Name).GetValue(obj2, null);

                if (obj1Value != obj2Value && (obj1Value == null || !obj1Value.Equals(obj2Value)))
                {
                    return false;
                }
            }

            return true;
        }

        public static PropertyInfo[] GetProperties<T>()
        {
            return GetProperties(typeof(T));
        }

        public static PropertyInfo[] GetProperties(Type classType)
        {
            Validator.ValidateRequiredArgumentAndThrow(classType, nameof(classType));

            return classType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static PropertyInfo[] GetPropertiesOfType<TClass, TProperty>()
        {
            return GetPropertiesOfType(typeof(TClass), typeof(TProperty));
        }

        public static PropertyInfo[] GetPropertiesOfType(Type classType, Type propertyType)
        {
            Validator.ValidateRequiredArgumentAndThrow(propertyType, nameof(propertyType));

            return GetProperties(classType)
                .Where(p => p.PropertyType == propertyType)
                .ToArray();
        }

        public static T CreateInstance<T>(params object[] constructorParameters)
            where T : class
        {
            return (T)Activator.CreateInstance(typeof(T), constructorParameters);
        }

        public static bool AllPropertiesAreNull<T>(T obj)
            where T : class
        {
            Validator.ValidateRequiredArgumentAndThrow(obj, nameof(obj));

            return !GetProperties(typeof(T))
                .Select(p => p.GetValue(obj))
                .Any(v => v != null && !String.IsNullOrWhiteSpace(Convert.ToString(v)));
        }

        public static bool AllPropertiesAreDefault<T>(T obj)
            where T : class
        {
            Validator.ValidateRequiredArgumentAndThrow(obj, nameof(obj));

            return !GetProperties(typeof(T))
                .Any(p => !object.Equals(p.GetValue(obj), GetDefaultValue(p.PropertyType)));
        }

        public static TAttribute GetFieldAttribute<TAttribute>(Type classType, string fieldName)
            where TAttribute : Attribute
        {
            Validator.ValidateRequiredArgumentAndThrow(classType, nameof(classType));
            Validator.ValidateRequiredArgumentAndThrow(fieldName, nameof(fieldName));

            var field = classType.GetField(fieldName);
            if (field == null)
            {
                throw new MissingMemberException(classType.FullName, fieldName);
            }

            return GetMemberAttribute<TAttribute>(field);
        }

        public static TAttribute GetPropertyAttribute<TAttribute>(Type classType, string propertyName)
           where TAttribute : Attribute
        {
            Validator.ValidateRequiredArgumentAndThrow(classType, nameof(classType));
            Validator.ValidateRequiredArgumentAndThrow(propertyName, nameof(propertyName));

            var property = classType.GetProperty(propertyName);
            if (property == null)
            {
                throw new MissingMemberException(classType.FullName, propertyName);
            }

            return GetMemberAttribute<TAttribute>(property);
        }

        private static TAttribute GetMemberAttribute<TAttribute>(MemberInfo member,
            Func<TAttribute, bool> attributeFilter = null)
          where TAttribute : Attribute
        {
            Validator.ValidateRequiredArgumentAndThrow(member, nameof(member));

            var attributes = member.GetCustomAttributes(typeof(TAttribute), false);
            if (attributes == null || attributes.Length == 0)
            {
                return null;
            }

            var attribute = (TAttribute)attributes.FirstOrDefault();
            if (attributeFilter != null && !attributeFilter(attribute))
            {
                return null;
            }

            return attribute;
        }

        public static PropertyInfo[] GetPropertiesWithAttribute<TClass, TAttribute>(
            Func<TAttribute, bool> attributeFilter = null)
            where TAttribute : Attribute
        {
            return GetPropertiesWithAttribute<TAttribute>(typeof(TClass), attributeFilter);
        }

        public static PropertyInfo[] GetPropertiesWithAttribute<TAttribute>(Type classType,
            Func<TAttribute, bool> attributeFilter = null)
            where TAttribute : Attribute
        {
            Validator.ValidateRequiredArgumentAndThrow(classType, nameof(classType));

            return GetProperties(classType)
                .Where(p => GetMemberAttribute<TAttribute>(p, attributeFilter) != null)
                .ToArray();
        }

        public static PropertyInfo[] GetPropertiesOfTypeWithAttribute<TClass, TProperty, TAttribute>(
            Func<TAttribute, bool> attributeFilter = null)
           where TAttribute : Attribute
        {
            return GetPropertiesOfTypeWithAttribute<TAttribute>(typeof(TClass), typeof(TProperty), attributeFilter);
        }

        public static PropertyInfo[] GetPropertiesOfTypeWithAttribute<TAttribute>(Type classType, Type propertyType,
            Func<TAttribute, bool> attributeFilter = null)
           where TAttribute : Attribute
        {
            Validator.ValidateRequiredArgumentAndThrow(classType, nameof(classType));

            return GetPropertiesOfType(classType, propertyType)
                .Where(p => GetMemberAttribute<TAttribute>(p, attributeFilter) != null)
                .ToArray();
        }

        public static T GetPropertyValue<T>(Type type, object obj, string propertyName)
        {
            var p = type.GetProperty(propertyName);
            if (p == null)
            {
                throw new MissingMemberException(type.Name, propertyName);
            }

            return (T)p.GetValue(obj);
        }

        public static void SetPropertyValue(Type type, object obj, string propertyName, object value)
        {
            var p = type.GetProperty(propertyName);
            if (p == null)
            {
                throw new MissingMemberException(type.Name, propertyName);
            }

            p.SetValue(obj, value);
        }
    }
}
