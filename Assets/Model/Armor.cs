using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : ScriptableObject
{
    public float maxArmorPoints;
    
    protected float armorPoints;

    public float physReduce;
    public float explosiveReduce;
    public float energyReduce;

    public virtual Vector3 reduceDamage(float physDamage, float explosiveDamage, float energyDamage) { return new Vector3(0,0,0); }

    public float getArmorPoints()
    {
        return armorPoints;
    }
}
