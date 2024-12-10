namespace Shared;

public static class StringExtensions
{
    public static List<int> GetAllIndiciesOfSubstring(this string sourceString, string stringToFind)
    {
        List<int> allIndicies = [];
        int searchIndex = 0;
        while (searchIndex < sourceString.Length)
        {
            int substringIndex = sourceString.IndexOf(stringToFind, searchIndex);
            if (substringIndex < 0) break;
        
            allIndicies.Add(substringIndex);
            searchIndex = (substringIndex + stringToFind.Length);
        }
        return allIndicies;
    }   
}
