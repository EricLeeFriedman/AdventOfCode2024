using System.Diagnostics;
using Shared;

//string[] puzzleInput = File.ReadAllLines("Sample.txt").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
string[] puzzleInput = File.ReadAllLines("PuzzleInput.txt").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
Debug.Assert(puzzleInput.Length % 3 == 0);

const int aCost = 3;
const int bCost = 1;
const bool part2 = true;

long totalCost = 0;

for (int i = 0; i < puzzleInput.Length; i += 3)
{
    string buttonA = puzzleInput[i];
    string buttonB = puzzleInput[i + 1];
    string prize = puzzleInput[i + 2];

    Vector2l buttonAInputs = ParseButton(buttonA);
    Vector2l buttonBInputs = ParseButton(buttonB);
    Vector2l prizeLocation = ParsePrize(prize);

    if (IsValidPrizeForInputs(buttonAInputs, aCost, buttonBInputs, bCost, prizeLocation, out long costForPrize))
    {
        totalCost += costForPrize;
    }
}

Console.WriteLine($"Total Cost: {totalCost}");
return;

Vector2l ParseButton(string button)
{
    string afterColon = button[(button.IndexOf(':') + 1)..].Trim();
    string[] parts = afterColon.Split(',');
    Debug.Assert(parts.Length == 2);
    
    string xPart = parts[0];
    string yPart = parts[1];
    Vector2l buttonInputs = new Vector2l
    (
        long.Parse(xPart[(xPart.IndexOf('+') + 1)..]),
        long.Parse(yPart[(yPart.IndexOf('+') + 1)..])
    );

    return buttonInputs;
}

Vector2l ParsePrize(string prize)
{
    string afterColon = prize[(prize.IndexOf(':') + 1)..].Trim();
    string[] parts = afterColon.Split(',');
    Debug.Assert(parts.Length == 2);

    // Part 2
    long additional = part2 ? 10000000000000 : 0;
    
    string xPart = parts[0];
    string yPart = parts[1];
    Vector2l prizeLocation = new Vector2l
    (
        long.Parse(xPart[(xPart.IndexOf('=') + 1)..]) + additional,
        long.Parse(yPart[(yPart.IndexOf('=') + 1)..]) + additional
    );

    return prizeLocation;
}

bool IsValidPrizeForInputs(Vector2l inputA, long inputACost, Vector2l inputB, long inputBCost,
    Vector2l prizeLocation, out long costForThisPrize)
{
    costForThisPrize = 0;
    
    // I couldn't remember my algebra, but I knew this was a system of equations
    // prizeLocation.X = inputA.X * inputACount + inputB.X * inputBCount
    // prizeLocation.Y = inputA.Y * inputACount + inputB.Y * inputBCount
    // https://x.com/i/grok/share/HFDGnxliZHrlWBvJL3nlBKkfo
    
    long inputACount = (prizeLocation.X * inputB.Y - prizeLocation.Y * inputB.X) / (inputA.X * inputB.Y - inputA.Y * inputB.X);
    long inputBCount = (inputA.X * prizeLocation.Y - inputA.Y * prizeLocation.X) / (inputA.X * inputB.Y - inputA.Y * inputB.X);
   
    Vector2l clawLocation = new Vector2l
    (
        inputA.X * inputACount + inputB.X * inputBCount,
        inputA.Y * inputACount + inputB.Y * inputBCount
    );
    if (!clawLocation.Equals(prizeLocation))
    {
        return false;
    }
    
    costForThisPrize = (inputACost * inputACount) + (inputBCost * inputBCount);
    return true;
}