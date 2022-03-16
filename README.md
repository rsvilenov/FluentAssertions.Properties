# About this project
Unofficial FluentAssertions extensions for testing the behavior of class/struct/record properties.

[![build workflow](https://github.com/rsvilenov/FluentAssertions.Properties/actions/workflows/build.yml/badge.svg)](https://github.com/rsvilenov/FluentAssertions.Properties/actions/workflows/build.yml) 
[![Coverage Status](https://coveralls.io/repos/github/rsvilenov/FluentAssertions.Properties/badge.svg)](https://coveralls.io/github/rsvilenov/FluentAssertions.Properties)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0) 
[![nuget](https://img.shields.io/nuget/v/FluentAssertions.Properties)](https://www.nuget.org/packages/FluentAssertions.Properties)

## Table of Contents  

- [How?](#How)
- [Why?](#Why)
- [Installation](#Installation)

## How?

* Test that all properties from a class provide symmetric access, i.e. return the same value that has been assigned to them
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

* Test that getters/setters throw exceptions in certain cases

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

* Select specific properties to test by their type and value

```csharp
    var objectUnderTest = new TestClass();
            
    objectUnderTest
        .Properties()
        .OfType<double>()
        .ThatHaveDefaultValue
        .Should()
        .HaveCount(10);
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

or select individual properties by name
```csharp
    var objectUnderTest = new TestRecord();
    string testValue = Guid.NewGuid().ToString();

    objectUnderTest
        .Properties(o => o.StringPropertyOne, o => o.StringPropertyTwo)
        .WhenCalledWith(testValue)
        .Should()
        .ProvideSymmetricAccess();
```

## Why?

### Why should I consider testing my class properties?
The public properties are part of the public contract of a type. Their semantics is such that
one expects them to behave like public fields. However, they have accessor methods, 
which can contain logic that breaks the expected behavior. Implementing nontrivial logic in the accessors is sometimes considered
to be an [anti-pattern](https://www.codeproject.com/Tips/1069467/Asymmetric-Property-anti-pattern){:target="_blank"},
and rightfully so - in order for a programmer to see how a particular property behaves,
they have to open the implementation of the class and look inside the code. The presence of accessor
methods is a big part of the reason why Microsoft has provided a list of [bad practices and design guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/property){:target="_blank"} that are often seen 
when properties are implemented.

### But that goes against the conventional wisdom!
There is a general rule of thumb that says properties should not be tested if their getter or setter does not
have any logic inside. However, no one can guarantee that the setter/getter, which is nothing but a method,
will never contain any complex logic. So, if we agree that we should always test our public methods, 
why not test our public properties as well? And there seems to be a [not-so-small minority](https://stackoverflow.com/questions/18967697/should-you-unit-test-simple-properties){:target="_blank"}, which agrees.

## Installation
You can view the [package page on NuGet](https://www.nuget.org/packages/FluentAssertions.Properties/){:target="_blank"}.

To install `FluentAssertions.Properties`, run the following command in the Package Manager Console:

```
PM> Install-Package FluentAssertions.Properties
```
Or use this command with the .NET CLI:
```
> dotnet add package FluentAssertions.Properties
```