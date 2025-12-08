using _04_2;

using StreamReader file = new(args[0]);

Grid grid = Grid.Parse(await file.ReadToEndAsync());

int counter = 0;

while (counter != (counter += grid.RemoveItemsWithLessThanFourNeighbors())) { }

Console.WriteLine(counter);

return 0;
