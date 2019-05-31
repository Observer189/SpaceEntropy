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
    private Rigidbody2D playerRigidbody2D;
    private Rigidbody2D enemyRigidbody2D;

    private Boolean flag = false;
    
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
    int rotation;
    
    public GameObject bullet;
    
    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
        
        isShooting = true;
        timeAftLastShot = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        GetComps(flag);
        
        //Debug.Log(Player.GetComponent<Rigidbody2D>().position);
        
        Debug.Log(playerRigidbody2D.position);
        //playerPos = playerRigidbody2D.position;
    }

    private void FixedUpdate()
    {
        if(isShooting)
        {
            if (timeAftLastShot > reloadTime)
            {
                GameObject bulletClone = PoolManager.getGameObjectFromPool(bullet);//получение ссылки на пулю из пула

                bulletClone.GetComponent<Rigidbody2D>().position = new Vector2(enemyRigidbody2D.position.x - 1 * Mathf.Sin(Mathf.Deg2Rad * enemyRigidbody2D.rotation), enemyRigidbody2D.position.y + 1 * Mathf.Cos(Mathf.Deg2Rad * enemyRigidbody2D.rotation));//выставление позиции пули

                bulletClone.GetComponent<BulletScript>().create(new Vector3(physDamage,explosiveDamage,energyDamage),enemyRigidbody2D.rotation, ammoSpeed, range, enemyRigidbody2D.velocity);//Передача пуле ее характеристик 
                timeAftLastShot = 0;
            }
            
                
            
        }
        timeAftLastShot += Time.fixedDeltaTime;
    }

    void GetComps(Boolean flag)
    {
        if (flag == false)
        {
            Player = GameController.GetComponent<GameConScript>().getPlayer();
            playerRigidbody2D = Player.GetComponent<Rigidbody2D>();
            flag = true;
        }
    }
}