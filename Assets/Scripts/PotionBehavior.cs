using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBehavior : MonoBehaviour
{
    public float healthToAdd = 30f;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colidiu");
        if (other.gameObject.tag == "Pawn")     // Checa se acertou um pawn
        {
            int randNumber = Random.Range(-10, 11);
            other.gameObject.GetComponent<IDamageable>().Heal(healthToAdd + randNumber);     // Adiciona vida se o objeto implementar a interface
            Destroy(this.gameObject);
        }       
    }
}
