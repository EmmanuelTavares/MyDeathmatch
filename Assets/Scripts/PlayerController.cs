using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Camera cam;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))    // "Escuta" o botao esquerdo do mouse ser clicado
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);    // Pega a posicao do mouse na tela
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))      // Checa se acerta alguma coisa
            {
                GetComponent<NavMeshAgent>().SetDestination(hit.point);     // Move o agente para a posicao clicada
            }
        }

        if (Input.GetMouseButtonDown(1))    // "Escuta" o botao direito do mouse ser clicado
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);    // Pega a posicao do mouse na tela
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))      // Checa se acerta alguma coisa
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(hit.point - this.transform.position), 1000f * Time.deltaTime);    // Rotaciona em direacao ao clique
            }

            GetComponent<PawnBehavior>().Shoot();   // Atira
        }
    }
}
