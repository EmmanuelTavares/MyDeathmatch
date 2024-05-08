using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class AIController : MonoBehaviour
{
    public enum EState
    {
        Idle,
        Patrolling,
        Chasing,
        Shooting,
        Running,
        Dead
    }

    [SerializeField] private Transform[] waypoints;
    [SerializeField] private BoxCollider baseArea;
    private NavMeshAgent agent;
    private Transform player = null;
    private EState currentState;

    private void Start()
    {  
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    
        UpdateState(EState.Patrolling);     // Comeca como patrulhando
    }

    private void UpdateState(EState state)
    {
        currentState = state;
        switch(state)
        {
            case EState.Dead:
                break;
            case EState.Running:
                break;
            case EState.Shooting:
                break;
            case EState.Chasing:
                Debug.Log("Perseguindo");
                StartCoroutine(OnChasing());
                break;
            case EState.Patrolling:
                Debug.Log("Patrulhando");
                StartCoroutine(OnPatrolling());
                break;
            case EState.Idle:
                Debug.Log("Idle");
                StartCoroutine(OnIdle());
                break;
            default: Debug.Log("Sem estado definido!");
                break;
        }
    }

    private IEnumerator OnIdle()
    {         
        while (!IsPlayerSeen(10f))  // Checa se viu o jogador a cada 10 milisegundos
        {
            yield return new WaitForSeconds(.1f);
        }
        UpdateState(EState.Chasing);
    }

    private IEnumerator OnPatrolling()
    {
        if (waypoints != null)
        {
            while (currentState == EState.Patrolling)
            {
                yield return new WaitForSeconds(2f);    // Delay de 2 segundos antes de comecar

                int randPoint = Random.Range(0, waypoints.Length);      // Escolhe destino randomico
                agent.SetDestination(waypoints[randPoint].position);

                while (!InDestination())    // Enquanto nao chega no destino 
                {
                    if (IsPlayerSeen(10f))      // Checa se viu o jogador a cada 10 milisegundos
                    {
                        UpdateState(EState.Chasing);
                        break;
                    }
                    yield return new WaitForSeconds(.1f);
                }

                if (currentState != EState.Patrolling) { break; }

                Debug.Log("Chegou ao destino");
            }          
        }
        else    // Se nao tiver waypoints, entra no estado de idle
        {
            Debug.Log("Nao ha waypoints!");
            UpdateState(EState.Idle);
        }
    }

    private IEnumerator OnChasing()
    {
        agent.SetDestination(player.position);

        while (!InDestination())
        {
            agent.SetDestination(player.position);
            yield return new WaitForSeconds(.1f);
        } 
    }

    private bool InDestination()
    {
        // Retorna verdade se distancia que falta for menor que a distancia de parar e se nao ja tiver caminho a percorrer
        return agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending;
    }

    private bool IsPlayerSeen(float detectionRange)
    {
        if (Vector3.Distance(player.position, this.transform.position) < detectionRange) {  return true; } 

        return false;
    }
}
