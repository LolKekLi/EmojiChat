using UnityEngine;
using System;

[Serializable]
public class IntRange
{
    [SerializeField]
    private int _min = 0;
    [SerializeField]
    private int _max = 0;

    public int Min
    {
        get => _min;
    }

    public int Max
    {
        get => _max;
    }

    public IntRange(int min, int max)
    {
        _min = min;
        _max = max;
    }
}
