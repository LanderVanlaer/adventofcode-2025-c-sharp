using StreamReader file = new(args[0]);

const int size = 12;

long totalJoltage = 0;

while (await file.ReadLineAsync() is { } bank)
{
	Index[] indices = FindHighestJoltage(bank.AsSpan(), size);

	for (int i = 0; i < size; ++i)
	{
		totalJoltage += (bank[indices[i]] - '0') * (long)Math.Pow(10, size - i - 1);
	}
}

Console.WriteLine(totalJoltage);

return 0;

static Index[] FindHighestJoltage(ReadOnlySpan<char> bank, int total)
{
	Index[] indices = new Index[total];
	int startFrom = 0;

	for (int i = 0; i < total; ++i)
	{
		int maxIndex = total - i - 1;

		int index = startFrom + IndexOfHighestValue(bank[startFrom..^maxIndex]);
		indices[i] = index;
		startFrom = index + 1;
	}

	return indices;
}

static int IndexOfHighestValue(ReadOnlySpan<char> list)
{
	int index = 0;

	for (int i = 1; i < list.Length; ++i)
	{
		if (list[i] > list[index])
		{
			index = i;
		}
	}

	return index;
}
