namespace Shared;

public static class ArrayExtensions
{
    public static void Swap<T>(this T[] array, int index0, int index1)
    {
        T temp = array[index0];
        array[index0] = array[index1];
        array[index1] = temp;
    }
}