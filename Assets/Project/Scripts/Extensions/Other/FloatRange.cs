using UnityEngine;
using System;

[Serializable]
public class FloatRange
{
    [SerializeField]
    private float _min = 0f;
    [SerializeField]
    private float _max = 0f;

    public float Min
    {
        get => _min;
    }

    public float Max
    {
        get => _max;
    }

    public FloatRange(float min, float max)
    {
        _min = min;
        _max = max;
    }

    public bool InRange(float value)
    {
        return value > _min && value < _max;
    }
}
