using System;
using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation
// ReSharper disable All

public class EnemyScript: MonoBehaviour
{

    public int difficulty; // 0 - peace; 1 - easy; 2 - normal; 3 - hard;

    private GameObject GameController;
    
    private GameObject Player;
    private Vector2 playerPos;
    private Rigidbody2D PlayerRigidbody2D;
    private Rigidbody2D EnemyRigidbody2D;

    //private Boolean flag = false;
    
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
    bool isShooting = true;
    int rotation; // 1 - влево, -1 - вправо
    
    private float Angle;
    private float internalArcLength;
    private float externalArcLength;
    
    public GameObject bullet;
    
    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        Player = GameController.GetComponent<GameConScript>().getPlayer();
        
        EnemyRigidbody2D = GetComponent<Rigidbody2D>(); 
        PlayerRigidbody2D = Player.GetComponent<Rigidbody2D>();
        
        timeAftLastShot = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(Player.GetComponent<Rigidbody2D>().position);
        //Debug.Log(PlayerRigidbody2D.position);
        //playerPos = playerRigidbody2D.position;


        Angle = GettingAngle();

        EnemyRigidbody2D.SetRotation(Angle);

        /*if (((Angle - 2) < transform.eulerAngles.z) && ((Angle + 2) > transform.eulerAngles.z))
        {
            EnemyRigidbody2D.angularVelocity = 0;
            isShooting = true;
        }
        else
        {
            Turning();
            isShooting = false;
        }*/


    }

    private void FixedUpdate()
    {
        
        
        if(isShooting)
        {
            if (timeAftLastShot > reloadTime)
            {
                GameObject bulletClone = PoolManager.getGameObjectFromPool(bullet);//получение ссылки на пулю из пула

                bulletClone.GetComponent<Rigidbody2D>().position = new Vector2(EnemyRigidbody2D.position.x - 1 * Mathf.Sin(Mathf.Deg2Rad * EnemyRigidbody2D.rotation), EnemyRigidbody2D.position.y + 1 * Mathf.Cos(Mathf.Deg2Rad * EnemyRigidbody2D.rotation));//выставление позиции пули

                bulletClone.GetComponent<BulletScript>().create(new Vector3(physDamage,explosiveDamage,energyDamage),EnemyRigidbody2D.rotation, ammoSpeed, range, EnemyRigidbody2D.velocity);//Передача пуле ее характеристик 
                timeAftLastShot = 0;
            }
        }
        timeAftLastShot += Time.fixedDeltaTime;
    }

    float GettingAngle()
    {
        
        float angle = 0.0f;
        
        float x1 = EnemyRigidbody2D.position.x; //Координаты Enemy
        float y1 = EnemyRigidbody2D.position.y; 
        
        float x2 = PlayerRigidbody2D.position.x; //Координаты Player
        float y2 = PlayerRigidbody2D.position.y;

        //Debug.Log("x2: " + x2 + "y2: " + y2);
        

        float EnemyRot = transform.eulerAngles.z;

        angle = (float) (RadToDeg(Math.Atan2(y1 - y2, x1 - x2)));
        angle = (angle < 0) ? angle + 360 : angle;
        angle = (angle < 270) ? angle + 90 : angle+90-360;
        
        externalArcLength = Math.Abs(angle - EnemyRot);
        internalArcLength = 360 - externalArcLength;

        if (angle > EnemyRot)
        {
            if (internalArcLength > externalArcLength)
            {
                if (EnemyRot == 360)
                    transform.eulerAngles.Set(0, 0, 0);
                transform.eulerAngles.Set(0, 0, transform.eulerAngles.z + 3f);
                rotation = 1;
                Debug.Log("Turn +");
            } else if (internalArcLength < externalArcLength) {
                if (EnemyRot == 0)
                    transform.eulerAngles.Set(0, 0, 360);
                transform.eulerAngles.Set(0, 0, transform.eulerAngles.z - 3f);
                rotation = -1;
                Debug.Log("Turn -");
            }
        } 
        else if (angle < EnemyRot)
        {
            if (internalArcLength < externalArcLength)
            {
                if (EnemyRot == 360)
                    transform.eulerAngles.Set(0, 0, 0);
                transform.eulerAngles.Set(0, 0, 1);
                rotation = 1;
                Debug.Log("Turn +");
            } else if (internalArcLength > externalArcLength) {
                if (EnemyRot == 0)
                    transform.eulerAngles.Set(0, 0, 360);
                transform.eulerAngles.Set(0, 0, 359);
                rotation = -1;
                Debug.Log("Turn -");
            }
        }

        Debug.Log("Angle: " + Angle + "_____" + "Rotation: " + transform.eulerAngles.z);
        Debug.Log(internalArcLength + "_____" + externalArcLength);

        return angle;
    }

    void Turning()
    {
        if (rotation > 0)
        {
            EnemyRigidbody2D.angularVelocity = angularVelocity;
        }
        else if (rotation < 0)
        {
            EnemyRigidbody2D.angularVelocity = -angularVelocity;
        }
    }
    
    public static double RadToDeg(double radians)
    {
        double degrees = (180 / Math.PI) * radians;
        return (degrees);
    }
}