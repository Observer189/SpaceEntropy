using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArmor : Armor
{
   
    public override Vector3 reduceDamage(float physDamage,float explosiveDamage,float energyDamage)
    {
        float consistency=armorPoints/maxArmorPoints;
        float pD;
        float expD;
        float enD = energyDamage * (1-energyReduce);
        if (consistency < 0.5f)
        {
            pD = physDamage - physReduce * (consistency / 0.5f);
        }
        else 
        {
            pD = physDamage - physReduce;
        }

        if (pD >= 0)
        {
            expD = explosiveDamage;
            armorPoints -= physDamage - pD;
        }
        else
        {
            expD = explosiveDamage * (1-explosiveReduce);
            pD = 0;
            armorPoints -= physDamage;
        }

        armorPoints -= energyDamage - enD;

        if (armorPoints < 0)
            armorPoints = 0;
        return new Vector3(pD, expD, enD);
    }
}
