using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAreaBehavior : MonoBehaviour
{
    private bool bTriggering;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Inimigo entrou na base");
        bTriggering = true;
        StartCoroutine(OnHealing(other.gameObject));
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Inimigo saiu na base");
        bTriggering = false;
    }

    private IEnumerator OnHealing(GameObject other)
    {
        while (bTriggering) 
        {
            other.GetComponent<IDamageable>().Heal(5);
            Debug.Log("Inimigo foi curado");
            yield return new WaitForSeconds(1f);
        } 
        
        yield return null;
    }
}
