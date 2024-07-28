using System.Collections.Immutable;

namespace Miser;

public interface IForwardOnlyReader<T> {
	/// <summary>
	/// Identical to Enumerable.Any()
	/// </summary>
	/// <returns>true if there are any results left</returns>
	bool Any();

	/// <summary>
	/// Similar to Enumerable.Take(), but tries to resolve the results immediately.
	/// This does not defer execution!
	/// </summary>
	/// <param name="count">Number of items to take</param>
	/// <returns>Up to count items from the list</returns>
	ImmutableArray<T> Take(int count);

	/// <summary>
	/// Returns the next top result from the Enumerable(). Unlike Enumerable.First()
	/// it will not always return the first result
	/// </summary>
	/// <returns>The next top result</returns>
	T Head();

	/// <summary>
	/// Returns the remaining results
	/// </summary>
	IEnumerable<T> Drain();
}