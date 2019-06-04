using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    new Transform transform;
    Transform playerTrans;

    void Start()
    {
        //player = GameObject.FindWithTag("Player");
        transform = GetComponent<Transform>();
        //playerTrans = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //var position = playerTrans.position;
        //transform.position = new Vector3(position.x, position.y, -10);
        //GameObject bulletClone = Instantiate(bullet, new Vector3(body.position.x-1*Mathf.Sin(Mathf.Deg2Rad*body.rotation), body.position.y+1*Mathf.Cos(Mathf.Deg2Rad*body.rotation), 0f), Quaternion.identity);
    }
}