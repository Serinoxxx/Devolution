using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
    public Color color;
    public Enums.SporeColors sporeColor;
    public AudioClip clip;
    public float soundPitch = 1f;

    private float flySpeed = 10f;
    private Transform target;
    private float timeAwoken;
    private float timeTillAbsorbable = 1f;
    void OnEnable()
    {
        Debug.Log("Spore is awake!");
        Initialize();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        target = null;
    }

    private void Initialize()
    {
        timeAwoken = Time.time;
        StartCoroutine(spawning());
        GetComponent<Rigidbody>().isKinematic = false;
    }


    private IEnumerator spawning()
    {
        yield return new WaitForSeconds(timeTillAbsorbable);

        GetComponent<Rigidbody>().isKinematic = true;

       foreach( Collider col in Physics.OverlapSphere(transform.position, gameObject.GetComponents<SphereCollider>()[1].radius * transform.localScale.x, LayerMask.GetMask("Default")))
        {
            Absorb(col);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (Time.time > timeAwoken + timeTillAbsorbable)
        {
            if (target == null)
            {
                Absorb(other);
            }
        }

        if (target != null && Vector3.Distance(transform.position, targetPos) < 1f)
        {
            //GameObject.Instantiate(Resources.Load("SporeBurst"), transform.position, Quaternion.identity);
            GameObject sporeBurst = ObjectPooler.instance.GetPooledObject("SporeBurst", activate: true);
            sporeBurst.transform.position = transform.position;

            ObjectDestroyer.instance.DeactivateThis(sporeBurst, 1f);

            GameController.instance.soundManager.PlaySound(clip, soundPitch);

            gameObject.SetActive(false); //Don't destroy, the object pooler will reuse this

            GameController.instance.sporeSystem.AddSpores(sporeColor, 1);
        }
    }

    private void Absorb(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject.transform;
            //gameObject.GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(flyToTarget());
        }
    }


    Vector3 targetPos;
    private IEnumerator flyToTarget()
    {
        targetPos = target.position;
        targetPos.y += 1.3f;
        while (true)
        //while (Vector3.Distance(transform.position, targetPos) > 0.5f)
        {
            targetPos = target.position;
            targetPos.y += 1.3f;
            transform.position += (targetPos - transform.position).normalized * flySpeed * Time.deltaTime;
            yield return null;
        }
        
    }

}
