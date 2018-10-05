using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float _duration = 10f;
    [SerializeField] float _damage = 10f;
    [SerializeField] Enums.DamageImpact _impact = Enums.DamageImpact.none;

    public bool heightLock;
    float _startTime;

    private void OnEnable()
    {
        OnStart();
        _startTime = Time.time;
        StartCoroutine(Fly());
    }

    public Health ownerHealth;

    private IEnumerator Fly()
    {
        Vector3 direction = transform.forward;


        while (Time.time < _startTime + _duration)
        {
            float previousHeight = transform.position.y;
            transform.Translate(direction * _speed * Time.deltaTime);
            if (heightLock)
            {
                transform.position = new Vector3(transform.position.x, previousHeight, transform.position.z);
            }
            yield return null;
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        OnCollision(other);

        if (_damage > 0)
        {
            if (other.gameObject.GetComponent<Health>())
            {
                other.gameObject.GetComponent<Health>().TakeDamage(_damage, ownerHealth.gameObject);
            }
        }

        Debug.Log("Projectile hit " + other.gameObject.name);
        Destroy(gameObject);

    }

    public abstract void OnStart();
    public abstract void OnCollision(Collider other);
}
