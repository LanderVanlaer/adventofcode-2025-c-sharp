using StreamReader fileStream = new(args[0]);

Dictionary<(int, int), long> cache = [];

string[] file = (await fileStream.ReadToEndAsync()).Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

Console.WriteLine(1 + Calculate(1, file[0].IndexOf('S')));

return 0;

long Calculate(int lineNumber, int index)
{
	if (lineNumber >= file.Length)
	{
		return 0;
	}

	if (cache.TryGetValue((lineNumber, index), out long value))
	{
		return value;
	}

	if (file[lineNumber][index] == '^')
	{
		long c = 1;
		c += Calculate(lineNumber + 1, index - 1);
		c += Calculate(lineNumber + 1, index + 1);
		cache[(lineNumber, index)] = c;
		return c;
	}
	else
	{
		long c = Calculate(lineNumber + 1, index);
		cache[(lineNumber, index)] = c;
		return c;
	}
}
