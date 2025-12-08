namespace _04_1;

internal sealed class Grid
{
	private readonly bool[][] _grid;

	private Grid(bool[][] grid)
	{
		_grid = grid;
	}

	public static Grid Parse(string input)
	{
		int height = input.Count('\n');
		int width = input.IndexOf('\n');

		bool[][] grid = new bool[height][];

		for (int y = 0; y < height; ++y)
		{
			grid[y] = new bool[width];

			for (int i = 0; i < width; i++)
			{
				grid[y][i] = input[y * (width + 1) + i] == '@';
			}
		}

		return new Grid(grid);
	}

	public int CountItemsWithLessThanFourNeighbors()
	{
		int count = 0;

		for (int y = 0; y < _grid.Length; y++)
		{
			for (int x = 0; x < _grid[y].Length; x++)
			{
				if (!_grid[y][x])
				{
					continue;
				}

				count += GetNeighbors(y, x).Count(b => b) < 4 ? 1 : 0;
			}
		}

		return count;
	}

	private IEnumerable<bool> GetNeighbors(int y, int x)
	{
		int maxY = Math.Min(y + 1, _grid.Length - 1);
		int maxX = Math.Min(x + 1, _grid[0].Length - 1);

		for (int i = Math.Max(0, y - 1); i <= maxY; ++i)
		{
			for (int j = Math.Max(0, x - 1); j <= maxX; ++j)
			{
				if (i == y && j == x)
				{
					continue;
				}

				yield return _grid[i][j];
			}
		}
	}
}
