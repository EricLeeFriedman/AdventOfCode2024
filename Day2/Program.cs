//string[] puzzleInput = File.ReadAllLines("Sample.txt");
string[] puzzleInput = File.ReadAllLines("PuzzleInput.txt");

bool IsReportSafe(int[] report)
{
    bool increased = false;
    bool decreased = false;
    for (int level = 0; level < report.Length - 1; level++)
    {
        int thisLevel = report[level];
        int nextLevel = report[level + 1];
        
        if (thisLevel < nextLevel)
        {
            increased = true;
        }
        else if (thisLevel > nextLevel)
        {
            decreased = true;
        }

        int difference = Math.Abs(thisLevel - nextLevel);
        if ((increased && decreased) || (difference < 1 || difference > 3))
        {
            return false;
        }
    }

    return true;
}

int safeReports = 0;
for (int i = 0; i < puzzleInput.Length; i++)
{
    string inputLine = puzzleInput[i];
    int[] report = inputLine.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();
    if (IsReportSafe(report))
    {
        safeReports++;
    }
}

Console.WriteLine($"Safe reports: {safeReports}");

// Part 2
safeReports = 0;
for (int i = 0; i < puzzleInput.Length; i++)
{
    string inputLine = puzzleInput[i];
    int[] report = inputLine.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();

    if (IsReportSafe(report))
    {
        ++safeReports;
        continue;
    }

    for (int levelToRemove = 0; levelToRemove < report.Length; levelToRemove++)
    {
        List<int> reportList = report.ToList();
        reportList.RemoveAt(levelToRemove);
        if (IsReportSafe(reportList.ToArray()))
        {
            ++safeReports;
            break;
        }
    }
}

Console.WriteLine($"Safe reports if we can remove 1: {safeReports}");