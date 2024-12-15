using Shared;

//string[] puzzleInput = File.ReadAllLines("Sample.txt");
string[] puzzleInput = File.ReadAllLines("PuzzleInput.txt");

Dictionary<Vector2i, char> map = [];
for (int y = 0; y < puzzleInput.Length; y++)
{
    for (int x = 0; x < puzzleInput[y].Length; x++)
    {
        map.Add(new Vector2i(x, y), puzzleInput[y][x]);
    }
}

ulong totalValue = 0;
ulong totalValueUsingSides = 0;
HashSet<Vector2i> visited = [];
foreach (KeyValuePair<Vector2i, char> kvp in map)
{
    if (visited.Contains(kvp.Key)) continue;
    
    HashSet<Vector2i> regionSpaces = [kvp.Key];
    GatherRegionSpaces(kvp.Key, kvp.Value, ref regionSpaces);
    visited.UnionWith(regionSpaces);

    Dictionary<Vector2i, List<Direction>> regionsWithSides = [];

    uint area = (uint)regionSpaces.Count;
    GetRegionPerimeter(regionSpaces, ref regionsWithSides, kvp.Value);
    uint sides = GetRegionSides(regionsWithSides, kvp.Value);
    
    uint perimeterCount = regionsWithSides.Values.Aggregate<List<Direction>, uint>(0, (current, directions) => current + (uint)directions.Count);

    totalValue += (area * perimeterCount);
    totalValueUsingSides += (area * sides);
}

Console.WriteLine($"Total Value: {totalValue}");
Console.WriteLine($"Total Value Using Sides: {totalValueUsingSides}");

return;

void GatherRegionSpaces(Vector2i position, char id, ref HashSet<Vector2i> regionSpaces)
{
    Vector2i[] adjacentPositions =
    [
        new (position.X, position.Y - 1),
        new (position.X, position.Y + 1),
        new (position.X - 1, position.Y),
        new (position.X + 1, position.Y) 
    ];
    
    foreach (Vector2i adjacentPosition in adjacentPositions)
    {
        if (regionSpaces.Contains(adjacentPosition)) continue;
        if (!map.TryGetValue(adjacentPosition, out char adjacentId)) continue;
        if (adjacentId != id) continue;
        regionSpaces.Add(adjacentPosition);
        GatherRegionSpaces(adjacentPosition, adjacentId, ref regionSpaces);
    }
}

void GetRegionPerimeter(HashSet<Vector2i> regionSpaces, ref Dictionary<Vector2i, List<Direction>> regionSpacesWithSides, char regionId)
{
    foreach (Vector2i regionSpace in regionSpaces)
    {
        regionSpacesWithSides.Add(regionSpace, []);
        Vector2i[] adjacentPositions =
        [
            new (regionSpace.X, regionSpace.Y - 1), // north
            new (regionSpace.X + 1, regionSpace.Y), // east
            new (regionSpace.X, regionSpace.Y + 1), // south
            new (regionSpace.X - 1, regionSpace.Y) // west
        ];

        for (int index = 0; index < adjacentPositions.Length; index++)
        {
            Vector2i adjacentPosition = adjacentPositions[index];
            if (map.TryGetValue(adjacentPosition, out char adjacentId) && adjacentId == regionId) continue;
            regionSpacesWithSides[regionSpace].Add((Direction)index);
        }
    }
}

uint GetNorthOrSouthFacingSides(List<Vector2i> sortedRegions)
{
    uint sides = 0;
    int currentY = -1;
    for (int i = 0; i < sortedRegions.Count; i++)
    {
        Vector2i firstRegionOfLine = sortedRegions[i];
        if (firstRegionOfLine.Y != currentY)
        {
            // Starting a new line, so we know we have at least one side
            sides++;
            currentY = firstRegionOfLine.Y;
            int lastX = firstRegionOfLine.X;
            
            // Keep going until we go through the entire line
            for (int j = i + 1; j < (sortedRegions.Count) && (sortedRegions[j].Y == currentY); j++)
            {
                if (Math.Abs(sortedRegions[j].X - lastX) > 1)
                {
                    ++sides;
                }
                lastX = sortedRegions[j].X;
                i = j;
            }
        }
    }

    return sides;
}

uint GetEastOrWestFacingSides(List<Vector2i> sortedRegions)
{
    uint sides = 0;
    int currentX = -1;
    for (int i = 0; i < sortedRegions.Count; i++)
    {
        Vector2i firstRegionOfLine = sortedRegions[i];
        if (firstRegionOfLine.X != currentX)
        {
            // Starting a new line, so we know we have at least one side
            sides++;
            currentX = firstRegionOfLine.X;
            int lastY = firstRegionOfLine.Y;
            
            // Keep going until we go through the entire line
            for (int j = i + 1; j < (sortedRegions.Count) && (sortedRegions[j].X == currentX); j++)
            {
                if (Math.Abs(sortedRegions[j].Y - lastY) > 1)
                {
                    ++sides;
                }
                lastY = sortedRegions[j].Y;
                i = j;
            }
        }
    }

    return sides;
}

uint GetRegionSides(Dictionary<Vector2i, List<Direction>> regionSpacesWithSides, char regionId)
{
    uint sides = 0;

    List<Vector2i> regionsWithNorthSides = regionSpacesWithSides.Where(kvp => kvp.Value.Contains(Direction.North)).Select(kvp => kvp.Key).ToList();
    regionsWithNorthSides.Sort((a, b) => a.Y == b.Y ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));
    List<Vector2i> regionsWithSouthSides = regionSpacesWithSides.Where(kvp => kvp.Value.Contains(Direction.South)).Select(kvp => kvp.Key).ToList();
    regionsWithSouthSides.Sort((a, b) => a.Y == b.Y ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));
    
    List<Vector2i> regionsWithEastSides = regionSpacesWithSides.Where(kvp => kvp.Value.Contains(Direction.East)).Select(kvp => kvp.Key).ToList();
    regionsWithEastSides.Sort((a, b) => a.X == b.X ? a.Y.CompareTo(b.Y) : a.X.CompareTo(b.X));
    List<Vector2i> regionsWithWestSides = regionSpacesWithSides.Where(kvp => kvp.Value.Contains(Direction.West)).Select(kvp => kvp.Key).ToList();
    regionsWithWestSides.Sort((a, b) => a.X == b.X ? a.Y.CompareTo(b.Y) : a.X.CompareTo(b.X));

    uint northSides = GetNorthOrSouthFacingSides(regionsWithNorthSides);
    uint southSides = GetNorthOrSouthFacingSides(regionsWithSouthSides);
    uint eastSides = GetEastOrWestFacingSides(regionsWithEastSides);
    uint westSides = GetEastOrWestFacingSides(regionsWithWestSides);
    sides = northSides + southSides + eastSides + westSides;
    
    Console.WriteLine($"Region: {regionId} has number of sides: {sides}");
    return sides;
}
