    using UnityEngine;
    using UnityEngine.AI;

    public class Enemy : MonoBehaviour
    {
        private Transform[] patrolPoints;
        private int indexPatrol;
        private NavMeshAgent agent;
        [SerializeField] private float stuckTime = 1.5f;
        private float stuckTimer;
        private GameManager gameManager;
        private int vida = 1;

        [Header("Trampas")]
        [SerializeField] private GameObject trapPrefab;
        [SerializeField] private float trapInterval = 5f;
        [SerializeField] private float trapLifetime = 10f; // si es 0 no desaparece
        [SerializeField] private bool placesTraps = false;
        private float trapTimer;

        void Awake()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.avoidancePriority = Random.Range(30, 70);
            vida = gameManager.EnemiesLife;
    }
        public void Init(Transform[] points, float speed, bool placesTraps)
        {
            patrolPoints = points;
            agent.speed = speed;
            this.placesTraps = placesTraps;
            GoToRandomPoint();
        }
        private void HandleTraps()
        {
            if (!placesTraps || trapPrefab == null) return;

            trapTimer += Time.deltaTime;
            if (trapTimer >= trapInterval)
            {
                trapTimer = 0f;
                GameObject trap = Instantiate(trapPrefab, transform.position, Quaternion.identity);
                if (trapLifetime > 0f) Destroy(trap, trapLifetime);
            }
        }


        void Update()
        {
            HandleTraps();

            if (patrolPoints == null || patrolPoints.Length == 0) return;
            if (!agent.isOnNavMesh) return;
            if (agent.pathPending) return;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    GoToRandomPoint();
                    stuckTimer = 0f;
                }
                return;
            }

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
                    gameManager.AddScore(100);
                    gameManager.AddTime(5);
                Destroy(gameObject);
                }
            }
        }
    }
