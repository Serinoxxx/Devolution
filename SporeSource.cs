using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeSource : MonoBehaviour
{
    public Enums.SporeColors type;
    public int count = 10;

    private float hForce = 0.025f;
    private float vForce = 0.05f;

    //Call from Death()
    public void Burst()
    {
        GameObject sporePfb = (GameObject)Resources.Load("Spore");
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y += 1f;
            GameObject spore = ObjectPooler.instance.GetPooledObject("Spore", activate: true);
            spore.transform.position = spawnPos;
            //GameObject.Instantiate(sporePfb, spawnPos, Quaternion.identity);
            Rigidbody rig = spore.GetComponent<Rigidbody>();
            Vector3 force = new Vector3(Random.Range(-hForce, hForce), vForce, Random.Range(-hForce, hForce));
            rig.AddForce(force, ForceMode.Impulse);
            spore.GetComponent<Spore>().sporeColor = this.type;

            switch (type)
            {
                case Enums.SporeColors.red:
                    //spore.GetComponent<Light>().color = Color.red;
                    spore.GetComponent<Renderer>().material.SetColor("_TintColor", Color.red);
                    break;
                case Enums.SporeColors.green:
                    //spore.GetComponent<Light>().color = Color.green;
                    spore.GetComponent<Renderer>().material.SetColor("_TintColor", Color.green);
                    break;
                case Enums.SporeColors.blue:
                    //spore.GetComponent<Light>().color = Color.blue;
                    spore.GetComponent<Renderer>().material.SetColor("_TintColor", Color.blue);
                    break;
            }

        }
    }

}

