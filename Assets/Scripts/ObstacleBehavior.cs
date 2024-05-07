using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 3f;
    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = this.transform.position;     // Salva primeira posicao como a ultima
    }

    private void FixedUpdate()
    {
        this.transform.Translate(direction * speed * Time.deltaTime);   // Move obstaculo

        if (lastPosition == this.transform.position)    // Checa se a ultima posicao e a mesma que a atual
        {
            Debug.Log("Parou");
            direction *= -1;    // Muda de direcao
        }

        lastPosition = this.transform.position;     // Salva a ultima posicao
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")     // Checa se colidiu com a parede
        {
            //Debug.Log("Colidiu");
            direction *= -1;
        }      
    }
}
