using Shared;

string puzzleInput = File.ReadAllText("PuzzleInput.txt").Trim();
//string puzzleInput = File.ReadAllText("Sample.txt").Trim();

bool isFile = true;
int fileId = 0;
List<string> filesAndEmptySpaces = [];
foreach (var c in puzzleInput)
{
    ulong count = ulong.Parse(c.ToString());
    if (isFile)
    {
        for (ulong j = 0; j < count; j++)
        {
            filesAndEmptySpaces.Add(fileId.ToString());
        }
        ++fileId;
    }
    else
    {
        for (ulong j = 0; j < count; j++)
        {
            filesAndEmptySpaces.Add(".");
        }
    }
    isFile = !isFile;
}

Part1([..filesAndEmptySpaces]);
Part2([..filesAndEmptySpaces]);
return;

void Part2(List<string> filemap)
{
    int fileIdToMove = fileId - 1;
    while (fileIdToMove > 0)
    {
        List<int> allIndicies= filemap.FindAllIndicies(x => x == fileIdToMove.ToString());
        int requiredEmptySpaces = allIndicies.Count;
        
        int emptySpaceIndex = filemap.FindFirstIndexOfConsecutiveValues(".", requiredEmptySpaces);
        if (emptySpaceIndex >= 0 && emptySpaceIndex < allIndicies[0])
        {
            for (int i = 0; i < requiredEmptySpaces; i++)
            {
                filemap.Swap(emptySpaceIndex + i, allIndicies[i]);
            }
        }
        --fileIdToMove;
    }
    
    ulong sum = 0;
    for (int i = 0; i < filemap.Count; i++)
    {
        if (filemap[i] == ".") continue;
        sum += uint.Parse(filemap[i]) * (uint)i;
    }
    Console.WriteLine(sum);
}

void Part1(List<string> filemap)
{
    int freespaceIndex = filemap.FindIndex(f => f == ".");
    int fileIndex = filemap.FindLastIndex(f => f != ".");
    while (freespaceIndex < fileIndex)
    {
        filemap.Swap(freespaceIndex, fileIndex);
        freespaceIndex = filemap.FindIndex(freespaceIndex, f => f == ".");
        fileIndex = filemap.FindLastIndex(f => f != ".");
    }

    string[] filesWithoutEmptySpaces = filemap.Where(file => file != ".").ToArray();
    ulong sum = 0;
    for (uint i = 0; i < filesWithoutEmptySpaces.Length; i++)
    {
        sum += uint.Parse(filesWithoutEmptySpaces[i]) * i;
    }
    Console.WriteLine(sum);
}