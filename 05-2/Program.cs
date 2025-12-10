using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using StreamReader file = new(args[0]);

LinkedList<(long Left, long Right)> ranges = new(await ExtractRanges(file).ToListAsync());

LinkedListNode<(long Left, long Right)>? item = ranges.First;

while (item is not null)
{
	LinkedListNode<(long Left, long Right)>? nextItem = item.Next;

	bool changed = false;

	while (nextItem is not null)
	{
		if (item.Value.Contains(nextItem.Value.Right) && nextItem.Value.Left <= item.Value.Left)
		{
			ranges.Remove(nextItem);
			item.Value = (nextItem.Value.Left, item.Value.Right);
			changed = true;
		}
		else if (item.Value.Contains(nextItem.Value.Left) && nextItem.Value.Right >= item.Value.Right)
		{
			ranges.Remove(nextItem);
			item.Value = (item.Value.Left, nextItem.Value.Right);
			changed = true;
		}
		else if (nextItem.Value.Left >= item.Value.Left && nextItem.Value.Right <= item.Value.Right)
		{
			ranges.Remove(nextItem);
			changed = true;
		}
		else if (nextItem.Value.Left <= item.Value.Left && nextItem.Value.Right >= item.Value.Right)
		{
			ranges.Remove(nextItem);
			item.Value = (nextItem.Value.Left, nextItem.Value.Right);
			changed = true;
		}
		// else skip

		nextItem = nextItem.Next;
	}

	if (!changed)
	{
		item = item.Next;
	}
}

Console.WriteLine(ranges.Sum(range => range.Right - range.Left + 1));

return 0;

async IAsyncEnumerable<(long, long)> ExtractRanges(StreamReader reader)
{
	while (await reader.ReadLineAsync() is { } line && !string.IsNullOrWhiteSpace(line))
	{
		int indexOfDash = line.IndexOf('-');
		long left = long.Parse(line.AsSpan(..indexOfDash));
		long right = long.Parse(line[(indexOfDash + 1)..]);

		Debug.Assert(left <= right);

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
