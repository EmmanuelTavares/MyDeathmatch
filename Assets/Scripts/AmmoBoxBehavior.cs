using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxBehavior : MonoBehaviour
{
    public int ammoToAdd = 15;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colidiu");
        if (other.gameObject.tag == "Pawn")     // Checa se acertou um pawn
        {
            int randNumber = Random.Range(-5, 6);
            other.gameObject.GetComponent<IDamageable>().AddAmmo(ammoToAdd + randNumber);     // Adiciona municao se o objeto implementar a interface
            Destroy(this.gameObject);
        }
    }
}
