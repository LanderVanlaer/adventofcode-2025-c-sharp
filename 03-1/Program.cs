using StreamReader file = new(args[0]);

long totalJoltage = 0;

while (await file.ReadLineAsync() is { } bank)
{
	(Index firstBattery, Index lastBattery) = FindHighestJoltage(bank.AsSpan());

	int joltageOfBank = (bank[firstBattery] - '0') * 10 + (bank[lastBattery] - '0');

	totalJoltage += joltageOfBank;
}

Console.WriteLine(totalJoltage);

return 0;

(Index firstBattery, Index lastBattery) FindHighestJoltage(ReadOnlySpan<char> bank)
{
	int firstBatteryIndex = 0;

	for (int i = 1; i < bank.Length - 1; i++)
	{
		if (bank[i] > bank[firstBatteryIndex])
		{
			firstBatteryIndex = i;
		}
	}

	int lastBatteryIndex = firstBatteryIndex + 1;

	for (int i = lastBatteryIndex + 1; i < bank.Length; i++)
	{
		if (bank[i] > bank[lastBatteryIndex])
		{
			lastBatteryIndex = i;
		}
	}

	return (firstBatteryIndex, lastBatteryIndex);
}
