using System.Numerics;
using Connection = (System.Numerics.Vector3, System.Numerics.Vector3);
using Circuit = System.Collections.Generic.HashSet<System.Numerics.Vector3>;

using StreamReader file = new(args[0]);

List<Vector3> points = [];
List<Circuit> circuits = [];
HashSet<Connection> connections = [];

while (await file.ReadLineAsync() is { } line)
{
	int firstComma = line.IndexOf(',');
	int secondComma = line.IndexOf(',', firstComma + 1);
	int x = int.Parse(line[..firstComma]);
	int y = int.Parse(line[(firstComma + 1)..secondComma]);
	int z = int.Parse(line[(secondComma + 1)..]);

	points.Add(new Vector3(x, y, z));
}

for (int i = 0; i < int.Parse(args[1]); ++i)
{
	FindClosestTogether();
}

int outcome = circuits
	.Select(circuit => circuit.Count)
	.OrderByDescending(count => count)
	.Take(3)
	.Aggregate((a, b) => a * b);

Console.WriteLine(outcome);

return 0;

void FindClosestTogether()
{
	Vector3 closest1 = default;
	Vector3 closest2 = default;
	float distance = int.MaxValue;

	for (int i = 0; i < points.Count; i++)
	{
		Vector3 point = points[i];

		for (int j = i + 1; j < points.Count; j++)
		{
			float currentDistance = Vector3.DistanceSquared(point, points[j]);

			if (currentDistance > distance)
			{
				continue;
			}

			if (connections.Contains((point, points[j])) || connections.Contains((points[j], point)))
			{
				continue;
			}

			closest1 = point;
			closest2 = points[j];
			distance = currentDistance;
		}
	}

	connections.Add((closest1, closest2));

	Circuit? closest1Circuit = circuits.FirstOrDefault(circuit => circuit.Contains(closest1));
	Circuit? closest2Circuit = circuits.FirstOrDefault(circuit => circuit.Contains(closest2));

	if (closest1Circuit is not null && closest2Circuit is not null)
	{
		if (closest1Circuit == closest2Circuit)
		{
			return;
		}

		// combine the circuits
		closest1Circuit.UnionWith(closest2Circuit);
		circuits.Remove(closest2Circuit);
	}
	else if (closest1Circuit is not null)
	{
		closest1Circuit.Add(closest2);
	}
	else if (closest2Circuit is not null)
	{
		closest2Circuit.Add(closest1);
	}
	else
	{
		// new circuit
		circuits.Add([closest1, closest2]);
	}
}
