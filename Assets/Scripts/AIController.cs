using System.Collections;
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
        RunningAway,
    }

    [SerializeField] private EState currentState;
    [SerializeField] private float chaseDetectionRange = 10f;
    [SerializeField] private float shootDetectionRange = 5f;
    [SerializeField] private float patrollingDelay = 2f;
    [SerializeField] private float shootingDelay = 1f;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform[] basePoints;

    private NavMeshAgent agent;
    private Transform player = null;
    private PawnBehavior pawn;

    private void Start()
    {  
        // Definicoes iniciais
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pawn = this.GetComponent<PawnBehavior>();
    
        UpdateState(EState.Patrolling);
    }

    private void UpdateState(EState state)
    {
        currentState = state;

        switch(currentState)
        {
            case EState.Idle:
                Debug.Log("Idle");
                StartCoroutine(OnIdle());
                break;
            case EState.Patrolling:
                Debug.Log("Patrulhando");
                StartCoroutine(OnPatrolling());
                break;
            case EState.Chasing:
                Debug.Log("Perseguindo");
                StartCoroutine(OnChasing());
                break;
            case EState.Shooting:
                Debug.Log("Atirando");
                StartCoroutine(OnShooting());
                break;
            case EState.RunningAway:
                Debug.Log("Fugindo");
                StartCoroutine(OnRunningAway());
                break;
            default: Debug.Log("Sem estado definido!");
                break;
        }
    }

    private IEnumerator OnIdle()
    {
        // Fica no estado de alerta enquanto estiver em idle
        while (currentState == EState.Idle)
        {
            if (IsHealthLow()) { UpdateState(EState.RunningAway); }
            else if (IsPlayerSeen(chaseDetectionRange)) { UpdateState(EState.Chasing); }

            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator OnPatrolling()
    {
        // Patrulha se tiver pontos de destino
        if (waypoints != null)
        {
            // Escolhe um ponto de destino pra ir
            int randPoint = Random.Range(0, waypoints.Length);
            agent.SetDestination(waypoints[randPoint].position);

            // Fica no loop enquanto nao chega no destino e permanece no estado de patrulha
            while (!InDestination() && currentState == EState.Patrolling)
            {
                if (IsHealthLow()) { UpdateState(EState.RunningAway); }
                else if (IsPlayerSeen(chaseDetectionRange)) { UpdateState(EState.Chasing); }

                yield return new WaitForSeconds(.1f);
            }

            // Reinicia patrulha se ainda estiver no estado de patrulhando
            if (currentState == EState.Patrolling) 
            {
                yield return new WaitForSeconds(patrollingDelay);
                UpdateState(EState.Patrolling); 
            }
        }
        else
        {
            Debug.Log("Nao ha waypoints!");
            UpdateState(EState.Idle);
        }
    }

    private IEnumerator OnChasing()
    {
        agent.SetDestination(player.position);

        // Atualiza o destino enquanto estiver perseguindo
        while (currentState == EState.Chasing)
        {
            if (IsHealthLow()) { UpdateState(EState.RunningAway); }
            else if (IsPlayerSeen(shootDetectionRange)) { UpdateState(EState.Shooting); }

            yield return new WaitForSeconds(.2f);

            agent.SetDestination(player.position);
        } 
    }

    private IEnumerator OnShooting()
    {   
        // Para, mira no jogador e atira
        agent.SetDestination(this.transform.position);
        this.transform.rotation = Quaternion.LookRotation(player.position - this.transform.position);
        GetComponent<PawnBehavior>().Shoot();
        Debug.Log("Atirou");

        // Volta para estado de cacando
        yield return new WaitForSeconds(shootingDelay);
        UpdateState(EState.Chasing);
    }

    private IEnumerator OnRunningAway()
    {
        // Escolhe um ponto da base pra ir
        int randPoint = Random.Range(0, basePoints.Length);

        // Fica no loop enquanto nao chega no destino ou a vida ainda esta baixa
        while (!InDestination() || IsHealthLow()) 
        {
            agent.SetDestination(basePoints[randPoint].position);
            yield return new WaitForSeconds(.5f); 
        }

        UpdateState(EState.Chasing);
    }

    private bool InDestination()
    {
        // Retorna verdade se distancia que falta for menor que a distancia de parar e se nao ja tiver caminho a percorrer
        return agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending;
    }

    private bool IsPlayerSeen(float detectionRange)
    {
        // Procura ver jogador se este esta dentro da area de deteccao
        if (Vector3.Distance(player.position, this.transform.position) < detectionRange)
        {
            RaycastHit hit;
            Vector3 playerDirection = (player.position - this.transform.position).normalized;

            if (Physics.Raycast(this.transform.position, playerDirection, out hit, detectionRange))
            {            
                if (hit.transform == player) 
                {
                    Debug.Log("Viu jogador");
                    return true; 
                }
                //else { Debug.Log(hit.collider.gameObject.name); }
            }
        }

        return false;
    }

    private bool IsHealthLow()
    {
        return pawn.currentHealth < 30f;
    }
}
