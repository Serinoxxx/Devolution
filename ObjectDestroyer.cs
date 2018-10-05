using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour {

    public static ObjectDestroyer instance;
	// Use this for initialization
	void Awake () {
        instance = this;
    }
	
	public void DestroyThis(GameObject objectToDestroy, float delay = 0f)
    {
        StartCoroutine(DestroyObject(objectToDestroy, delay));
    }

    private IEnumerator DestroyObject(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(go);
    }

    public void DeactivateThis(GameObject objectToDeactivate, float delay = 0f)
    {
        StartCoroutine(DeactivateObject(objectToDeactivate, delay));
    }

    private IEnumerator DeactivateObject(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }
}
