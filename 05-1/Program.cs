using System.Diagnostics.CodeAnalysis;

using StreamReader file = new(args[0]);

(long, long)[] ranges = await ExtractRanges(file).ToArrayAsync();

int counter = 0;

while (await file.ReadLineAsync() is { } idStr)
{
	long id = long.Parse(idStr);

	if (ranges.Any(range => range.Contains(id)))
	{
		++counter;
	}
}

Console.WriteLine(counter);

return 0;

async IAsyncEnumerable<(long, long)> ExtractRanges(StreamReader reader)
{
	while (await reader.ReadLineAsync() is { } line && !string.IsNullOrWhiteSpace(line))
	{
		int indexOfDash = line.IndexOf('-');
		long left = long.Parse(line.AsSpan(..indexOfDash));
		long right = long.Parse(line[(indexOfDash + 1)..]);

		yield return (left, right);
	}
}

[SuppressMessage("csharpsquid", "S3903")]
internal static class RangeHelper
{
	extension((long Left, long Right) @this)
	{
		internal bool Contains(long x)
		{
			return @this.Left <= x && x <= @this.Right;
		}
	}
}
