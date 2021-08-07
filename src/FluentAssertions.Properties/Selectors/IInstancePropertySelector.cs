using FluentAssertions.Properties.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentAssertions.Properties.Selectors
{
    public interface IInstancePropertySelector<TDeclaringType>
    {
        TDeclaringType Instance { get; set; }
        IEnumerable<InstancePropertyInfo<TDeclaringType>> SelectedProperties { get; set; }

    }
}
