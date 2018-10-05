using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour {

    [SerializeField] LineRenderer lrWebThread;
    [SerializeField] int numberOfThreads;

    int[] threadPoints;
    Vector3[] vertices;
    SkinnedMeshRenderer mesh;
    GameObject target;
    // Use this for initialization
    void Start () {
        lrWebThread.positionCount = numberOfThreads;
    }

    private void Update()
    {
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;
        Mesh bakedMesh = new Mesh();
        mesh.BakeMesh(bakedMesh);
        for (int i = 0; i < numberOfThreads; i++)
        {
            lrWebThread.SetPosition(i, bakedMesh.vertices[threadPoints[i]]);
        }
    }

    public void WeaveWeb(GameObject targetGO)
    {
        target = targetGO;
        //Find the first enabled skinnedMeshRenderer
        mesh = null;
        foreach (SkinnedMeshRenderer smr in targetGO.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (smr.gameObject.activeInHierarchy)
            {
                mesh = smr;
                break;
            }
        }

        //Couldn't find a mesh to stick to
        if (mesh == null)
            Destroy(this.gameObject);

        vertices = mesh.sharedMesh.vertices;
        threadPoints = new int[numberOfThreads];
        Random.InitState(System.DateTime.Now.Millisecond);
        for (int i = 0; i < numberOfThreads; i++)
        {
            threadPoints[i] = Random.Range(0, vertices.Length);
        }

    }
}
