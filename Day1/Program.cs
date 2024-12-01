using System.Diagnostics;

//string[] puzzleInput = File.ReadAllLines("Sample.txt");
string[] puzzleInput = File.ReadAllLines("PuzzleInput.txt");

List<int> inputs1 = new List<int>(puzzleInput.Length);
List<int> inputs2 = new List<int>(puzzleInput.Length);

for (int i = 0; i < puzzleInput.Length; i++)
{
    string inputLine = puzzleInput[i];
    string[] inputParts = inputLine.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
    Debug.Assert(inputParts.Length == 2, "Input line should have 2 parts");
    
    inputs1.Add(int.Parse(inputParts[0]));
    inputs2.Add(int.Parse(inputParts[1]));
}

inputs1.Sort();
inputs2.Sort();

int totalDifference = 0;
for (int i = 0; i < inputs1.Count; i++)
{
    totalDifference += Math.Abs(inputs1[i] - inputs2[i]);
}

Console.WriteLine($"Total difference: {totalDifference}");

Dictionary<int, int> inputs2Counts = new Dictionary<int, int>();
for (int i = 0; i < inputs2.Count; i++)
{
    if (inputs2Counts.ContainsKey(inputs2[i]))
    {
        inputs2Counts[inputs2[i]]++;
    }
    else
    {
        inputs2Counts[inputs2[i]] = 1;
    }
}

int totalSimilarity = 0;
for (int i = 0; i < inputs1.Count; i++)
{
    int value = inputs1[i];
    if (inputs2Counts.TryGetValue(value, out var count))
    {
        totalSimilarity += (value * count);
    }
}

Console.WriteLine($"Total similarity: {totalSimilarity}");
