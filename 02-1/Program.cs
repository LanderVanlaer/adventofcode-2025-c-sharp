using StreamReader file = new(args[0]);

long counter = 0;

string line = await file.ReadToEndAsync();

foreach ((long start, long end) in ExtractRanges(line.AsMemory()))
{
	for (long i = start; i <= end; i++)
	{
		int length = (int)Math.Log10(i) + 1;

		if (length % 2 == 1)
		{
			continue;
		}

		int half = length / 2;
		long left = i / (int)Math.Pow(10, half);
		long right = i % (int)Math.Pow(10, half);

		if (left == right)
		{
			counter += i;
		}
	}
}

Console.WriteLine(counter);

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
