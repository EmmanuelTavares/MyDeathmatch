using System.Collections;
using System.Collections.Generic;
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

    public EState currentState;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private BoxCollider baseArea;
    private NavMeshAgent agent;

    private void Start()
    {
        //currentState = EState.Idle;
        agent = this.GetComponent<NavMeshAgent>();
        UpdateState();
    }

    private void UpdateState()
    {
        switch(currentState)
        {
            case EState.Dead:
                break;
            case EState.Running:
                break;
            case EState.Shooting:
                break;
            case EState.Chasing:
                break;
            case EState.Patrolling:
                Debug.Log("Patrulhando");
                StartCoroutine(OnPatrolling());
                break;
            case EState.Idle:
                Debug.Log("Idle");
                break;
            default: Debug.Log("Sem estado definido!");
                break;
        }
    }

    private IEnumerator OnPatrolling()
    {
        if (waypoints != null)
        {
            yield return new WaitForSeconds(2f);    // Delay de 2 segundos
            int randPoint = Random.Range(0, waypoints.Length);
            agent.SetDestination(waypoints[randPoint].position);    // Seta destino randomico

            while (!InDestination())
            {
                yield return new WaitForSeconds(.1f);   // A cada 10 milisegundos checa se chegou no destino
            }

            Debug.Log("Chegou ao destino");
            StartCoroutine(OnPatrolling());
        }
        else Debug.Log("Nao ha waypoints!");
    }

    private bool InDestination()
    {
        // Retorna verdade se tem caminho e se a distancia que falta e menor que a distancia de parar
        return agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending;
    }
}
