using Shared;

//string[] puzzleInput = File.ReadAllLines("Sample.txt");
string[] puzzleInput = File.ReadAllLines("PuzzleInput.txt");

Dictionary<Vector2i, uint> PositionToHeight = [];

for (int y = 0; y < puzzleInput.Length; y++)
{
    string line = puzzleInput[y];
    for (int x = 0; x < line.Length; x++)
    {
        uint height = uint.Parse(line[x].ToString());
        PositionToHeight.Add(new Vector2i(x, y), height);
    }
}

uint numberOfTrails = 0;
uint totalRating = 0;
foreach (KeyValuePair<Vector2i, uint> kvp in PositionToHeight)
{
    if (kvp.Value == 0)
    {
        HashSet<Vector2i> TrailEnds = [];
        uint trailRating = 0;
        RecursiveFindTrail(kvp.Key, 0, ref TrailEnds, ref trailRating);
        numberOfTrails += (uint)TrailEnds.Count;
        totalRating += trailRating;
    }
}

Console.WriteLine($"Number of trails: {numberOfTrails}");
Console.WriteLine($"Total rating: {totalRating}");
void RecursiveFindTrail(Vector2i startingPosition, uint currentHeight, ref HashSet<Vector2i> TrailEnds, ref uint trailRating)
{
    if (currentHeight == 9)
    {
        TrailEnds.Add(startingPosition);
        trailRating++;
        return;
    }
    
    Vector2i[] directions =
    [
        new(0, 1),
        new(1, 0),
        new(0, -1),
        new(-1, 0)
    ];
    
    foreach (Vector2i direction in directions)
    {
        Vector2i newPosition = new Vector2i
            ( direction.X + startingPosition.X, direction.Y + startingPosition.Y );

        if (PositionToHeight.TryGetValue(newPosition, out uint nextHeight))
        {
            if (nextHeight == currentHeight + 1)
            {
                RecursiveFindTrail(newPosition, nextHeight, ref TrailEnds, ref trailRating);
            }
        }
    }
}
