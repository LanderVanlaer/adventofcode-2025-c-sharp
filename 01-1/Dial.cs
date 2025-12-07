namespace _01_1;

internal struct Dial()
{
	/// <summary>
	///     The maximum dial value (exclusive)
	/// </summary>
	private const int Max = 100;

	public int Value { get; private set; } = 50;

	public void Add(int delta)
	{
		Value = (Value + delta) % Max;
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
