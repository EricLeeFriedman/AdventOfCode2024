//string puzzleInput = File.ReadAllText("Sample.txt");
string puzzleInput = File.ReadAllText("PuzzleInput.txt");
List<ulong> stones = puzzleInput.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(ulong.Parse).ToList();
stones.EnsureCapacity(4096 * 1024);

ulong numZeroStones = 0;
Dictionary<ulong, ulong> stonesThatAreEvenLength = new();
Dictionary<ulong, ulong> stonesThatAreOddLength = new();
Dictionary<ulong, ulong> newStonesThatAreEvenLength = [];
Dictionary<ulong, ulong> newStonesThatAreOddLength = [];

InitializeStoneCounts();

//for (int i = 0; i < 25; i++)
for (int i = 0; i < 75; i++)
{
    ulong newZeroStones = 0;
    foreach (KeyValuePair<ulong, ulong> evenStoneToCount in stonesThatAreEvenLength)
    {
        ulong evenStone = evenStoneToCount.Key;
        string evenStoneString = evenStone.ToString();
        int halfLength = evenStoneString.Length >> 1;
        
        string firstStoneString = evenStoneString[..halfLength];
        ulong firstStone = ulong.Parse(firstStoneString);
        ProcessPotentialZeroStone(firstStone, ref newZeroStones, evenStoneToCount.Value);
        
        string secondStoneString = evenStoneString[halfLength..];
        ulong secondStone = ulong.Parse(secondStoneString);
        ProcessPotentialZeroStone(secondStone, ref newZeroStones, evenStoneToCount.Value);
    }

    foreach (KeyValuePair<ulong, ulong> oddStoneToCount in stonesThatAreOddLength)
    {
        ulong newStone = oddStoneToCount.Key * 2024;
        if (IsNumberOfDigitsEven(newStone))
        {
            AddStoneToDictionary(newStone, ref newStonesThatAreEvenLength, oddStoneToCount.Value);
        }
        else
        {
            AddStoneToDictionary(newStone, ref newStonesThatAreOddLength, oddStoneToCount.Value);
        }
    }

    if (numZeroStones > 0)
    {
        AddStoneToDictionary(1, ref newStonesThatAreOddLength, numZeroStones);
    }

    stonesThatAreEvenLength = new Dictionary<ulong, ulong>(newStonesThatAreEvenLength);
    stonesThatAreOddLength = new Dictionary<ulong, ulong>(newStonesThatAreOddLength);
    numZeroStones = newZeroStones;
    newStonesThatAreEvenLength.Clear();
    newStonesThatAreOddLength.Clear();
}

ulong numEvenStones = stonesThatAreEvenLength.Values.Aggregate<ulong, ulong>(0, (current, count) => current + count);
ulong numOddStones = stonesThatAreOddLength.Values.Aggregate<ulong, ulong>(0, (current, count) => current + count);
Console.Write($"Number of stones: {numZeroStones + numEvenStones + numOddStones}");
return;

void ProcessPotentialZeroStone(ulong stone, ref ulong newZeroStones, ulong count)
{
    if (stone == 0)
    {
        newZeroStones += count;
    }
    else
    {
        if (IsNumberOfDigitsEven(stone))
        {
            AddStoneToDictionary(stone, ref newStonesThatAreEvenLength, count);
        }
        else
        {
            AddStoneToDictionary(stone, ref newStonesThatAreOddLength, count);
        }
    }
}

void InitializeStoneCounts()
{
    foreach (ulong stone in stones)
    {
        if (stone == 0)
        {
            ++numZeroStones;
            continue;
        }

        if (IsNumberOfDigitsEven(stone))
        {
            AddStoneToDictionary(stone, ref stonesThatAreEvenLength, 1);
        }
        else
        {
            AddStoneToDictionary(stone, ref stonesThatAreOddLength, 1);
        }
    }
}

void AddStoneToDictionary(ulong stone, ref Dictionary<ulong, ulong> dictionary, ulong count)
{
    if (dictionary.TryGetValue(stone, out ulong currentCount))
    {
        dictionary[stone] = currentCount + count;
    }
    else
    {
        dictionary.Add(stone, count);
    }
}

bool IsNumberOfDigitsEven(ulong number)
{
    return ((int)Math.Floor(Math.Log10(number)) + 1 & 1) == 0;
}