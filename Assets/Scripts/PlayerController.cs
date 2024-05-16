using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Camera cam;

    private void Update()
    {
        // Move o agente para a posicao clicada com botao esquerdo do mouse
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GetComponent<NavMeshAgent>().SetDestination(hit.point);
            }
        }

        // Atira para a posicao clicada com botao direito do mouse
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(hit.point - this.transform.position), 1000f * Time.deltaTime);    // Rotaciona em direacao ao clique
            }

            GetComponent<PawnBehavior>().Shoot();
        }
    }
}
