using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxBehavior : MonoBehaviour
{
    public int ammoToAdd = 15;

    void OnTriggerEnter(Collider other)
    {       
        if (other.gameObject.tag == "Player")
        {
            int randNumber = Random.Range(-3, 4);
            other.gameObject.GetComponent<IDamageable>().AddAmmo(ammoToAdd + randNumber);
            Debug.Log("Jogador pegou municao");
            Destroy(this.gameObject);
        }
    }
}
