using System.Diagnostics;
using Shared;

//string[] puzzleInput = File.ReadAllLines("Sample.txt");
string[] puzzleInput = File.ReadAllLines("PuzzleInput.txt");

List<Tuple<int, int>> pageOrderingRules = [];
int puzzleInputLine = 0;
for (; puzzleInputLine < puzzleInput.Length; puzzleInputLine++)
{
    string line = puzzleInput[puzzleInputLine];
    if (string.IsNullOrWhiteSpace(line)) break;

    string[] parts = line.Split('|');
    Debug.Assert(parts.Length == 2);
    pageOrderingRules.Add(new Tuple<int, int>(int.Parse(parts[0]), int.Parse(parts[1])));
}

List<int[]> updates = [];
for (int i = puzzleInputLine + 1; i < puzzleInput.Length; i++)
{
    string line = puzzleInput[i];
    updates.Add(line.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray());
}

bool IsPageIndexInUpdateValid(int[] update, int pageIndex, ref int invalidPage)
{
    int page = update[pageIndex];
    Tuple<int, int>[] relevantRules = pageOrderingRules.Where(x => page == x.Item1 || page == x.Item2).ToArray();

    int[] pagesBefore = update.Take(pageIndex).ToArray();
    foreach (int pageBefore in pagesBefore)
    {
        if (relevantRules.Any(x => x.Item2 == pageBefore))
        {
            invalidPage = pageBefore;
            return false;
        }
    }
        
    int[] pagesAfter = update.Skip(pageIndex + 1).ToArray();
    foreach (int pageAfter in pagesAfter)
    {
        if (relevantRules.Any(x => x.Item1 == pageAfter))
        {
            invalidPage = pageAfter;
            return false;
        }
    }

    return true;
}

bool IsUpdateValid(int[] update, ref int pageFailure, ref int invalidPage)
{
    for (int i = 0; i < update.Length; i++)
    {
        if (!IsPageIndexInUpdateValid(update, i, ref invalidPage))
        {
            pageFailure = update[i];
            return false;
        }
    }

    return true;
}

List<int[]> validUpdates = [];
List<int[]> invalidUpdates = [];
foreach (int[] update in updates)
{
    int pageFailure = -1;
    int invalidPage = -1;
    if (IsUpdateValid(update, ref pageFailure, ref invalidPage))
    {
        validUpdates.Add(update);
    }
    else
    {
        invalidUpdates.Add(update);
    }
}

List<int> validMiddlePages = validUpdates.Select(x => x[x.Length / 2]).ToList();
int sum = validMiddlePages.Sum();
Console.WriteLine($"Sum: {sum}");

// Part 2
List<int> invalidMiddlePages = [];
foreach (int[] invalidUpdate in invalidUpdates)
{
    int pageFailure = -1;
    int invalidPage = -1;
    while (!IsUpdateValid(invalidUpdate, ref pageFailure, ref invalidPage))
    {
        int pageFailureIndex = Array.IndexOf(invalidUpdate, pageFailure);
        int invalidPageIndex = Array.IndexOf(invalidUpdate, invalidPage);
        invalidUpdate.Swap(pageFailureIndex, invalidPageIndex);
    }
    invalidMiddlePages.Add(invalidUpdate[invalidUpdate.Length / 2]);
}

int invalidSum = invalidMiddlePages.Sum();
Console.WriteLine($"Invalid Sum: {invalidSum}");