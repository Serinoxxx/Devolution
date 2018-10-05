using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShot : Projectile {

    [SerializeField] float webDuration = 5f;
    [SerializeField] GameObject webPfb;

    public override void OnCollision(Collider other)
    {
        if (other.GetComponent<Movement>())
        {
            other.GetComponent<Movement>().addLock(webDuration);
            GameObject webGO = Instantiate(webPfb, other.transform.position, Quaternion.identity);
            webGO.GetComponent<Web>().WeaveWeb(other.gameObject);
            ObjectDestroyer.instance.DestroyThis(webGO, webDuration);
        }
    }

    public override void OnStart()
    {
        heightLock = true;
    }
}
