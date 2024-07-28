## Miser

This is a terrible name, but it's a name.

Miser is some C# code that is meant to replace some functionality of LINQ's. Specifically, it's goal is to never enumerate an enumerable twice.

To use, just call `.ToForwardOnlyReader()` on an `IEnumerable<T>`

```csharp
IForwardOnlyReader items = Enumerable.Range(1,10)ToForwardOnlyReader();
```

There are a number of similar methods to LINQ, like `.Any()` and `.Take()` but there are key differences in many of them

Documentation is mostly in code in the [IForwardOnlyReader<T>](src/IForwardOnlyReader.cs) file