using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Transform[] patrolPoints;
    private int indexPatrol;
    private NavMeshAgent agent;
    [SerializeField] private float stuckTime = 1.5f;
    private float stuckTimer;

    private int vida = 1;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.avoidancePriority = Random.Range(30, 70);
    }

    // Lo llama el spawner justo después de instanciar al enemigo
    public void Init(Transform[] points, float speed)
    {
        patrolPoints = points;
        agent.speed = speed;
        GoToRandomPoint();
    }

    void Update()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        if (!agent.isOnNavMesh) return;
        if (agent.pathPending) return;

        // Llegó al destino
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                GoToRandomPoint();
                stuckTimer = 0f;
            }
            return;
        }

        // Va en camino pero casi no avanza: probablemente atascado
        if (agent.velocity.sqrMagnitude < 0.01f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= stuckTime)
            {
                GoToRandomPoint();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
    }

    private void GoToRandomPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        indexPatrol = Random.Range(0, patrolPoints.Length);
        agent.destination = patrolPoints[indexPatrol].position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerAttack"))
        {
            vida--;
            if(vida <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
