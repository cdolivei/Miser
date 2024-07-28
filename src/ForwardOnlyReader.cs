using System.Collections;
using System.Collections.Immutable;

namespace Miser;

internal sealed class ForwardOnlyReader<T> : IForwardOnlyReader<T> {
	private readonly EnumeratorWrapper<T> m_enumerator;

	public ForwardOnlyReader(IEnumerable<T> enumerable) {
		m_enumerator = new EnumeratorWrapper<T>(enumerable.GetEnumerator());
	}

	bool IForwardOnlyReader<T>.Any() => m_enumerator.HasCurrentItem();

	ImmutableArray<T> IForwardOnlyReader<T>.Take(int count) =>
		ImmutableArray.Create(Consume(count).ToArray());

	IEnumerable<T> IForwardOnlyReader<T>.Drain() {
		while (m_enumerator.HasCurrentItem())
		{
			yield return m_enumerator.Current;
			m_enumerator.MoveNext();
		}
	}

	IEnumerable<T> Consume(int count) {
		int i = 0;
		while (m_enumerator.HasCurrentItem() && i++ < count)
		{
			yield return m_enumerator.Current;
			m_enumerator.MoveNext();
		}
	}

	T IForwardOnlyReader<T>.Head() {
		if (!m_enumerator.HasCurrentItem())
		{
			throw new InvalidOperationException();
		}
		T result = m_enumerator.Current;
		m_enumerator.MoveNext();
		return result;
	}
}
