using Unity.Collections;

public static class NativeListExtensions
{
    public static void Reverse<T>(this NativeList<T> list)
           where T : unmanaged
    {
        var length = list.Length;
        var index1 = 0;

        for (var index2 = length - 1; index1 < index2; --index2)
        {
            var obj = list[index1];
            list[index1] = list[index2];
            list[index2] = obj;
            ++index1;
        }
    }
}