namespace _04_2;

internal sealed class Grid
{
	// >=0: roll is present
	//  -1: roll is not present
	private readonly int[][] _grid;

	private Grid(int[][] grid)
	{
		_grid = grid;
	}

	public static Grid Parse(string input)
	{
		int height = input.Count('\n');
		int width = input.IndexOf('\n');

		int[][] grid = new int[height][];

		for (int y = 0; y < height; ++y)
		{
			grid[y] = new int[width];

			for (int i = 0; i < width; i++)
			{
				grid[y][i] = input[y * (width + 1) + i] == '@' ? 0 : -1;
			}
		}

		for (int y = 0; y < grid.Length; y++)
		{
			for (int x = 0; x < grid[y].Length; x++)
			{
				if (grid[y][x] == -1)
				{
					continue;
				}

				grid[y][x] = GetNeighbors(grid, y, x).Count(b => b != -1);
			}
		}

		return new Grid(grid);
	}

	public int RemoveItemsWithLessThanFourNeighbors()
	{
		int count = 0;

		for (int y = 0; y < _grid.Length; y++)
		{
			for (int x = 0; x < _grid[y].Length; x++)
			{
				switch (_grid[y][x])
				{
					case -1:
						continue;
					case < 4:
						++count;
						_grid[y][x] = -1;
						SubtractSurrounding(_grid, y, x);
						break;
				}
			}
		}

		return count;
	}

	private static IEnumerable<int> GetNeighbors(int[][] grid, int y, int x)
	{
		int maxY = Math.Min(y + 1, grid.Length - 1);
		int maxX = Math.Min(x + 1, grid[0].Length - 1);

		for (int i = Math.Max(0, y - 1); i <= maxY; ++i)
		{
			for (int j = Math.Max(0, x - 1); j <= maxX; ++j)
			{
				if (i == y && j == x)
				{
					continue;
				}

				yield return grid[i][j];
			}
		}
	}

	private static void SubtractSurrounding(int[][] grid, int y, int x)
	{
		int maxY = Math.Min(y + 1, grid.Length - 1);
		int maxX = Math.Min(x + 1, grid[0].Length - 1);

		for (int i = Math.Max(0, y - 1); i <= maxY; ++i)
		{
			for (int j = Math.Max(0, x - 1); j <= maxX; ++j)
			{
				if (i == y && j == x)
				{
					continue;
				}

				if (grid[i][j] > 0)
				{
					--grid[i][j];
				}
			}
		}
	}
}
