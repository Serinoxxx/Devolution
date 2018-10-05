using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Ragdollifier : MonoBehaviour {


    public GameObject ragdoll;
    public float mass = .01f;

	// Use this for initialization
	void Start () {
        foreach (Rigidbody rig in ragdoll.GetComponentsInChildren<Rigidbody>())
        {
            rig.isKinematic = true;
            rig.GetComponent<Collider>().enabled = false;
            rig.mass = mass;
            rig.drag = 0.1f;
            rig.angularDrag = 0.2f;
            if (rig.GetComponent<CharacterJoint>()) rig.GetComponent<CharacterJoint>().enableProjection = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Turn a ragdollable character into a ragdoll
    //apply force at the same time (pass Vector3.zero for no force) 
    //duration until ragdoll is disabled and other components enabled, 0 duration means it will not re-enable
    public void Ragdollify(Vector3 force)
    {
        ragdoll.SetActive(true);

        foreach(Rigidbody rig in ragdoll.GetComponentsInChildren<Rigidbody>())
        {
            rig.isKinematic = false;
            rig.GetComponent<Collider>().enabled = true;
        }

        if (gameObject.GetComponent<Collider>())
            gameObject.GetComponent<Collider>().enabled = false;

        if (gameObject.GetComponent<Rigidbody>())
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

        //if (gameObject.GetComponent<ThirdPersonCharacter>())
        //    gameObject.GetComponent<ThirdPersonCharacter>().enabled = !bEnable;

        if (gameObject.GetComponent<NavMeshAgent>())
        {
            gameObject.GetComponent<NavMeshAgent>().SetDestination(transform.position);
            gameObject.GetComponent<NavMeshAgent>().speed = 0f;
        }

        if (gameObject.GetComponent<AICharacterControl>())
            gameObject.GetComponent<AICharacterControl>().enabled = false;

        if (gameObject.GetComponent<Animator>())
        {
            gameObject.GetComponent<Animator>().enabled = false;

        }

        if (force != Vector3.zero)
            ragdoll.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

    }
}
