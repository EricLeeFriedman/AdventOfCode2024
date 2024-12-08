using Shared;

//string[] puzzleInput = File.ReadAllLines("Sample.txt");
string[] puzzleInput = File.ReadAllLines("PuzzleInput.txt");

Dictionary<char, List<Vector2i>> antennaPositionsByLetter = new();
for (int y = 0; y < puzzleInput.Length; y++)
{
    string line = puzzleInput[y];
    for (int x = 0; x < line.Length; x++)
    {
        char c = line[x];
        if (c == '.') continue;
        
        if (!antennaPositionsByLetter.TryGetValue(c, out List<Vector2i>? positions))
        {
            positions = [];
            antennaPositionsByLetter.Add(c, positions);
        }
        positions.Add(new Vector2i { X = x, Y = y});
        antennaPositionsByLetter[c] = positions;
    }
}

int GetMagnitude(int val1, int val2)
{
    if (val1 == val2) return 0;
    return val1 < val2 ? -1 : 1;
}

bool IsPositionValid(Vector2i position)
{
    return position.X >= 0 && position.X < puzzleInput[0].Length && position.Y >= 0 && position.Y < puzzleInput.Length;
}

HashSet<Vector2i> antinodePositions = [];
HashSet<Vector2i> antinodePositionsWithTFrequency = [];

foreach (var kvp in antennaPositionsByLetter)
{
    List<Vector2i> positions = kvp.Value;
    for (int i = 0; i < positions.Count; i++)
    {
        Vector2i position = positions[i];
        for (int j = i + 1; j < positions.Count; j++)
        {
            Vector2i otherPosition = positions[j];
            int xDistance = Math.Abs(otherPosition.X - position.X);
            int yDistance = Math.Abs(otherPosition.Y - position.Y);
            
            int x1Magnitude = GetMagnitude(position.X, otherPosition.X);
            int y1Magnitude = GetMagnitude(position.Y, otherPosition.Y);
            int x2Magnitude = GetMagnitude(otherPosition.X, position.X);
            int y2Magnitude = GetMagnitude(otherPosition.Y, position.Y);
            
            int x1 = position.X + (x1Magnitude * xDistance);
            int y1 = position.Y + (y1Magnitude * yDistance);
            int x2 = otherPosition.X + (x2Magnitude * xDistance);
            int y2 = otherPosition.Y + (y2Magnitude * yDistance);
            
            Vector2i potentialPosition1 = new Vector2i{X = x1, Y = y1};
            if (IsPositionValid(potentialPosition1))
            {
                antinodePositions.Add(potentialPosition1);
            }
            Vector2i potentialPosition2 = new Vector2i { X = x2, Y = y2 };
            if (IsPositionValid(potentialPosition2))
            {
                antinodePositions.Add(potentialPosition2);
            }
            
            antinodePositionsWithTFrequency.Add(position);
            antinodePositionsWithTFrequency.Add(otherPosition);
            while (IsPositionValid(potentialPosition1))
            {
                antinodePositionsWithTFrequency.Add(potentialPosition1);
                potentialPosition1.X += (x1Magnitude * xDistance);
                potentialPosition1.Y += (y1Magnitude * yDistance);
            }

            while (IsPositionValid(potentialPosition2))
            {
                antinodePositionsWithTFrequency.Add(potentialPosition2);
                potentialPosition2.X += (x2Magnitude * xDistance);
                potentialPosition2.Y += (y2Magnitude * yDistance);
            }
        }
    }
}

Console.WriteLine($"Number of antinode positions: {antinodePositions.Count}");
Console.WriteLine($"Number of antinode positions with T frequency: {antinodePositionsWithTFrequency.Count}");