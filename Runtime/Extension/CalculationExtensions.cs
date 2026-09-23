using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityExtensions
{
    public static class CalculationExtensions
    {
        private static System.Random random = new System.Random();

        public static int RangeNumber(int from, int to) => random.Next(from, to);

        public static float RangeNumber(float from, float to) => from + (float)(random.NextDouble() * (to - from));

        public static string IntoTimeCountDownLimitedHours(float time)
        {
            float hours = Mathf.Floor(time / 3600);
            float minutes = Mathf.Floor(time % 3600 / 60);
            float seconds = Mathf.RoundToInt(time % 60);

            if (hours > 0)
            {
                return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
            }
            else
            {
                return minutes.ToString("00") + ":" + seconds.ToString("00");
            }
        }


        #region Utils

        public static T Roll<T>(List<SpawnRate<T>> rates)
        {
            float total = 0;
            foreach (var r in rates) total += r.Rate;

            float rand = Random.Range(0, total);
            float cur = 0;

            foreach (var r in rates)
            {
                cur += r.Rate;
                if (rand <= cur)
                    return r.Type;
            }

            return rates[0].Type;
        }
        #endregion

        public static T RollByIndexWeight<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return default;

            int count = list.Count;

            float totalWeight = 0;
            float[] weights = new float[count];

            for (int i = 0; i < count; i++)
            {
                float w = (count - i);
                weights[i] = w;
                totalWeight += w;
            }

            float rand = Random.Range(0, totalWeight);
            float cur = 0;

            for (int i = 0; i < count; i++)
            {
                cur += weights[i];
                if (rand <= cur)
                    return list[i];
            }

            return list[0];
        }
    }
}

[System.Serializable]
public struct SpawnRate<T>
{
    public T Type;
    [Range(0, 100)] public float Rate;
}