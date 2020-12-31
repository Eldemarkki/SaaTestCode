using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class TestMonoBehaviour : MonoBehaviour
{
    NativeArray<float> source;
    NativeArray<float> output;

    [SerializeField] private int size = 1000000;

    private Stopwatch stopwatch;

    private TestWithStructAsArrayJob saaJob;
    private long saaTicks;

    private TestWithNativeArrayJob nativeArrayJob;
    private long nativeArrayTicks;

    private int frameCount;

    private void Start()
    {
        source = new NativeArray<float>(size, Allocator.Persistent);
        for (int i = 0; i < source.Length; i++)
        {
            source[i] = Random.Range(-100f, 100f);
        }

        output = new NativeArray<float>(size * StructAsArray8<float>.Length, Allocator.Persistent);

        saaJob = new TestWithStructAsArrayJob()
        {
            source = source,
            output = output
        };

        nativeArrayJob = new TestWithNativeArrayJob()
        {
            source = source,
            output = output
        };

        // Run the jobs once to compile the burst job (the first run is always slower than the rest)
        saaJob.Schedule(size, 64).Complete();
        nativeArrayJob.Schedule(size, 64).Complete();

        stopwatch = Stopwatch.StartNew();
    }

    void Update()
    {
        stopwatch.Restart();
        saaJob.Schedule(size, 64).Complete();
        saaTicks += stopwatch.ElapsedTicks;

        stopwatch.Restart();
        nativeArrayJob.Schedule(size, 64).Complete();
        nativeArrayTicks += stopwatch.ElapsedTicks;

        frameCount++;
        print(frameCount);
    }

    private void OnDisable()
    {
        source.Dispose();
        output.Dispose();

        print($"SAA:  {saaTicks / (float)frameCount}");
        print($"NativeArray: {nativeArrayTicks / (float)frameCount}");
    }
}
