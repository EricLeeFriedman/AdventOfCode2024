using System.Diagnostics;
using System.Globalization;
using Day3;
using Shared;

//string[] puzzleInput = File.ReadAllLines("Sample.txt");
//string[] puzzleInput = File.ReadAllLines("Sample2.txt");
string[] puzzleInput = File.ReadAllLines("PuzzleInput.txt");

string mulToken = "mul(";
int mulTokenLength = mulToken.Length;
List<MulCommand> mulCommands = [];

for (int i = 0; i < puzzleInput.Length; i++)
{
    string line = puzzleInput[i];
    int currentMulCommandIndex = line.IndexOf(mulToken);
    while (currentMulCommandIndex >= 0)
    {
        int mulCommandEndIndex = line.IndexOf(')', currentMulCommandIndex);
        if (mulCommandEndIndex < 0)
        {
            break;
        }

        int mulCommandParametersStartIndex = currentMulCommandIndex + mulTokenLength;
        string mulCommandParameters =
            line.Substring(mulCommandParametersStartIndex, mulCommandEndIndex - mulCommandParametersStartIndex);

        string[] parameters = mulCommandParameters.Split(',');
        if (parameters.Length != 2) goto NextLoop;

        try
        {
            long val0 = long.Parse(parameters[0], NumberStyles.None);
            long val1 = long.Parse(parameters[1], NumberStyles.None);
            mulCommands.Add(new MulCommand
            {
                LineIndex = i,
                StartIndex = currentMulCommandIndex,
                Value0 = val0,
                Value1 = val1
            });
        }
        catch (FormatException)
        {
            // This is fine to continue, do nothing
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e}");
            throw;
        }

        NextLoop:
        currentMulCommandIndex = line.IndexOf(mulToken, currentMulCommandIndex + mulTokenLength);
    }
}

long mulCommandSum = 0;

mulCommands.ForEach(x => mulCommandSum += x.Value0 * x.Value1);
Console.WriteLine($"Mul command sum: {mulCommandSum}");

// Part 2
string doToken = "do()";
string dontToken = "don't()";

List<DisableZone>[] disableZonesPerLine = new List<DisableZone>[puzzleInput.Length];
bool lookingForDisableZoneStart = true;
for (int i = 0; i < puzzleInput.Length; i++)
{
    List<DisableZone> disableZones = [];
    if (!lookingForDisableZoneStart)
    {
        disableZones.Add(new DisableZone
        {
            StartIndex = 0,
        });
    }
    
    string line = puzzleInput[i];
    
    List<int> doIndicies = line.GetAllIndiciesOfSubstring(doToken);
    List<int> dontIndicies = line.GetAllIndiciesOfSubstring(dontToken);

    int doIndex = 0;
    int dontIndex = 0;
    int lastDo = -1;
    int lastDont = -1;
    
    while (doIndex < doIndicies.Count && dontIndex < dontIndicies.Count)
    {
        if (lookingForDisableZoneStart)
        {
            while (dontIndicies[dontIndex] <= lastDo)
            {
                dontIndex++;
                if (dontIndex >= dontIndicies.Count)
                {
                    goto NoMoreDosAndDonts;
                }
            }
            
            disableZones.Add(new DisableZone
            {
                StartIndex = dontIndicies[dontIndex],
            });
            lookingForDisableZoneStart = false;
            lastDont = dontIndicies[dontIndex];
        }
        else
        {
            while (doIndicies[doIndex] <= lastDont)
            {
                doIndex++;
                if (doIndex >= doIndicies.Count)
                {
                    goto NoMoreDosAndDonts;
                }
            }
            DisableZone disableZone = disableZones.Last();
            disableZone.EndIndex = doIndicies[doIndex];
            Debug.Assert(disableZone.EndIndex > disableZone.StartIndex);
            disableZones[^1] = disableZone;
            lookingForDisableZoneStart = true;
            lastDo = doIndicies[doIndex];
        }
    }
    
    NoMoreDosAndDonts:
    if (!lookingForDisableZoneStart)
    {
        DisableZone disableZone = disableZones.Last();
        disableZone.EndIndex = line.Length - 1;
        disableZones[^1] = disableZone;
    }
    
    disableZonesPerLine[i] = disableZones;
}

bool IsInDisableZone(MulCommand mulCommand)
{
    List<DisableZone> disableZones = disableZonesPerLine[mulCommand.LineIndex];
    return disableZones.Any(disableZone => mulCommand.StartIndex >= disableZone.StartIndex && mulCommand.StartIndex <= disableZone.EndIndex);
}

mulCommandSum = 0;
mulCommands.Where(x => !IsInDisableZone(x)).ToList().ForEach(x => mulCommandSum += x.Value0 * x.Value1);
Console.WriteLine($"Mul commands not in disable zone sum: {mulCommandSum}");
