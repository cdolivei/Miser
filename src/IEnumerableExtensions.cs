namespace Miser;

public static class IEnumerableExtensions {
	public static IForwardOnlyReader<T> ToForwardOnlyReader<T>(this IEnumerable<T> values) {
		return new ForwardOnlyReader<T>(values);
	}
}