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
        // Salva a ultima posicao
        lastPosition = this.transform.position; 
    }

    private void FixedUpdate()
    {
        this.transform.Translate(direction * speed * Time.deltaTime);

        // Muda de direcao se o obstaculo parou
        if (lastPosition == this.transform.position)
        {
            //Debug.Log("Parou");
            direction *= -1;
        }

        lastPosition = this.transform.position;
    }
}
