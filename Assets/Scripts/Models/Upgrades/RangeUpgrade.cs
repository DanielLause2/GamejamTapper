using UnityEngine;

public class RangeUpgrade : Upgrade
{
    public int ArrowAmount;
    public bool PiercingArrow;

    [Range(0, 1)]
    public float PiercingReduction;
}