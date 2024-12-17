using System.Diagnostics;
using Shared;

//string[] puzzleInput = File.ReadAllLines("Sample.txt");
//Vector2l roomDimensions = new(11, 7);

string[] puzzleInput = File.ReadAllLines("PuzzleInput.txt");
Vector2l roomDimensions = new (101, 103);

Vector2l[] locations = new Vector2l[puzzleInput.Length];
Vector2l[] velocities = new Vector2l[puzzleInput.Length];

for (int i = 0; i < puzzleInput.Length; i++)
{
    string line = puzzleInput[i];
    string[] parts = line.Split(' ');
    Debug.Assert(parts.Length == 2);

    string positionString = parts[0][(parts[0].IndexOf('=') + 1)..];
    string[] positionParts = positionString.Split(',');
    locations[i] = new Vector2l(long.Parse(positionParts[0]), long.Parse(positionParts[1]));

    string velocityString = parts[1][(parts[1].IndexOf('=') + 1)..];
    string[] velocityParts = velocityString.Split(',');
    velocities[i] = new Vector2l(long.Parse(velocityParts[0]), long.Parse(velocityParts[1]));
}

// Part 2
int step = 0;
while (true)
{
    for (long x = 0; x < roomDimensions.X; x++)
    {
        int numMatchingX = locations.Count(location => location.X == x);
        if (numMatchingX > roomDimensions.X / 3)
        {
            Console.WriteLine($"Step: {step}");
            DrawLocations();
            Console.WriteLine();
            Console.ReadLine();
        }
    }
    
    //Console.WriteLine();
    //Console.WriteLine($"Step: {step}");
    //Console.ReadLine();
    StepPositions(1);
    step++;
}

StepPositions(100);

Vector2l quadrantSize = new((roomDimensions.X - 1) / 2, (roomDimensions.Y - 1) / 2);
Vector2l[] quadrantLocations =
[
    new(0, 0),
    new(quadrantSize.X + 1, 0),
    new(0, quadrantSize.Y + 1),
    new(quadrantSize.X + 1, quadrantSize.Y + 1)
];
long[] locationsInQuadrants = new long[4];
for (int i = 0; i < 4; i++)
{
    locationsInQuadrants[i] = locations.Count(x =>
        x.X >= quadrantLocations[i].X && x.X < quadrantLocations[i].X + quadrantSize.X &&
        x.Y >= quadrantLocations[i].Y && x.Y < quadrantLocations[i].Y + quadrantSize.Y);
}

long safetyFactor = locationsInQuadrants.Aggregate<long, long>(1, (current, locationCount) => current * locationCount);
Console.WriteLine($"Safety Factor: {safetyFactor}");
return;

void StepPositions(int count)
{
    for (int i = 0; i < locations.Length; i++)
    {
        long totalXMovement = velocities[i].X * count;
        locations[i].X += totalXMovement;
        if (locations[i].X > 0)
        {
            locations[i].X %= roomDimensions.X;
        }
        else
        {
            while (locations[i].X < 0)
            {
                locations[i].X += roomDimensions.X;
            }
        }

        long totalYMovement = velocities[i].Y * count;
        locations[i].Y += totalYMovement;
        if (locations[i].Y > 0)
        {
            locations[i].Y %= roomDimensions.Y;
        }
        else
        {
            while (locations[i].Y < 0)
            {
                locations[i].Y += roomDimensions.Y;
            }
        }
    }
}

void DrawLocations()
{
    for (long y = 0; y < roomDimensions.Y; y++)
    {
        for (long x = 0; x < roomDimensions.X; x++)
        {
            Console.Write(locations.Any(location => location.X == x && location.Y == y) ? '#' : '.');
        }
        Console.WriteLine();
    }
}