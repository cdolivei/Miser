namespace Miser;

internal sealed class EnumeratorWrapper<T> {
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