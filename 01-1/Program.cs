using _01_1;

using StreamReader file = new(args[0]);

Dial dial = new();
int counter = 0;

while (await file.ReadLineAsync() is { } line)
{
	dial.Add(line);

	if (dial.Value == 0)
	{
		++counter;
	}
}

Console.WriteLine(counter);

return 0;
