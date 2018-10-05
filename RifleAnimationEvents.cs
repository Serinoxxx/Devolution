using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleAnimationEvents : MonoBehaviour {

    public GameObject[] bulletTex;
    public GameObject bulletTrail;
    public Item item;

    //public void Shoot()
    //{
    //    gameObject.GetComponentInChildren<ParticleSystem>().Play();

    //    Vector3 fwd = transform.TransformDirection(Vector3.forward);
    //    RaycastHit hit;
    //    Debug.DrawRay(transform.position, fwd * 10, Color.green);

    //    GameObject trail = Instantiate(bulletTrail, transform.position, transform.rotation);
    //    trail.GetComponent<LineRenderer>().SetPosition(0, transform.position);
    //    trail.GetComponent<LineRenderer>().SetPosition(1, transform.position + fwd * 10.0f);

    //    if (Physics.Raycast(transform.position, fwd, out hit, 50f))
    //    {

    //        //Check that we're not hitting another bullethole!!
    //        //if (hit.collider.tag != "BulletHole")
    //        //{
    //            //Get the surface type of the collider to determine which bullethole to spawn
    //            Surface surface = hit.collider.gameObject.GetComponent<Surface>();
    //            int bulletTextIndex;
    //            if (surface != null)
    //            {
    //                switch (surface.surface)
    //                {
    //                    case Surface.SurfaceType.metal:
    //                        bulletTextIndex = 0;
    //                        break;
    //                    case Surface.SurfaceType.concrete:
    //                        bulletTextIndex = 1;
    //                        break;
    //                    case Surface.SurfaceType.glass:
    //                        bulletTextIndex = 2;
    //                        break;
    //                    case Surface.SurfaceType.wood:
    //                        bulletTextIndex = 3;
    //                        break;
    //                    case Surface.SurfaceType.flesh:
    //                        bulletTextIndex = 4;
    //                        break;
    //                    default:
    //                        bulletTextIndex = 0;
    //                        break;
    //                }
    //            }
    //            else
    //            {
    //                //Metal by default
    //                bulletTextIndex = 0;
    //            }

    //            GameObject bulletHole = Instantiate(bulletTex[bulletTextIndex], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
    //            bulletHole.transform.SetParent(hit.collider.gameObject.transform);
    //            bulletHole.transform.Rotate(Vector3.up, Random.Range(0, 360));

    //            if (hit.collider.gameObject.GetComponent<Health>())
    //        {
    //            hit.collider.gameObject.GetComponent<Health>().TakeDamage(item.damage, gameObject);

    //        }
    //            //if (hit.collider.gameObject.GetComponent<Ragdollifier>())
    //            //{
    //            //    hit.collider.gameObject.GetComponent<Ragdollifier>().Ragdollify(Vector3.zero);
    //            //}
    //    }
    //}
}
