using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class PoolManager
{
    private static Dictionary<string, LinkedList<GameObject>> poolsDictionary;
    private static Transform deactivatedObjectsParent;

    public static void init(Transform pooledObjectsContainer)
    {
        deactivatedObjectsParent = pooledObjectsContainer;
        poolsDictionary = new Dictionary<string, LinkedList<GameObject>>();
    }

    public static GameObject getGameObjectFromPool(GameObject prefab,float x,float y)
    {
        if (!poolsDictionary.ContainsKey(prefab.name))
        {
            poolsDictionary[prefab.name] = new LinkedList<GameObject>();
        }

        GameObject result;

        if (poolsDictionary[prefab.name].Count > 0)
        {
            result = poolsDictionary[prefab.name].First.Value;
            poolsDictionary[prefab.name].RemoveFirst();
            result.GetComponent<Rigidbody2D>().position=new Vector2(x,y);
            result.SetActive(true);
            NetworkServer.Spawn(result);
            Debug.Log("Spawn1");
            return result;
        }
        else
        {
            result = GameObject.Instantiate(prefab,new Vector3(x,y,0),Quaternion.identity);
            //Debug.Log(result.GetComponent<Rigidbody2D>().position);
            result.name = prefab.name;
            NetworkServer.Spawn(result);
            Debug.Log("Spawn2");
            return result;
        }
    }

    public static void putGameObjectToPool(GameObject target)
    {
        if (!poolsDictionary.ContainsKey(target.name))
        {
            poolsDictionary[target.name] = new LinkedList<GameObject>();
        }

        poolsDictionary[target.name].AddFirst(target);

        target.transform.parent = deactivatedObjectsParent;
        //здесь кордината еще (1000,1000)
        target.SetActive(false);
        //а здесь уже координата уничтоженя
    }
}