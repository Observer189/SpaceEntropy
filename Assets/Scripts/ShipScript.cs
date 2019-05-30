﻿using System.Collections;
using System.Collections.Generic;
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
            movementVector.Set((float)(-Mathf.Sin(Mathf.Deg2Rad * body.rotation)), (float)(Mathf.Cos(Mathf.Deg2Rad * body.rotation)));//выставление нормального вектора движения
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


        body.AddForce(new Vector2(movementVector.x * enginePower, movementVector.y * enginePower));//приложение силы по векторы движения
        if (body.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            Vector2 newVelocity = (body.velocity / body.velocity.magnitude) * maxSpeed;//Ограничение максимальной скорости
            body.velocity = newVelocity;
        }

        
        if(isShooting)
        {
            if (timeAftLastShot > reloadTime)
            {
                GameObject bulletClone = PoolManager.getGameObjectFromPool(bullet);//получение ссылки на пулю из пула

                bulletClone.GetComponent<Rigidbody2D>().position = new Vector2(body.position.x - 1 * Mathf.Sin(Mathf.Deg2Rad * body.rotation), body.position.y + 1 * Mathf.Cos(Mathf.Deg2Rad * body.rotation));//выставление позиции пули

                bulletClone.GetComponent<BulletScript>().create(body.rotation, ammoSpeed, range, body.velocity);//Передача пуле ее характеристик 
                timeAftLastShot = 0;
            }
            
                
            
        }
        timeAftLastShot += Time.fixedDeltaTime;
    }
}
