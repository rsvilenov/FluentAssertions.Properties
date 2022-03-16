# Selecting properties for assertion

## Selecting properties from a type
The original FluentAssertions library already provides means of enumerating all publicly visible properties of a certain type. See <a href="https://fluentassertions.com/typesandmethods/" target="_blank"> this </a> for details. Using these selectors and the corresponding assertions, we can assert that the definitions of the properties meat certain criteria.

## Selecting properties from an instance
When we want to assert the <b>behavior</b> of properties, rather than how they are defined, we need to operate upon the instance of the type that has defined the properties. To do this, we can use the selectors, provided by FluentAssertions.Properties.

### Selecting all publicly visible properties

Publicly visible properties are those that can be seen from the project, containing the tests. These are all public properties as well as the internal ones from the friend assemblies (see <a href="https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.internalsvisibletoattribute?view=net-6.0" target="_blank">InternalsVisibleToAttribute</a>).

```csharp
    // Instantiate the type
    var instance = new TypeUnderTest();

    // Select all publicly visible properties
    var properties = instance.Properties();

    // Assert their behavior
    properties
        .WhenCalledWithValuesFrom(valueSource)
        .Should()
        .ProvideSymmetricAccess();
```

In the above example `valueSource` is an instance of the same object or an instance of an object with the same interface, filled with test values (see <a href="./Assertions.md:#Passing-a-value-source-object">this</a> for details).
### Selecting properties by name

It is possible to select individual properties by their name:
```csharp
var properties = instance.Properties(t => t.Name, t => t.Surname);
```
### Filtering the selection
The properties, selected with the property selectors, can be filtered further by their type, value or definition details:
```csharp
    // Select all publicly visible properties
    var properties = instance.Properties();

    // Get all writable properties of type int that have value of 1:
    var filteredProperties = properties
        .OfType<int>()
        .ThatAreWritable
        .HavingValue(1);

```