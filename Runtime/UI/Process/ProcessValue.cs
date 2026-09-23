using System;
using UnityEngine;


[Serializable]
public class ProcessValue
{
    [SerializeField] double min = 0d;
    [SerializeField] double max = 1d;
    [SerializeField] double value = 0d;

    public ProcessValue() { }

    public ProcessValue(double value, double min, double max) => Setup(value, min, max);

    public double Min => min;
    public double Max => max;
    public double Value => value;
    public double Range => max - min;
    public double Remain => max - value;
    public bool IsFull => value >= max;
    public bool IsEmpty => value <= min;

    public float Normalized
    {
        get
        {
            double range = max - min;
            if (range <= 0d || double.IsNaN(range) || double.IsInfinity(range))
                return value >= max ? 1f : 0f;

            double t = (value - min) / range;
            if (double.IsNaN(t)) return 0f;
            return Mathf.Clamp01((float)t);
        }
    }

    public void Setup(double value, double min, double max)
    {
        SetMinMax(min, max);
        SetValue(value);
    }

    public void SetMinMax(double min, double max, bool keepNormalized = false)
    {
        float normalized = Normalized;

        this.min = min;
        this.max = max < min ? min : max;

        if (keepNormalized) SetNormalized(normalized);
        else SetValue(value); 
    }

    public void SetValue(double value) => this.value = Clamp(value, min, max);

    public void AddValue(double amount) => SetValue(value + amount);

    public void SetNormalized(float normalized) => SetValue(min + (max - min) * Mathf.Clamp01(normalized));

    static double Clamp(double value, double min, double max)
    {
        if (double.IsNaN(value)) return min;
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
}
