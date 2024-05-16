using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


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

    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform[] basePoints;

    private NavMeshAgent agent;
    private Transform player = null;
    private PawnBehavior pawn;
    private EState currentState;

    private void Start()
    {  
        agent = this.GetComponent<NavMeshAgent>();      // Salva a variavel nav mesh agent

        player = GameObject.FindGameObjectWithTag("Player").transform;      // Salva o transform do jogador

        pawn = this.GetComponent<PawnBehavior>();
    
        UpdateState(EState.Patrolling);     // Comeca como patrulhando
    }

    private void UpdateState(EState state)      // Metodo que altera o estado
    {
        currentState = state;   // Atualiza o estado atual

        switch(currentState)   // Implementa metodos dependendo do estado atual
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
        if (IsPlayerSeen(10f)) { UpdateState(EState.Chasing); }
        else if (IsHealthLow()) { UpdateState(EState.RunningAway); }

        yield return new WaitForSeconds(.1f);

        UpdateState(EState.Idle);
    }

    private IEnumerator OnPatrolling()
    {
        if (waypoints != null)      // Checa se tem ponto para ir
        {
            while (currentState == EState.Patrolling)   // Enquanto estiver patrulhando...
            {
                yield return new WaitForSeconds(2f);    // Delay de 2 segundos antes

                int randPoint = Random.Range(0, waypoints.Length);      // Escolhe destino randomico e defini como destino
                agent.SetDestination(waypoints[randPoint].position);

                while (!InDestination())    // Enquanto nao chega no destino...
                {
                    if (IsPlayerSeen(10f))      // Checa se viu o jogador a cada 0.1 segundo
                    {
                        UpdateState(EState.Chasing);    // Passa para estado de perseguindo
                        break;
                    }
                    else if (IsHealthLow())
                    {
                        UpdateState(EState.RunningAway);
                        break;
                    }
                    yield return new WaitForSeconds(.1f);
                }

                if (currentState != EState.Patrolling) { break; }   // Quebra o loop se ja nao mais tiver patrulhando

                Debug.Log("Chegou ao destino");
            }          
        }
        else    // Se nao tiver pontos para ir, entra no estado de idle
        {
            Debug.Log("Nao ha waypoints!");
            UpdateState(EState.Idle);
        }
    }

    private IEnumerator OnChasing()
    {
        agent.SetDestination(player.position);      // Defini posicao do jogador como destino

        while (!InDestination() && currentState == EState.Chasing)    // Fica definindo a posicao do jogador como destino a cada 0.1 segundo
        {
            agent.SetDestination(player.position);

            if (IsPlayerSeen(5f))    // Checa se jogador esta proximo a cada 0.1 segundo
            {
                UpdateState(EState.Shooting);       // Passa para estado de atirando
                break;
            }

            if (currentState != EState.Chasing) { break; }      // Quebra o loop se ja nao mais tiver perseguindo
              
            yield return new WaitForSeconds(.1f);
        } 
    }

    private IEnumerator OnShooting()
    {   
        // Para, mira no jogador e atira
        agent.SetDestination(this.transform.position);
        this.transform.rotation = Quaternion.LookRotation(player.position - this.transform.position);
        GetComponent<PawnBehavior>().Shoot();
        Debug.Log("Atirou");

        // Troca para cacando depois de 1 segundo
        yield return new WaitForSeconds(1f);
        UpdateState(EState.Chasing);
    }

    private IEnumerator OnRunningAway()
    {
        // Escolhe uma base pra ir
        int randPoint = Random.Range(0, basePoints.Length);
        agent.SetDestination(basePoints[randPoint].position);

        // Fica no loop enquanto nao chega no destino ou a vida ainda esta baixa
        while (!InDestination() || IsHealthLow()) { yield return new WaitForSeconds(.5f); }

        UpdateState(EState.Chasing);
    }

    private bool InDestination()
    {
        // Retorna verdade se distancia que falta for menor que a distancia de parar e se nao ja tiver caminho a percorrer
        return agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending;
    }

    private bool IsPlayerSeen(float detectionRange)
    {
        if (Vector3.Distance(player.position, this.transform.position) < detectionRange)    // Checa se jogador esta dentro da area de deteccao
        {
            RaycastHit hit;
            Vector3 playerDirection = (player.position - this.transform.position).normalized;

            // Cria uma linha da ia para o jogador, se bater em algo antes, nao viu o jogador
            if (Physics.Raycast(this.transform.position, playerDirection, out hit, detectionRange))
            {            
                if (hit.transform == player) 
                {
                    Debug.Log("Viu jogador");
                    return true; 
                }
                else { Debug.Log(hit.collider.gameObject.name); }
            }
        }
        return false;
    }

    private bool IsHealthLow()
    {
        return pawn.currentHealth < 30f;
    }
}
