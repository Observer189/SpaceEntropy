using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelArmor : BaseArmor
{
    public SteelArmor()
    {
        maxArmorPoints = 15;
        armorPoints = maxArmorPoints;
        physReduce = 10;
        explosiveReduce = 0.7f;
        energyReduce = 0.3f;
    }
    
}
