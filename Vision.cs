using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour {

    public List<GameObject> visibleCreatures;
    private void Start()
    {
        visibleCreatures = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>())
        {
            visibleCreatures.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Health>())
        {
            visibleCreatures.Remove(other.gameObject);
        }
    }
}
