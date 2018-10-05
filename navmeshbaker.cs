using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class navmeshbaker : MonoBehaviour {

    public NavMeshSurface surf;

	// Use this for initialization
	void Start () {
        surf.BuildNavMesh();
	}

}
