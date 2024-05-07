using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 1000f;
    public float damage = 5f;

    private void Start()
    {
        Destroy(this.gameObject, 5f);   // Destroi a bala depois de 5 segundos
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed * Time.deltaTime;    // Lancamento da bala
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pawn")     // Checa se acertou um pawn
        {
            float randNumber = Random.Range(-2, 3);
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(damage + randNumber);     // Aplica dano se o objeto implementar a interface
        }

        Destroy(this.gameObject);
    }
}
