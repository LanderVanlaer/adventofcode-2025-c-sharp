using _04_1;

using StreamReader file = new(args[0]);

Grid grid = Grid.Parse(await file.ReadToEndAsync());

Console.WriteLine(grid.CountItemsWithLessThanFourNeighbors());

return 0;
