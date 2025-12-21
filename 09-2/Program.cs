using System.Numerics;

using StreamReader file = new(args[0]);

List<Vector2> points = [];

while (await file.ReadLineAsync() is { } line)
{
	int firstComma = line.IndexOf(',');
	int x = int.Parse(line[..firstComma]);
	int y = int.Parse(line[(firstComma + 1)..]);
	points.Add(new Vector2(x, y));
}

double biggestArea = 0;

for (int i = 0; i < points.Count; ++i)
{
	for (int j = i + 1; j < points.Count; ++j)
	{
		double area =
			(Math.Abs((double)points[i].X - points[j].X) + 1) * (Math.Abs((double)points[i].Y - points[j].Y) + 1);

		if (area > biggestArea)
		{
			bool contains = false;

			// check if any point is inside this square
			for (int k = 0; k < points.Count && !contains; ++k)
			{
				if (k == i || k == j)
				{
					continue;
				}

				if (Between(points[i].X, points[k].X, points[j].X) && Between(points[i].Y, points[k].Y, points[j].Y))
				{
					contains = true;
					break;
				}
			}

			if (contains)
			{
				continue;
			}

			Vector2 leftTop = new(Math.Min(points[i].X, points[j].X), Math.Min(points[i].Y, points[j].Y));
			Vector2 rightBottom = new(Math.Max(points[i].X, points[j].X), Math.Max(points[i].Y, points[j].Y));

			// Now check if any line intersects this square
			for (int k = 0; k < points.Count && !contains; ++k)
			{
				if (k == i || k == j)
				{
					continue;
				}

				Vector2 p1 = points[k];
				Vector2 p2 = points[(k + 1) % points.Count];

				// 0x0001: top, 0x0010: right, 0x0100: bottom, 0x1000: left
				int p1IsAtSide = IsAtSideOfRect(p1, leftTop, rightBottom);
				if (HasMoreThanOneFlagDefined(p1IsAtSide))
				{
					continue;
				}

				int p2IsAtSide = IsAtSideOfRect(p2, leftTop, rightBottom);
				if (HasMoreThanOneFlagDefined(p2IsAtSide))
				{
					continue;
				}

				if (p1IsAtSide == p2IsAtSide)
				{
					continue;
				}

				contains = true;
				break;
			}

			if (contains)
			{
				continue;
			}

			biggestArea = Math.Max(area, biggestArea);
		}
	}
}

Console.WriteLine(biggestArea.ToString("0"));

return 0;

bool Between(float a, float b, float c)
{
	return (a < b && b < c) || (c < b && b < a);
}

// https://stackoverflow.com/a/56361601/13165967
bool HasMoreThanOneFlagDefined(int flags)
{
	return (flags & (flags - 1)) != 0;
}

int IsAtSideOfRect(Vector2 p, Vector2 leftTop, Vector2 rightBottom)
{
	int res = 0;

	// 0x0001: top, 0x0010: right, 0x0100: bottom, 0x1000: left
	if (p.Y <= leftTop.Y)
	{
		res |= 0x0001;
	}

	if (p.X >= rightBottom.X)
	{
		res |= 0x0010;
	}

	if (p.Y >= rightBottom.Y)
	{
		res |= 0x0100;
	}

	if (p.X <= leftTop.X)
	{
		res |= 0x1000;
	}

	return res;
}
