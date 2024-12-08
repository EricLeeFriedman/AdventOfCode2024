using Day7;

//string[] puzzleInput = File.ReadAllLines("Sample.txt");
string[] puzzleInput = File.ReadAllLines("PuzzleInput.txt");

List<ulong> validTestValues = [];
List<ulong> validTestValuesWithCombine = [];
foreach (string line in puzzleInput)
{
    string[] lineParts = line.Split(':');
    if (lineParts.Length != 2) throw new Exception("Expecting two parts");

    ulong testValue = ulong.Parse(lineParts[0].Trim());
    ulong[] operands = lineParts[1].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(ulong.Parse).ToArray();

    int operatorCount = operands.Length - 1;

    Node root = new Node();
    root.Operator = Operators.ReturnY;
    AddChildNodes(root, operatorCount, false);
    if (TraverseTree(root, testValue, operands, 0, 0))
    {
        validTestValues.Add(testValue);
    }

    Node rootWithCombine = new Node();
    rootWithCombine.Operator = Operators.ReturnY;
    AddChildNodes(rootWithCombine, operatorCount, true);
    if (TraverseTree(rootWithCombine, testValue, operands, 0, 0))
    {
        validTestValuesWithCombine.Add(testValue);
    }
}

Console.WriteLine(
    $"Test Value Sum: {validTestValues.Aggregate<ulong, ulong>(0, (current, validTestValue) => current + validTestValue)}");
Console.WriteLine(
    $"Test Value Sum with Combine: {validTestValuesWithCombine.Aggregate<ulong, ulong>(0, (current, validTestValue) => current + validTestValue)}");

bool TraverseTree(Node node, ulong testValue, ulong[] operands, ulong runningResult, int currentDepth)
{
    if (node.Operator != null)
    {
        runningResult = node.Operator(runningResult, operands[currentDepth]);
    }

    if (node.Children.Count == 0)
    {
        return runningResult == testValue;
    }


    return node.Children.Any(child => TraverseTree(child, testValue, operands, runningResult, currentDepth + 1));
}

void AddChildNodes(Node node, int count, bool addCombineNode)
{
    if (count == 0) return;

    Node addNode = new Node();
    addNode.Operator = Operators.Add;
    AddChildNodes(addNode, count - 1, addCombineNode);
    node.Children.Add(addNode);

    Node multiplyNode = new Node();
    multiplyNode.Operator = Operators.Multiply;
    AddChildNodes(multiplyNode, count - 1, addCombineNode);
    node.Children.Add(multiplyNode);

    if (addCombineNode)
    {
        Node combineNode = new Node();
        combineNode.Operator = Operators.Combine;
        AddChildNodes(combineNode, count - 1, addCombineNode);
        node.Children.Add(combineNode);
    }
}

public static class Operators
{
    public static ulong ReturnY(ulong x, ulong y)
    {
        return y;
    }

    public static ulong Add(ulong x, ulong y)
    {
        return x + y;
    }

    public static ulong Multiply(ulong x, ulong y)
    {
        return x * y;
    }

    public static ulong Combine(ulong x, ulong y)
    {
        string xString = x.ToString();
        string yString = y.ToString();
        return ulong.Parse(xString + yString);
    }
}