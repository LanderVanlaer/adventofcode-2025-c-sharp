using System.Text.RegularExpressions;

using StreamReader file = new(args[0]);

Regex regex = WhiteSpaceRegex();

(long, long)[] items = regex
	.Split(await file.ReadLineAsync() ?? throw new InvalidOperationException("Input file is empty"))
	.Select(long.Parse)
	.Select(i => (i, i))
	.ToArray();

while (await file.ReadLineAsync() is { } line)
{
	if (line.AsSpan(..10).Contains('*') || line.AsSpan(..10).Contains('+'))
	{
		long sum = regex
			.Split(line)
			.Zip(items)
			.Select(tuple =>
				tuple.First switch
				{
					"+" => tuple.Second.Item1,
					"*" => tuple.Second.Item2,
					_ => throw new InvalidOperationException($"Unexpected character: {tuple.First}"),
				}
			)
			.Sum();

		Console.WriteLine(sum);

		continue;
	}

	items = regex
		.Split(line)
		.Where(s => !string.IsNullOrWhiteSpace(s))
		.Select(long.Parse)
		.Zip(items)
		.Select(tuple => (tuple.Second.Item1 + tuple.First, tuple.Second.Item2 * tuple.First))
		.ToArray();
}

return 0;

internal static partial class Program
{
	[GeneratedRegex(@"\s+")]
	private static partial Regex WhiteSpaceRegex();
}
