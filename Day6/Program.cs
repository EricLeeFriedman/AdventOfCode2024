using Day6;
using Shared;

//string[] puzzleInput = File.ReadAllLines("Sample.txt");
string[] puzzleInput = File.ReadAllLines("PuzzleInput.txt");

Vector2i guardLocation = new Vector2i { X = 0, Y = 0 };
Vector2i guardDirection = new Vector2i { X = 0, Y = -1 };

List<Vector2i> obstacles = [];
List<Vector2i> blankSpaces = [];
for (int y = 0; y < puzzleInput.Length; y++)
{
    string line = puzzleInput[y];
    for (int x = 0; x < line.Length; x++)
    {
        switch (line[x])
        {
            case '#':
                obstacles.Add(new Vector2i { X = x, Y = y });
                break;
            case '^':
                guardLocation = new Vector2i { X = x, Y = y };
                break;
            default:
                blankSpaces.Add(new Vector2i { X = x, Y = y });
                break;
        }
    }
}

void TurnDirection(ref Vector2i direction)
{
    switch (direction.X, direction.Y)
    {
        case (0, -1):
            direction.X = 1;
            direction.Y = 0;
            break;
        case (1, 0):
            direction.X = 0;
            direction.Y = 1;
            break;
        case (0, 1):
            direction.X = -1;
            direction.Y = 0;
            break;
        case (-1, 0):
            direction.X = 0;
            direction.Y = -1;
            break;
        default:
            throw new Exception("Invalid direction");
    }
}

void RunGuardSimulation(Vector2i location, Vector2i direction, List<Vector2i> obstaclesToAvoid, out int numLocationsVisited, out bool gotOut)
{
    List<GuardHistoryEntry> guardHistory = [];
    List<Vector2i> visitedLocation = [];
    while (location.X < puzzleInput[0].Length && location.X >= 0 && location.Y < puzzleInput.Length &&
           location.Y >= 0)
    {
        GuardHistoryEntry historyEntry = new GuardHistoryEntry
            { Location = location, Direction = direction };
        
        if (guardHistory.Contains(historyEntry))
        {
            gotOut = false;
            numLocationsVisited = visitedLocation.Count;
            return;
        }
        guardHistory.Add(historyEntry);
        
        if (!visitedLocation.Contains(location))
        {
            visitedLocation.Add(location);
        }

        Vector2i nextLocation = new Vector2i
            { X = location.X + direction.X, Y = location.Y + direction.Y };

        while (obstaclesToAvoid.Contains(nextLocation))
        {
            TurnDirection(ref direction);
            nextLocation = new Vector2i
                { X = location.X + direction.X, Y = location.Y + direction.Y };
        }

        location = nextLocation;
    }
    numLocationsVisited = visitedLocation.Count;
    gotOut = true;
}

{
    RunGuardSimulation(guardLocation, guardDirection, obstacles, out int placesVisited, out bool gotOut);
    Console.WriteLine($"Places visited: {placesVisited}");
}

// Part 2
int loopedCount = 0;
Parallel.ForEach(blankSpaces, blankSpace =>
{
    List<Vector2i> obstaclesCopy = new List<Vector2i>(obstacles);
    obstaclesCopy.Add(blankSpace);
    
    RunGuardSimulation(guardLocation, guardDirection, obstaclesCopy, out int placesVisited, out bool gotOut);
    Interlocked.Add(ref loopedCount, !gotOut ? 1 : 0);
});

Console.WriteLine($"Looped count: {loopedCount}");