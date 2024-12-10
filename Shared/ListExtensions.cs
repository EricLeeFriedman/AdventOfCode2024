namespace Shared;

public static class ListExtensions
{
    public static void Swap<T>(this List<T> list, int index0, int index1)
    {
        T temp = list[index0];
        list[index0] = list[index1];
        list[index1] = temp;
    }
    
    public static List<int> FindAllIndicies<T>(this List<T> list, Func<T, bool> predicate)
    {
        List<int> allIndicies = [];
        for (int i = 0; i < list.Count; i++)
        {
            if (predicate(list[i]))
            {
                allIndicies.Add(i);
            }
        }
        return allIndicies;
    }

    public static int FindFirstIndexOfConsecutiveValues<T>(this List<T> list, T value, int consecutiveCount)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!Equals(list[i], value)) continue;
            
            int runningCount = 0;
            for (; i < list.Count && runningCount < consecutiveCount; i++, runningCount++)
            {
                if (!Equals(list[i], value)) break;
            }
            
            if (runningCount == consecutiveCount)
            {
                return i - consecutiveCount;
            }
        }
        return -1;
    }
}