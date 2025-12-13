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

		biggestArea = Math.Max(area, biggestArea);
	}
}

Console.WriteLine(biggestArea.ToString("0"));
