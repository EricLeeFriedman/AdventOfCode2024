namespace Day7;

public class Node
{
    public Func<ulong, ulong, ulong>? Operator = null;
    public List<Node> Children = [];
}