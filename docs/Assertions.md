# Asserting the behavior of selected properties

## Table of Contents
- [Asserting the behavior of selected properties](#asserting-the-behavior-of-selected-properties)
  * [Asserting that what goes in, goes out unchanged (symmetricity)](#asserting-that-what-goes-in--goes-out-unchanged--symmetricity-)
    + [Passing a fixed value of a certain type](#passing-a-fixed-value-of-a-certain-type)
    + [Passing a value source object](#passing-a-value-source-object)
  * [Asserting that the getters or setters of the selected properties throw exceptions under certain conditions](#asserting-that-the-getters-or-setters-of-the-selected-properties-throw-exceptions-under-certain-conditions)
    + [Asserting that the exception meets the expected criteria](#asserting-that-the-exception-meets-the-expected-criteria)


## Asserting that what goes in, goes out unchanged (symmetricity)
In order to test the behavior of the getters and setters of the selected properties, we should assign some values to them. FluentAssertions.Properties provides two ways of passing the values to the properties - by passing a fixed value of a certain type or by using a value source object.

### Passing a fixed value of a certain type

```csharp
    var instance = new TypeUnderTest();
    string testValue = Guid.NewGuid().ToString();

    instance
        .Properties()
        .ExactlyOfType<string>()
        .WhenCalledWith(testValue)
        .Should()
        .ProvideSymmetricAccess();
```

### Passing a value source object
To generate test values, we can use the wonderful library [AutoFixture](https://github.com/AutoFixture/AutoFixture). Or we can create an object of the type under test (or a type that implement the same interface) and assign the test values manually.

- Creating a value source object with AutoFixture:

```csharp
    var valueSource = new Fixture().Create<TestClass>();
```

- Creating a value source object manually

```csharp
    var valueSource = new TestClass
    {
        Name = "Milan",
        Surname = "Kundera"
    };
```

* Using the value source object

```csharp
    var instance = new TypeUnderTest();
    var valueSource = ...;

    instance
        .Properties()
        .WhenCalledWithValuesFrom(valueSource)
        .Should()
        .ProvideSymmetricAccess();
```

## Asserting that the getters or setters of the selected properties throw exceptions under certain conditions


Asserting the throwing of exceptions requires passing a value or a value source object and then using the exception assertion methods:

```csharp
    var instance = new TypeUnderTest();
    var valueSource = ...;

    instance
        .Properties(t => t.Name, t => t.Surname)
        .WhenCalledWith(null)
        .Should()
        .ThrowFromSetter<ArgumentNullException>()
        .WithMessage("Nulls are not accepted.");
```

### Asserting that the exception meets the expected criteria 

* Asserting that the exception has the expected message:

```csharp
        ///...
        .Should()
        .ThrowFromSetter<ArgumentNullException>()
        .WithMessage("Nulls are not accepted.");
```

The method `WithMessage()` supports wildcard patterns. The supported wildcard characters are described here: [https://fluentassertions.com/exceptions/](https://fluentassertions.com/exceptions/).

* Asserting that the exception is exactly of a certain type and not from a derived one:

```csharp
        ///...
        .Should()
        .ThrowFromSetterExactly<ArgumentNullException>()
```

* Asserting that the exception has an inner exception of an expected type:

```csharp
        ///...
        .Should()
        .ThrowFromSetter<ArgumentNullException>()
        .WithInnerException<NullReferenceException>();
```

* Asserting using a custom lambda:

```csharp
        ///...
        .Should()
        .ThrowFromSetter<ArgumentNullException>()
        .Where(ex => ex.Message.Contains("unexpected"));
```

* Chaining the assertion methods:

```csharp
        ///...
        .Should()
        .ThrowFromSetter<ArgumentNullException>()
        .WithInnerExceptionExactly<NullReferenceException>()
        .WithMessage("Object reference *");
```