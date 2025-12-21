using System.Collections;
using System.Diagnostics;

using StreamReader file = new(args[0]);

int count = 0;

while (await file.ReadLineAsync() is { } line)
{
	ReadOnlySpan<char> lightsStr = line.AsSpan(1..line.IndexOf(']'));
	BitArray desired = new(lightsStr.Length);

	for (int i = 0; i < lightsStr.Length; ++i)
	{
		desired[i] = lightsStr[i] == '#';
	}

	ReadOnlySpan<char> possibilitiesStr = line.AsSpan((line.IndexOf('(') + 1)..(line.IndexOf('{') + 1));

	int[][] possibilities = new int[possibilitiesStr.Count(')')][];

	for (int i = 0; i < possibilities.Length; ++i)
	{
		int index = possibilitiesStr.IndexOf(')');

		Debug.Assert(index != -1);

		possibilities[i] = possibilitiesStr[..index].ToString().Split(',').Select(int.Parse).ToArray();
		possibilitiesStr = possibilitiesStr[(index + 3)..];
	}

	BitArray actual = new(desired.Length);

	// Test every possible combination of possibilities
	int fewest = int.MaxValue;

	foreach (int[] combination in GetPossibilities(possibilities.Length))
	{
		actual.SetAll(false);

		foreach (int possibilityIndex in combination)
		{
			foreach (int i in possibilities[possibilityIndex])
			{
				actual[i] = !actual[i];
			}
		}

		if (BitArrayEquals(actual, desired))
		{
			fewest = Math.Min(fewest, combination.Length);
		}
	}

	count += fewest;
}

Console.WriteLine(count);

return 0;

IEnumerable<int[]> GetPossibilities(int length)
{
	Debug.Assert(sizeof(int) * 8 >= length);

	for (int i = 0; i < Math.Pow(2, length); ++i)
	{
		yield return Enumerable.Range(0, length).Where(j => ((i >> j) & 1) == 1).ToArray();
	}
}

bool BitArrayEquals(BitArray left, BitArray right)
{
	return left.Cast<bool>().SequenceEqual(right.Cast<bool>());
}
