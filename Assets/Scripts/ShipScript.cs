using UnityEngine;
using UnityEngine.Networking;

public class ShipScript : NetworkBehaviour,Destroyable
{
    Rigidbody2D body;
    [SyncVar]
    public float hp;
    public Armor armor;
    public float angularVelocity;
    public float ammoSpeed;
    public float enginePower;
    public float range;
    public float maxSpeed;
    public float reloadTime;
    public float physDamage;
    public float explosiveDamage;
    public float energyDamage;

    float timeAftLastShot;
    Vector2 movementVector;
    bool isShooting;

    public GameObject bullet;

    int rotation;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        isShooting = false;
        timeAftLastShot = 0;
  
        armor=new SteelArmor();
    }
    
    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void Update()
    {
        if(!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.Space)) isShooting = true;
        if (Input.GetKeyUp(KeyCode.Space)) isShooting = false;
        if (Input.GetAxis("Horizontal") < 0) 
        
        {
            rotation = 1;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            rotation = -1;
        }
        else if (Input.GetAxis("Horizontal") == 0)
        {
            rotation = 0;
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            movementVector.Set(-Mathf.Sin(Mathf.Deg2Rad * body.rotation),
                Mathf.Cos(Mathf.Deg2Rad * body.rotation)); //выставление нормального вектора движения
        }
        else
        {
            movementVector.Set(0, 0);
        }

        if (hp <= 0)
        {
            destroy();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (rotation > 0)
        {
            body.angularVelocity = angularVelocity;
        }
        else if (rotation < 0)
        {
            body.angularVelocity = -angularVelocity;
        }
        else
        {
            body.angularVelocity = 0;
        }


        body.AddForce(new Vector2(movementVector.x * enginePower,
            movementVector.y * enginePower)); //приложение силы по векторы движения
        if (body.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            var velocity = body.velocity;
            Vector2 newVelocity = (velocity / velocity.magnitude) * maxSpeed; //Ограничение максимальной скорости
            velocity = newVelocity;
            body.velocity = velocity;
        }


        if (isShooting)
        {
            CmdFire();
        }

        timeAftLastShot += Time.fixedDeltaTime;
    }

    public void doDamage(float physDamage, float explosiveDamage, float energyDamage)
    {
        Vector3 damage = armor.reduceDamage(physDamage, explosiveDamage, energyDamage);
        hp -= damage.x + damage.y + damage.z;
    }

    public void destroy()
    {
        PoolManager.putGameObjectToPool(gameObject);
    }

    [Command] 
    void CmdFire()
    {
        if (timeAftLastShot > reloadTime)
        {
            GameObject bulletClone = PoolManager.getGameObjectFromPool(bullet,body.position.x - 2 * Mathf.Sin(Mathf.Deg2Rad * body.rotation),
                body.position.y + 2 * Mathf.Cos(Mathf.Deg2Rad * body.rotation)); //получение ссылки на пулю из пула

            /*bulletClone.GetComponent<Rigidbody2D>().position = new Vector2(
                body.position.x - 2 * Mathf.Sin(Mathf.Deg2Rad * body.rotation),
                body.position.y + 2 * Mathf.Cos(Mathf.Deg2Rad * body.rotation)); *///выставление позиции пули

            bulletClone.GetComponent<BulletScript>().create(new Vector3(physDamage, explosiveDamage, energyDamage),
                body.rotation, ammoSpeed, range, body.velocity); //Передача пуле ее характеристик 
            
            //NetworkServer.Spawn(bulletClone);
            timeAftLastShot = 0;
        }
        /*if (timeAftLastShot > reloadTime)
        {
            GameObject bulletClone = Instantiate(bullet,new Vector2(
                body.position.x - 2 * Mathf.Sin(Mathf.Deg2Rad * body.rotation),
                body.position.y + 2 * Mathf.Cos(Mathf.Deg2Rad * body.rotation)),Quaternion.identity);
            GameObject bulletClone = PoolManager.getGameObjectFromPool(bullet); //получение ссылки на пулю из пула
            bulletClone.GetComponent<Rigidbody2D>().position = new Vector2(
                body.position.x - 2 * Mathf.Sin(Mathf.Deg2Rad * body.rotation),
                body.position.y + 2 * Mathf.Cos(Mathf.Deg2Rad * body.rotation));
            //bulletClone.GetComponent<BulletScript>().create(new Vector3(physDamage, explosiveDamage, energyDamage),
              //  body.rotation, ammoSpeed, range, body.velocity);
              bulletClone.GetComponent<Rigidbody2D>().velocity = new Vector2((float) -Mathf.Sin(Mathf.Deg2Rad * body.rotation) * ammoSpeed + body.velocity.x,
                  (float) Mathf.Cos(Mathf.Deg2Rad * body.rotation) * ammoSpeed + body.velocity.y);;  
            NetworkServer.Spawn(bulletClone);
            timeAftLastShot = 0;
            
        }*/
    }
}