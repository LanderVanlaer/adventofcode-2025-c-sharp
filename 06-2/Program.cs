using StreamReader file = new(args[0]);

string input = await file.ReadToEndAsync();
string[] lines = input.Split("\n").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

char[][] matrix = new char[lines.Length][];

for (int i = 0; i < lines.Length; i++)
{
	matrix[i] = lines[i].ToCharArray();
}

matrix = matrix.Transpose();

long total = 0;

for (int i = 0; i < matrix.Length; ++i)
{
	char type = matrix[i][^1];
	long count = long.Parse(matrix[i].AsSpan()[..^1]);
	++i;

	for (; i < matrix.Length && !MatrixHelpers.IsNullOrWhiteSpace(matrix[i]); ++i)
	{
		long value = long.Parse(matrix[i].AsSpan()[..^1]);

		switch (type)
		{
			case '*':
				count *= value;
				break;
			case '+':
				count += value;
				break;
		}
	}

	total += count;
}

Console.WriteLine(total);

return 0;

internal static class MatrixHelpers
{
	public static char[][] Transpose(this char[][] matrix)
	{
		int maxLength = matrix.Max(m => m.Length);
		char[][] result = new char[maxLength][];

		for (int i = 0; i < maxLength; i++)
		{
			result[i] = new char[matrix.Length];

			for (int j = 0; j < matrix.Length; j++)
			{
				if (i >= matrix[j].Length)
				{
					result[i][j] = ' ';
				}
				else
				{
					result[i][j] = matrix[j][i];
				}
			}
		}

		return result;
	}

	public static bool IsNullOrWhiteSpace(ReadOnlySpan<char> value)
	{
		foreach (char t in value)
		{
			if (!char.IsWhiteSpace(t))
			{
				return false;
			}
		}

		return true;
	}
}
