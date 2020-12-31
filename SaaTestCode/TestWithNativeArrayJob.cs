using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile]
public struct TestWithNativeArrayJob : IJobParallelFor
{
    public const int GetValuesSize = 8;

    [ReadOnly]
    public NativeArray<float> source;

    [WriteOnly, NativeDisableParallelForRestriction]
    public NativeArray<float> output;

    public void Execute(int index)
    {
        // Get some random values, this function could do basically anything, all that matters is that it has to return a NativeArray.
        NativeArray<float> values = GetValues(index, source);

        // Fill a portion of the output array with the new values
        for (int i = 0; i < values.Length; i++)
        {
            output[index * GetValuesSize + i] = values[i];
        }
    }

    private NativeArray<float> GetValues(int index, NativeArray<float> source)
    {
        NativeArray<float> values = new NativeArray<float>(GetValuesSize, Allocator.Temp);
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = source[index] * 5 + i;
        }

        return values;
    }
}