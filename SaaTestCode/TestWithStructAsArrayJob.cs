using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile]
public struct TestWithStructAsArrayJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<float> source;

    [WriteOnly, NativeDisableParallelForRestriction]
    public NativeArray<float> output;

    public void Execute(int index)
    {
        // Get some random values, this function could do basically anything, all that matters is that it has to return a StructAsArray.
        StructAsArray8<float> values = GetValues(index, source);

        // Fill a portion of the output array with the new values
        output[index * StructAsArray8<float>.Length + 0] = values.Element1;
        output[index * StructAsArray8<float>.Length + 1] = values.Element2;
        output[index * StructAsArray8<float>.Length + 2] = values.Element3;
        output[index * StructAsArray8<float>.Length + 3] = values.Element4;
        output[index * StructAsArray8<float>.Length + 4] = values.Element5;
        output[index * StructAsArray8<float>.Length + 5] = values.Element6;
        output[index * StructAsArray8<float>.Length + 6] = values.Element7;
        output[index * StructAsArray8<float>.Length + 7] = values.Element8;
    }

    private StructAsArray8<float> GetValues(int index, NativeArray<float> source)
    {
        StructAsArray8<float> values = new StructAsArray8<float>();

        // Here you can set the values to whatever, I just set them to y = x*5 + i
        values.Element1 = source[index] * 5 + 0;
        values.Element2 = source[index] * 5 + 1;
        values.Element3 = source[index] * 5 + 2;
        values.Element4 = source[index] * 5 + 3;
        values.Element5 = source[index] * 5 + 4;
        values.Element6 = source[index] * 5 + 5;
        values.Element7 = source[index] * 5 + 6;
        values.Element8 = source[index] * 5 + 7;

        return values;
    }
}
