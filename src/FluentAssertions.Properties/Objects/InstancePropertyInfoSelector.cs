using FluentAssertions.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FluentAssertions.Properties.Objects
{
    public class InstancePropertyInfoSelector<TDeclaringType, TProperty> : IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>>
    {

        protected IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> SelectedProperties { get; set; } = new List<InstancePropertyInfo<TDeclaringType, TProperty>>();
        private readonly TDeclaringType _instance;
        public InstancePropertyInfoSelector(TDeclaringType instance, InstancePropertyInfo<TDeclaringType, TProperty> instancePropertyInfo)
            : this(instance, new[] { instancePropertyInfo })
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstancePropertyInfoSelector"/> class.
        /// </summary>
        /// <param name="types">The types from which to select properties.</param>
        /// <exception cref="ArgumentNullException"><paramref name="types"/> is <c>null</c>.</exception>
        public InstancePropertyInfoSelector(TDeclaringType instance, IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> instancePropertyInfos)
        {
            _instance = instance;
            SelectedProperties = instancePropertyInfos;
        }

        public InstancePropertyInfoSelector<TDeclaringType, TProperty> HavingValue(TProperty value)
        {
            SelectedProperties = SelectedProperties
                .Where(property => property.PropertyValue.Equals(value));

            return this;
        }


        /// <summary>
        /// Only select the properties that return the specified type
        /// </summary>
        public InstanceWithValuePropertyInfoSelector<TDeclaringType, TProperty> WhenCalledWith(TProperty value)
        {
            return new InstanceWithValuePropertyInfoSelector<TDeclaringType, TProperty> (_instance, value, SelectedProperties);
        }

        public IEnumerator<InstancePropertyInfo<TDeclaringType, TProperty>> GetEnumerator()
        {
            return SelectedProperties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return SelectedProperties.GetEnumerator();
        }
    }

    /// <summary>
    /// Allows for fluent selection of properties of a type through reflection.
    /// </summary>
    public class InstancePropertyInfoSelector<TDeclaringType> : IEnumerable<InstancePropertyInfo<TDeclaringType>>
    {
        protected IEnumerable<InstancePropertyInfo<TDeclaringType>> SelectedProperties { get; set; } = new List<InstancePropertyInfo<TDeclaringType>>();
        protected TDeclaringType Instance { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="InstancePropertyInfoSelector"/> class.
        /// </summary>
        /// <param name="types">The types from which to select properties.</param>
        /// <exception cref="ArgumentNullException"><paramref name="types"/> is <c>null</c>.</exception>
        public InstancePropertyInfoSelector(TDeclaringType instance, IEnumerable<string> propertyNames = null)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(paramName: nameof(instance));
            }

            Instance = instance;

            var properties = new List<InstancePropertyInfo<TDeclaringType>>();

            Type type = instance.GetType();
            foreach (PropertyInfo propInfo in type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (propertyNames == null || propertyNames.Contains(propInfo.Name))
                {
                    properties.Add(new InstancePropertyInfo<TDeclaringType>(propInfo));
                }
            }


            SelectedProperties = properties;
        }

        /// <summary>
        /// Only select the properties that have a public or internal getter.
        /// </summary>
        public InstancePropertyInfoSelector<TDeclaringType> ThatArePublicOrInternal
        {
            get
            {
                SelectedProperties = SelectedProperties.Where(property =>
                {
                    MethodInfo getter = property.PropertyInfo.GetGetMethod(nonPublic: true);
                    return (getter != null) && (getter.IsPublic || getter.IsAssembly);
                });

                return this;
            }
        }

        /// <summary>
        /// Only select the properties that return the specified type
        /// </summary>
        public InstancePropertyInfoSelector<TDeclaringType, TProperty> OfType<TProperty>()
        {
            var selectors = SelectedProperties
                .Where(property => property.PropertyInfo.PropertyType == typeof(TProperty))
                .Select(p => new InstancePropertyInfo<TDeclaringType, TProperty>(p));

            return new InstancePropertyInfoSelector<TDeclaringType, TProperty>(Instance, selectors);
        }



        public InstancePropertyInfoSelector<TDeclaringType> ThatAreOfPrimitiveTypes
        {
            get
            {
                SelectedProperties = SelectedProperties
                    .Where(property => property.PropertyInfo.PropertyType.IsPrimitive);

                return this;
            }
        }

        public InstancePropertyInfoSelector<TDeclaringType> ThatAreWritable
        {
            get
            {
                SelectedProperties = SelectedProperties.Where(property =>
                {
                    MethodInfo setter = property.PropertyInfo.GetSetMethod(nonPublic: true);
                    return (setter != null) && (setter.IsPublic || setter.IsAssembly);
                });

                return this;
            }
        }

        /// <summary>
        /// Only select the properties that do not return the specified type
        /// </summary>
        public InstancePropertyInfoSelector<TDeclaringType> NotOfType<TProperty>()
        {
            SelectedProperties = SelectedProperties
                .Where(property => property.PropertyInfo.PropertyType != typeof(TProperty));

            return this;
        }

        /// <summary>
        /// Select return types of the properties
        /// </summary>
        public TypeSelector ReturnTypes()
        {
            var returnTypes = SelectedProperties.Select(property => property.PropertyInfo.PropertyType);

            return new TypeSelector(returnTypes);
        }

        /// <summary>
        /// The resulting <see cref="PropertyInfo"/> objects.
        /// </summary>
        public InstancePropertyInfo<TDeclaringType>[] ToArray()
        {
            return SelectedProperties.ToArray();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<InstancePropertyInfo<TDeclaringType>> GetEnumerator()
        {
            return SelectedProperties.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
