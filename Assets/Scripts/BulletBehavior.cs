using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 1000f;
    public float damage = 5f;

    private void Start()
    {
        // Destroi a bala depois de 5 segundos
        Destroy(this.gameObject, 5f);
    }

    private void FixedUpdate()
    {
        // Lancamento da bala
        GetComponent<Rigidbody>().velocity = transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Causa dano se acertou um inimigo ou o jogador
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player")
        {
            float randNumber = Random.Range(-2, 3);
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(damage + randNumber);
        }

        Destroy(this.gameObject);
    }
}
