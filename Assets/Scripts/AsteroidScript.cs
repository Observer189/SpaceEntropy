using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour,Destroyable
{
    // Start is called before the first frame update
    Armor armor;
    public float maxHp;
    float hp;
    Rigidbody2D body;
    void Start()
    {
        hp = maxHp;
        armor=new SteelArmor();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            destroy();
        }
    }

    public void doDamage(float physDamage, float explosiveDamage, float energyDamage)
    {
        Vector3 damage = armor.reduceDamage(physDamage, explosiveDamage, energyDamage);
        hp -= damage.x + damage.y + damage.z;
        Debug.Log(damage);
    }
    public void destroy()
    {
        
        PoolManager.putGameObjectToPool(gameObject);
    }
    public void nullify()
    {
        body.velocity = new Vector2(0, 0);
        hp = maxHp;
    }
}
