using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("csharpsquid", "S1199")]
[assembly: SuppressMessage("csharpsquid", "S3776")]

using StreamReader file = new(args[0]);
string line = await file.ReadToEndAsync();

{
	Stopwatch stopwatch = Stopwatch.StartNew();
	long counter = 0;

	foreach ((long start, long end) in ExtractRanges(line.AsMemory()))
	{
		for (long i = start; i <= end; i++)
		{
			if (10 < i && IsMadeOutOfSequences(i))
			{
				counter += i;
			}
		}
	}

	Console.Write("Modulo: ");
	Console.WriteLine(stopwatch.ElapsedMilliseconds);
	Console.WriteLine(counter);
	stopwatch.Stop();
}

{
	Stopwatch stopwatch = Stopwatch.StartNew();
	long counter = 0;

	foreach ((long start, long end) in ExtractRanges(line.AsMemory()))
	{
		for (long i = start; i <= end; i++)
		{
			if (10 < i && IsMadeOutOfSequencesSpan(i.ToString().AsSpan()))
			{
				counter += i;
			}
		}
	}

	Console.Write("Spans: ");
	Console.WriteLine(stopwatch.ElapsedMilliseconds);
	Console.WriteLine(counter);
	stopwatch.Stop();
}

return 0;

IEnumerable<(long start, long end)> ExtractRanges(ReadOnlyMemory<char> s)
{
	ReadOnlyMemory<char> remaining = s;

	while (remaining.Length > 0)
	{
		// 1231-3213,6432-78646
		int indexOfDash = remaining.Span.IndexOf('-');
		ReadOnlySpan<char> left = remaining[..indexOfDash].Span;
		long start = long.Parse(left);

		remaining = remaining[(indexOfDash + 1)..];

		int indexOfComma = remaining.Span.IndexOf(',');
		ReadOnlySpan<char> right = remaining[indexOfComma == -1 ? .. : ..indexOfComma].Span;
		long end = long.Parse(right);

		remaining = remaining[indexOfComma == -1 ? remaining.Length.. : (indexOfComma + 1)..];

		yield return (start, end);
	}
}

bool IsMadeOutOfSequences(long number)
{
	int length = (int)Math.Log10(number) + 1;
	int maxSize = length / 2;

	if (Same(number, 1, length))
	{
		return true;
	}

	for (int i = length % 2 + 2; i <= maxSize; ++i)
	{
		if (length % i != 0)
		{
			continue;
		}

		if (Same(number, i, length))
		{
			return true;
		}
	}

	return false;

	static bool Same(long number, int size, int length)
	{
		long powSize = (long)Math.Pow(10, size);
		long lastChunk = number % powSize;

		for (int i = 1; i < length / size; i++)
		{
			long chunk = number / (long)Math.Pow(10, size * i) % powSize;

			if (lastChunk != chunk)
			{
				return false;
			}
		}

		return true;
	}
}

bool IsMadeOutOfSequencesSpan(ReadOnlySpan<char> s)
{
	int max = s.Length / 2;

	if (Same(s, 1))
	{
		return true;
	}

	for (int i = s.Length % 2 + 2; i <= max; i += 1)
	{
		if (s.Length % i != 0)
		{
			continue;
		}

		if (Same(s, i))
		{
			return true;
		}
	}

	return false;

	static bool Same(ReadOnlySpan<char> s, int size)
	{
		ReadOnlySpan<char> part = s[..size];
		bool same = true;

		for (int j = size; same && j < s.Length; j += size)
		{
			if (!part.Equals(s.Slice(j, size), StringComparison.Ordinal))
			{
				same = false;
			}
		}

		return same;
	}
}
