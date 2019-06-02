using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D body;
    float physDamage;
    float explosiveDamage;
    float energyDamage;
    float startSpeed;
    float range;
    float coveredDistance;
    Vector2 lastPosition;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        coveredDistance = 0;
        lastPosition = body.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        coveredDistance += Mathf.Abs((body.position - lastPosition).magnitude); //Рассчет пройденного расстояния
        lastPosition = body.position;
        if (coveredDistance >= range) //Уничтожение пули при прохождении расстояния стрельбы
        {
            destroy();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Asteroid"))
        {
            col.gameObject.GetComponent<AsteroidScript>().doDamage(physDamage, explosiveDamage, energyDamage);
            destroy();
        }
    }

    public void create(Vector3 damage, float shipRotation, float startSpeed, float range, Vector2 shipVelocity)
    {
        //Debug.Log(shipRotation);
        physDamage = damage.x;
        explosiveDamage = damage.y;
        energyDamage = damage.z;
        this.startSpeed = startSpeed;
        this.range = range;
        body.rotation = shipRotation;
        body.velocity = new Vector2((float) -Mathf.Sin(Mathf.Deg2Rad * body.rotation) * startSpeed + shipVelocity.x,
            (float) Mathf.Cos(Mathf.Deg2Rad * body.rotation) * startSpeed + shipVelocity.y);
        lastPosition = body.position;
    }

    void nullify() //Функция обнуления
    {
        physDamage = 0;
        explosiveDamage = 0;
        energyDamage = 0;
        coveredDistance = 0;
        body.position = new Vector2(1000, 1000);
        body.velocity = new Vector2(0, 0);
    }

    void destroy()
    {
        nullify();
        PoolManager.putGameObjectToPool(gameObject);
    }
}