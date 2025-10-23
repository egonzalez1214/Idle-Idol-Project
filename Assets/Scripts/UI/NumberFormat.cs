using UnityEngine;

public class NumberFormat
{
    public static string Abbr(double v)
    {
        double a = System.Math.Abs(v);
        if (a >= 1_000_000_000)
        {
            return (v / 1_000_000_000d).ToString("0.##") + "B";
        }
        if (a >= 1_000_000)
        {
            return (v / 1_000_000d).ToString("0.##") + "M";
        }
        if (a >= 1_000)
        {
            return (v / 1_000d).ToString("0.##") + "k";
        }
        return v.ToString("0");
    }
}
