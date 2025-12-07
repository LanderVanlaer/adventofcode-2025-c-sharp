using _01_2;

using StreamReader file = new(args[0]);

CounterDial dial = new();

while (await file.ReadLineAsync() is { } line)
{
	dial.Add(line);
}

Console.WriteLine(dial.Count);

return 0;
