using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBehavior : MonoBehaviour
{
    public float healthToAdd = 30f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            int randNumber = Random.Range(-5, 6);
            other.gameObject.GetComponent<IDamageable>().Heal(healthToAdd + randNumber);
            Debug.Log("Jogador pegou a pocao");
            Destroy(this.gameObject);
        }       
    }
}
