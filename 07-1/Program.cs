using StreamReader file = new(args[0]);

string firstLine = await file.ReadLineAsync() ?? throw new InvalidOperationException("Input file is empty");
int length = firstLine.Length;

Span<char> prev = stackalloc char[length];
Span<char> curr = stackalloc char[length];

firstLine.CopyTo(prev);

prev[prev.IndexOf('S')] = '|';

int count = 0;

while (file.ReadLine() is { } line)
{
	line.CopyTo(curr);

	for (int i = 0; i < length; ++i)
	{
		if (prev[i] != '|')
		{
			continue;
		}

		switch (curr[i])
		{
			case '.':
				curr[i] = '|';
				break;
			case '^':
				++count;
				if (i != 0)
				{
					curr[i - 1] = '|';
				}

				if (i != length - 1)
				{
					curr[i + 1] = '|';
				}

				break;
		}
	}

	curr.CopyTo(prev);
}

Console.WriteLine(count);
