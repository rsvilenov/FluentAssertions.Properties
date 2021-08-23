﻿using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Properties.Selectors
{
    public class InstancePropertySelector<TDeclaringType> :  InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstancePropertyInfoSelector"/> class.
        /// </summary>
        /// <param name="types">The types from which to select properties.</param>
        /// <exception cref="ArgumentNullException"><paramref name="types"/> is <c>null</c>.</exception>
        internal InstancePropertySelector(TDeclaringType instance, IEnumerable<string> propertyNames = null)
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
                if ((propertyNames == null || propertyNames.Contains(propInfo.Name))
                    && IsPropertyPublicOrInternal(propInfo))
                {

                    properties.Add(new InstancePropertyInfo<TDeclaringType>(propInfo));
                }
            }


            SelectedProperties = properties;
        }

        private bool IsPropertyPublicOrInternal(PropertyInfo propertyInfo)
        {
            MethodInfo getter = propertyInfo.GetGetMethod(nonPublic: true);
            return (getter != null) && (getter.IsPublic || getter.IsAssembly);
        }

        /// <summary>
        /// Only select the properties that return the specified type
        /// </summary>
        public InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty> OfType<TProperty>()
        {
            var selectors = SelectedProperties
                .Where(property => property.PropertyInfo.PropertyType == typeof(TProperty))
                .Select(p => new InstancePropertyInfo<TDeclaringType, TProperty>(p));

            return new InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty>(Instance, selectors);
        }

        /// <summary>
        /// Only select the properties that do not return the specified type
        /// </summary>
        public InstancePropertySelector<TDeclaringType> NotOfType<TProperty>()
        {
            SelectedProperties = SelectedProperties
                .Where(property => property.PropertyInfo.PropertyType != typeof(TProperty));

            return this;
        }

        public InstancePropertyOfValueTypeSelector<TDeclaringType> ThatAreOfValueTypes
        {
            get
            {
                SelectedProperties = SelectedProperties
                    .Where(property => property.PropertyInfo.PropertyType.IsValueType ||
                        (property.PropertyInfo.PropertyType.CheckIfTypeIsNullableValueType()
                            && property.PropertyInfo.PropertyType.GetActualTypeIfNullable().IsValueType));

                return new InstancePropertyOfValueTypeSelector<TDeclaringType>(this);
            }
        }

        public InstancePropertySelector<TDeclaringType> ThatAreOfReferenceTypes
        {
            get
            {
                SelectedProperties = SelectedProperties
                    .Where(property => !property.PropertyInfo.PropertyType.IsValueType &&
                        (property.PropertyInfo.PropertyType.CheckIfTypeIsNullableValueType()
                            && !property.PropertyInfo.PropertyType.GetActualTypeIfNullable().IsValueType));

                return this;
            }
        }


        ///// <summary>
        ///// Only select the properties that are decorated with an attribute of the specified type.
        ///// </summary>
        //public InstancePropertySelector<TDeclaringType> ThatAreDecoratedWith<TAttribute>()
        //    where TAttribute : Attribute
        //{
        //    SelectedProperties = SelectedProperties.Where(property => property.IsDecoratedWith<TAttribute>());
        //    return this;
        //}


        ///// <summary>
        ///// Only select the properties that are not decorated with an attribute of the specified type.
        ///// </summary>
        //public PropertyInfoSelector ThatAreNotDecoratedWith<TAttribute>()
        //    where TAttribute : Attribute
        //{
        //    selectedProperties = selectedProperties.Where(property => !property.IsDecoratedWith<TAttribute>());
        //    return this;
        //}
    }
}
