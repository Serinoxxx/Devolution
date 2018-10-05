using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand = true;
}

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler instance;
    public List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;


    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public void ResetPool()
    {
        foreach (GameObject item in pooledObjects)
        {
            item.SetActive(false);
        }
    }

    public GameObject GetPooledObject(string tag, bool activate = false)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i] != null)
            {
                if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
                {
                    pooledObjects[i].SetActive(activate);
                    return pooledObjects[i];
                }
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(activate);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    //Gets a tagged object from the pool, positions it, can activate it and set a lifeTime if desired
    public GameObject QuickSpawn(string tag, Vector3 position, bool activate = true,  float lifeTime = 0, Quaternion rotation = new Quaternion())
    {
        GameObject obj = GetPooledObject(tag, activate);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        if (lifeTime > 0)
        {
            ObjectDestroyer.instance.DeactivateThis(obj, lifeTime);
        }
        return obj;
    }
}

