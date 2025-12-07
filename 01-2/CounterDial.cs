namespace _01_2;

internal struct CounterDial()
{
	/// <summary>
	///     The maximum dial value (exclusive)
	/// </summary>
	private const int Max = 100;

	public int Value { get; private set; } = 50;
	public int Count { get; private set; } = 0;

	public void Add(int delta)
	{
		int deltaValue = Value + delta;

		if (Value != 0 && deltaValue <= 0)
		{
			++Count;
		}

		Count += (int)(Math.Abs(deltaValue) / (double)Max);
		Value = (deltaValue % Max + Max) % Max; // https://stackoverflow.com/a/1082938/13165967
	}

	public void Add(ReadOnlySpan<char> delta)
	{
		int deltaInt = int.Parse(delta[1..]);

		switch (delta[0])
		{
			case 'L':
				Add(-deltaInt);
				break;
			case 'R':
				Add(+deltaInt);
				break;
		}
	}
}
