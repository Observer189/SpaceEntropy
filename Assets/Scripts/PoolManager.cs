using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    private static Dictionary<string, LinkedList<GameObject>> poolsDictionary;
    private static Transform deactivatedObjectsParent;

    public static void init(Transform pooledObjectsContainer)
    {
        deactivatedObjectsParent = pooledObjectsContainer;
        poolsDictionary = new Dictionary<string, LinkedList<GameObject>>();
    }

    public static GameObject getGameObjectFromPool(GameObject prefab)
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
            result.SetActive(true);
            return result;
        }
        else
        {
            result = GameObject.Instantiate(prefab);
            //Debug.Log(result.GetComponent<Rigidbody2D>().position);
            result.name = prefab.name;

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