//string[] puzzleInput = File.ReadAllLines("Sample.txt");
string[] puzzleInput = File.ReadAllLines("PuzzleInput.txt");

char[] xmas = ['X', 'M', 'A', 'S'];
char[] mas = ['M', 'A', 'S'];

char[][] grid = new char[puzzleInput.Length][];
for (int i = 0; i < puzzleInput.Length; i++)
{
    grid[i] = puzzleInput[i].ToCharArray();
}

bool IsTestWordAtLocationAndDirection(int xDirection, int yDirection, int startingX, int startingY, char[] test)
{
    int testIndex = 1;
    int nextX = startingX + xDirection;
    int nextY = startingY + yDirection;
    while (nextY >= 0 && nextY < grid.Length && nextX >= 0 && nextX < grid[nextY].Length)
    {
        if (grid[nextY][nextX] == test[testIndex])
        {
            testIndex++;
            if (testIndex == test.Length)
            {
                return true;
            }
        }
        else
        {
            break;
        }
        
        nextX += xDirection;
        nextY += yDirection;
    }
    
    return false;
}

int GetXmasCount(int startingY, int startingX)
{
    int count = 0;
    for (int xDirection = -1; xDirection <= 1; xDirection++)
    {
        for (int yDirection = -1; yDirection <= 1; yDirection++)
        {
            if (xDirection == 0 && yDirection == 0)
            {
                continue;
            }
            count += IsTestWordAtLocationAndDirection(xDirection, yDirection, startingX, startingY, xmas) ? 1 : 0;
        }
    }
    return count;
}

Dictionary<Tuple<int, int>, int> MasCenterLocationToCount = [];

void AddMasCenterLocation(int x, int y)
{
    Tuple<int, int> centerLocation = new Tuple<int, int>(x, y);
    if (!MasCenterLocationToCount.TryAdd(centerLocation, 1))
    {
        MasCenterLocationToCount[centerLocation]++;
    }
}

void SearchForMasAtLocation(int startingY, int startingX)
{
    if (IsTestWordAtLocationAndDirection(-1, -1, startingX, startingY, mas))
    {
        AddMasCenterLocation(startingX - 1, startingY - 1);
    }
    if (IsTestWordAtLocationAndDirection(1, 1, startingX, startingY, mas))
    {
        AddMasCenterLocation(startingX + 1, startingY + 1);
    }
    if (IsTestWordAtLocationAndDirection(1, -1, startingX, startingY, mas))
    {
        AddMasCenterLocation(startingX + 1, startingY - 1);
    }
    if (IsTestWordAtLocationAndDirection(-1, 1, startingX, startingY, mas))
    {
        AddMasCenterLocation(startingX - 1, startingY + 1);
    }
}

int xmasCount = 0;
for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[i].Length; j++)
    {
        char c = grid[i][j];
        if (c == 'X')
        {
            xmasCount += GetXmasCount(i, j);
        }
        
        if (c == 'M')
        {
            SearchForMasAtLocation(i, j);
        }
    }
}

var masCount = MasCenterLocationToCount.Values.Count(x => x > 1);

Console.WriteLine($"Xmas count: {xmasCount}");
Console.WriteLine($"Mas count: {masCount}");