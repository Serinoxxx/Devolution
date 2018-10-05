using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour {

	public enum SurfaceType
    {
        metal, concrete, glass, wood, flesh
    };

    public SurfaceType surface;
}
