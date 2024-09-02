/* Copyright 2024 Cesar Oliveira
 * Under the MIT licence. The latest version can be found in https://github.com/cdolivei/Miser
 */
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

public static class IEnumerableExtensions {
	public static IForwardOnlyReader<T> ToForwardOnlyReader<T>(this IEnumerable<T> values) {
		return new ForwardOnlyReader<T>(values);
	}

    private sealed class ForwardOnlyReader<T> : IForwardOnlyReader<T> {
        private readonly EnumeratorWrapper m_enumerator;

        public ForwardOnlyReader(IEnumerable<T> enumerable) {
            m_enumerator = new EnumeratorWrapper(enumerable.GetEnumerator());
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

        private sealed class EnumeratorWrapper {
            private readonly IEnumerator<T> m_enumerator;
            private bool m_initialized;
            private bool m_hasCurrentItem;
            internal EnumeratorWrapper(IEnumerator<T> enumerator) {
                m_enumerator = enumerator;
                m_initialized = false;
            }

            public void MoveNext() =>
                m_hasCurrentItem = m_enumerator.MoveNext();
            public T Current => m_enumerator.Current;

            public bool HasCurrentItem() {
                if (!m_initialized)
                {
                    MoveNext();
                    m_initialized = true;
                }
                return m_hasCurrentItem;
            }
        }
    }
}