[![CI](https://github.com/cdolivei/Miser/actions/workflows/ci.yml/badge.svg)](https://github.com/cdolivei/Miser/actions/workflows/ci.yml)

## Miser

This is a terrible name, but it's a name.

Miser is some C# code that is meant to replace some functionality of LINQ's. Specifically, it's goal is to never enumerate an enumerable twice.

To use, just call `.ToForwardOnlyReader()` on an `IEnumerable<T>`

Example:
```csharp
IForwardOnlyReader<int> items = Enumerable.Range(1, 10).ToForwardOnlyReader();
Assert.That(items.Any(), Is.True);
Assert.That(items.Head(), Is.EqualTo(1));
Assert.That(items.Take(2), Is.EquivalentTo(new int[] { 2, 3 }));
Assert.That(items.Take(2), Is.EquivalentTo(new int[] { 4, 5 }));
Assert.That(items.Head(), Is.EqualTo(6));
Assert.That(items.Head(), Is.EqualTo(7));
Assert.That(items.Drain(), Is.EquivalentTo(new int[] { 8, 9, 10 }));
Assert.That(items.Any(), Is.False);
```

There are a number of similar methods to LINQ, like `.Any()` and `.Take()` but there are key differences in many of them

Documentation is mostly in code in the [IForwardOnlyReader<T>](src/Miser.cs#L8) file.

This library is not distributed as a nuget package. You must copy the source file.