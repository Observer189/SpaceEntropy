using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Asteroid;
    public int AstCount;
    public float AstFieldRadius;

    private GameObject Player;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60; // Фиксация fps на 60

        PoolManager.init(GetComponent<Transform>());
        
        Instantiate(Asteroid, new Vector3(0, 10, 0), Quaternion.identity);
        for (int i = 0; i < AstCount; i++)
        {
            float angle = i * Mathf.PI * 2 / AstCount;
            Vector3 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * AstFieldRadius;
            Instantiate(Asteroid, pos, Quaternion.identity);
        }
        
    }


    // Update is called once per frame
    void Update()
    {
    }

    public GameObject getPlayer()
    {
        return Player;
    }
}