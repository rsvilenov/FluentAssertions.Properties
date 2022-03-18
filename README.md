# FluentAssertions.Properties

Unofficial FluentAssertions extensions for testing the behavior of class/struct/record properties.

[![build workflow](https://github.com/rsvilenov/FluentAssertions.Properties/actions/workflows/build.yml/badge.svg)](https://github.com/rsvilenov/FluentAssertions.Properties/actions/workflows/build.yml) 
[![Coverage Status](https://coveralls.io/repos/github/rsvilenov/FluentAssertions.Properties/badge.svg)](https://coveralls.io/github/rsvilenov/FluentAssertions.Properties)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0) 
[![nuget](https://img.shields.io/nuget/v/FluentAssertions.Properties)](https://www.nuget.org/packages/FluentAssertions.Properties)

## Table of Contents  

- [How?](#How)
- [Why?](#Why)
- [Installation](#Installation)
- [Documentation](#Documentation)

## How?

Some common scenarios:

* Testing that all properties from a class provide symmetric access, i.e. they return the same value that has been assigned to them
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

Speaking in the lingo of AutoFixture we can say that `ProvideSymmetricAccess()` verifies that the properteis are "well-behaved writables" (see [AutoFixture's WritablePropertyAssertion idiom](http://www.shujaat.net/2013/05/writable-property-assertions-using.html)).

* Testing that getters/setters throw exceptions in certain cases

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

* Selecting specific properties to test by their type and value

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

or selecting individual properties by name
```csharp
    var objectUnderTest = new TestRecord();
    string testValue = Guid.NewGuid().ToString();

    objectUnderTest
        .Properties(o => o.StringPropertyOne, o => o.StringPropertyTwo)
        .WhenCalledWith(testValue)
        .Should()
        .ProvideSymmetricAccess();
```

A more comprehensive explanation of the selection and assertions methods, provided by this library, can be found [here](./Selectors.md) and [here](./Assertions.md).

## Why?

> Even if code is trivial you should still test it.
> 
> -- <cite>Mark Seemann</cite>

### Why should I consider testing my class properties?
From the perspective of the caller, the public properties are part of the public "interface" of a type. They imply a contract - their semantics is such that
one expects them to behave like public fields. However, they have accessor methods, 
which can contain logic that modifies the expected behavior. Implementing nontrivial logic in the accessors is sometimes considered
to be an [anti-pattern](https://www.codeproject.com/Tips/1069467/Asymmetric-Property-anti-pattern),
and rightfully so - in order for a programmer to see how a particular property behaves,
they have to open the implementation of the type and look inside the code. The presence of accessor
methods is a big part of the reason why Microsoft has provided a list of [bad practices and design guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/property), concerning the implementation of properties.

### But that goes against the conventional wisdom!
There is a rule of thumb that says properties should not be tested if their getter and setter do not
contain any logic, e.g. if they are [auto-implemented](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/auto-implemented-properties). Even Robert C. Martin seems to think this way. However, there are other prominent authors, such as [Mark Seeman](https://blog.ploeh.dk/2013/03/08/test-trivial-code/), who strongly disagree. And there seems to be a [not-so-small minority](https://stackoverflow.com/questions/18967697/should-you-unit-test-simple-properties), which thinks that testing all public properties is necessary.

## Installation
This library is distributed as a [NuGet](https://www.nuget.org/packages/FluentAssertions.Properties/).

To install `FluentAssertions.Properties`, run the following command in the Package Manager Console:

```
PM> Install-Package FluentAssertions.Properties
```
Or use this command with the .NET CLI:
```
> dotnet add package FluentAssertions.Properties
```

## Documentation

[Property selector methods](https://github.com/rsvilenov/FluentAssertions.Properties/blob/master/docs/Selectors.md).

[Assertion methods](https://github.com/rsvilenov/FluentAssertions.Properties/blob/master/docs/Assertions.md).
