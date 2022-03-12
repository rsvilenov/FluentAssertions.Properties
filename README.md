# About this project
Unofficial FluentAssertions extensions for testing the behavior of class/struct/record properties.

[![build workflow](https://github.com/rsvilenov/FluentAssertions.Properties/actions/workflows/build.yml/badge.svg)](https://github.com/rsvilenov/FluentAssertions.Properties/actions/workflows/build.yml) 
[![Coverage Status](https://coveralls.io/repos/github/rsvilenov/FluentAssertions.Properties/badge.svg)](https://coveralls.io/github/rsvilenov/FluentAssertions.Properties)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0) 
[![nuget](https://img.shields.io/nuget/v/FluentAssertions.Properties)](https://www.nuget.org/packages/FluentAssertions.Properties)

## How?

* Test that all properties from a class return the same value that has been assigned to them:
```csharp
    var dtoUnderTest = new SampleDto();
    var testValues = new Fixture().Create<SampleDto>();

    sampleDto
        .Properties()
        .ThatAreWritable
        .WhenCalledWithValuesFrom(testValues)
        .Should()
        .ProvideSymmetricAccess();
```

* Test that getters/setters throws exceptions:

```csharp
    var objectUnderTest = new TestClass();
            
    objectUnderTest
        .Properties()
        .OfType<string>()
        .WhenCalledWith(null)
        .Should()
        .ThrowFromSetter<ArgumentNullException>()
        .WithMessage("Nulls are not accepted here");
```

* Select specific properties to test:
```csharp
    var objectUnderTest = new TestClass();
            
    objectUnderTest
        .Properties(o => o.PropertyOne, o => o.PropertyTwo)
        .Should()
        .BeInitOnly();
```
or
```csharp
    var objectUnderTest = new TestClass();
            
    objectUnderTest
        .Properties()
        .OfType<string>()
        .HavingValue("some value")
        .Should()
        .HaveCount(2);
```

## Why should I consider testing my class properties?
The public properties are part of the public contract. Their semantics is such that
one expects them to behave like public fields. However, they have accessor methods, 
which can contain logic that breaks the expected behavior. This is sometimes considered
to be an <a href="https://www.codeproject.com/Tips/1069467/Asymmetric-Property-anti-pattern">anti-pattern</a>,
and rightfully so - in order for a programmer to see how a particular property behaves,
they have to open the implementation of the class and look inside the code. The presence of accessor
methods is a big part of the reason Microsoft has provided a list of <a href="https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/property">bad practices and design guidelines</a> that are often seen 
when properties are implemented.

## But that goes against the conventional wisdom!
There is a general rule of thumb that says properties should not be tested if their getter or setter does not
have any logic inside. However, no one can guarantee that the setter/getter, which is nothing but a method,
will never contain any complex logic. So, if we agree that we should always test our public methods, 
why not test our public properties as well? And there seems to be a <a href="https://stackoverflow.com/questions/18967697/should-you-unit-test-simple-properties">not-so-small minority</a>, which agrees.
