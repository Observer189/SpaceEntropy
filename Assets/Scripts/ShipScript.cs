using UnityEngine;

public class ShipScript : MonoBehaviour
{
    Rigidbody2D body;
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
    }

    void Update()
    {
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
            if (timeAftLastShot > reloadTime)
            {
                GameObject bulletClone = PoolManager.getGameObjectFromPool(bullet); //получение ссылки на пулю из пула

                bulletClone.GetComponent<Rigidbody2D>().position = new Vector2(
                    body.position.x - 1 * Mathf.Sin(Mathf.Deg2Rad * body.rotation),
                    body.position.y + 1 * Mathf.Cos(Mathf.Deg2Rad * body.rotation)); //выставление позиции пули

                bulletClone.GetComponent<BulletScript>().create(new Vector3(physDamage, explosiveDamage, energyDamage),
                    body.rotation, ammoSpeed, range, body.velocity); //Передача пуле ее характеристик 
                timeAftLastShot = 0;
            }
        }

        timeAftLastShot += Time.fixedDeltaTime;
    }
}