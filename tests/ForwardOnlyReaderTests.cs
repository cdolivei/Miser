using System;
using System.Collections.Immutable;
using NUnit;
using Miser;

namespace Miser.Tests;

internal sealed class ForwardOnlyReaderTests {

	[Test]
	// Testing test code isn't amazing, but this is pretty critical to know the code
	// is working as intended
	// And what better way than to use LINQ
	public void EnumerableGenerator_ThrowsOnDoubleEnumeration() {
		IEnumerable<int> items = EnumerableGenerator.GenerateList(5);
		Assert.That( items.Any(), Is.True );
		Assert.That( 
			() => items.Any(), Throws.Exception
		);
	}

	[Test]
	public void Head_AlwaysMovesForward() {
		IForwardOnlyReader<int> items = EnumerableGenerator.GenerateList(5).ToForwardOnlyReader();
		Assert.That(items.Head(), Is.EqualTo(1));
		Assert.That(items.Head(), Is.EqualTo(2));
		Assert.That(items.Head(), Is.EqualTo(3));
		Assert.That(items.Head(), Is.EqualTo(4));
		Assert.That(items.Head(), Is.EqualTo(5));
	}

	[Test]
	public void Head_WithEmptyEnumeration_Throws() {
		IForwardOnlyReader<int> items = EnumerableGenerator.Empty.ToForwardOnlyReader();
		Assert.That(
			() => items.Head(), Throws.InvalidOperationException
		);
	}

	[Test]
	public void Take_AlwaysMovesForward() {
		IForwardOnlyReader<int> items = EnumerableGenerator.GenerateList(5).ToForwardOnlyReader();
		Assert.That(items.Take(2), Is.EquivalentTo(new int[] { 1, 2 }));
		Assert.That(items.Take(2), Is.EquivalentTo(new int[] { 3, 4 }));
	}

	[Test]
	public void Take_WithSmallerList_DoesntOverflow() {
		IForwardOnlyReader<int> items = EnumerableGenerator.GenerateList(5).ToForwardOnlyReader();
		Assert.That(items.Take(3), Is.EquivalentTo(new int[] { 1, 2, 3 }));
		Assert.That(items.Take(3), Is.EquivalentTo(new int[] { 4, 5 }));
	}

	[Test]
	public void Take_WithEmptyEnumeration_ReturnsEmptyList() {
		IForwardOnlyReader<int> items = EnumerableGenerator.Empty.ToForwardOnlyReader();
		Assert.That(items.Take(10), Is.Empty);
	}

	[Test]
	public void Drain_ReturnsAllItems() {
		IForwardOnlyReader<int> items = EnumerableGenerator.GenerateList(2).ToForwardOnlyReader();
		Assert.That(items.Drain(), Is.EquivalentTo(new[] { 1, 2 }));
	}

	[Test]
	public void Any_DoesNotConsume() {
		IForwardOnlyReader<int> items = EnumerableGenerator.GenerateList(3).ToForwardOnlyReader();
		bool any = items.Any();
		Assert.That(any, Is.True);
		Assert.That(items.Drain(), Is.EquivalentTo(new[] { 1, 2, 3 }));
	}

	[Test]
	public void Any_WithEmptyList_ReturnsFalse() {
		IForwardOnlyReader<int> items = EnumerableGenerator.Empty.ToForwardOnlyReader();
		bool any = items.Any();
		Assert.That(any, Is.False);
	}


	internal sealed class EnumerableGenerator {
		private bool isDoubleEnumeration = false;

		public static IEnumerable<int> GenerateList(int number) {
			EnumerableGenerator generator = new();
			return generator.GetList(number);
		}
		public static IEnumerable<int> Empty => Enumerable.Empty<int>();
		public IEnumerable<int> GetList(int number) {
			if (isDoubleEnumeration)
			{
				Assert.Fail("Caught double enumeration");
			}
			isDoubleEnumeration = true;
			for (int i = 1; i <= number; ++i)
			{
				yield return i;
			}
		}
	}
}