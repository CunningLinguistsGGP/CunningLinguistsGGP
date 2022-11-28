using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 20.0f, lifetime = 10.0f,explosionSize=5.0f,explosionDamage = 20.0f;
    [SerializeField] private bool explodes = false,gravity=false;
    [SerializeField] GameObject explosion = null;
    [SerializeField] LayerMask targetLayer;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = gravity;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            Destroy(this.gameObject);
    }

    private void Explode()
    {
        if (explosion != null)
        {
            GameObject newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        }
        Collider[] targets = Physics.OverlapSphere(transform.position, explosionSize, targetLayer, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < targets.Length; i++)
        {
            Target target = targets[i].GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(explosionDamage);
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("here");
        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }
        if (explodes)
            Explode();
        Destroy(this.gameObject);
    }
}
