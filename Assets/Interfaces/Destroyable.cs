using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Destroyable 
{

    void doDamage(float physDamage,float explosiveDamage,float energyDamage);
    void destroy();

}
