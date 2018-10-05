using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBut : MonoBehaviour {
    [SerializeField] float _projectileSpeed;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] Item _item;
    public void OnWebShot()
    {
        Projectile projectile = ObjectPooler.instance.QuickSpawn("webShotProjectile", _spawnPoint.position, activate: true, rotation: transform.rotation).GetComponent<Projectile>();
        projectile.ownerHealth = _item.owner.GetComponent<Health>();
    }
}
