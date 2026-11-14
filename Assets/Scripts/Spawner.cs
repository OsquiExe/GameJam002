using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Quadrant
{
    public string name;                 // Solo para identificarlo en el Inspector
    public int enemyCount = 3;          // Cuántos enemigos se generan aquí
    public Vector2 spawnCenter;         // Centro de la zona de spawn (posición en el mundo)
    public Vector2 spawnSize = new Vector2(10f, 10f);
    public Transform[] patrolPoints;    // Puntos de patrulla de ESTE cuadrante
}

public class Spawner : MonoBehaviour
{


    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Quadrant[] quadrants = new Quadrant[4];
    [SerializeField] private float navMeshSearchRadius = 3f;
    [SerializeField] private LayerMask obstacleMask; 
    [SerializeField] private float checkRadius = 0.4f;

    [Header("Dificultad")]
    [SerializeField] private float baseSpeed = 2f;
    [SerializeField] private float speedIncrease = 0.5f;
    [SerializeField] private float maxSpeed = 8f;

    private int level = 0;
    private List<GameObject> enemies = new List<GameObject>();
    private float CurrentSpeed => Mathf.Min(baseSpeed + speedIncrease * level, maxSpeed);

    void Start()
    {
        foreach (Quadrant q in quadrants)
        {
            SpawnQuadrant(q);
        }
    }

    public void NextLevel()
    {
        level++;

        foreach (GameObject e in enemies)
            if (e != null) Destroy(e);
        enemies.Clear();

        foreach (Quadrant q in quadrants)
            SpawnQuadrant(q);
    }

    private void SpawnQuadrant(Quadrant q)
    {
        if (q.patrolPoints == null || q.patrolPoints.Length == 0)
        {
            Debug.LogWarning($"El cuadrante '{q.name}' no tiene puntos de patrulla.");
            return;
        }

        for (int i = 0; i < q.enemyCount; i++)
        {
            bool found = false;

            for (int attempt = 0; attempt < 30 && !found; attempt++)
            {
                Vector3 pos = GetRandomPosition(q);

                if (!NavMesh.SamplePosition(pos, out NavMeshHit hit, navMeshSearchRadius, NavMesh.AllAreas))
                    continue;

                // Segunda verificación contra el Tilemap Collider
                if (Physics2D.OverlapCircle(hit.position, checkRadius, obstacleMask))
                    continue;

                GameObject obj = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
                enemies.Add(obj);
                obj.GetComponent<Enemy>().Init(q.patrolPoints, CurrentSpeed);
                found = true;
            }

            if (!found)
                Debug.LogWarning($"No se encontró posición válida en '{q.name}'.");
        }
    }

    private Vector3 GetRandomPosition(Quadrant q)
    {
        float x = Random.Range(q.spawnCenter.x - q.spawnSize.x / 2f, q.spawnCenter.x + q.spawnSize.x / 2f);
        float y = Random.Range(q.spawnCenter.y - q.spawnSize.y / 2f, q.spawnCenter.y + q.spawnSize.y / 2f);
        return new Vector3(x, y, 0f);
    }

    // Dibuja las zonas de spawn en la Scene view para ubicarlas fácil
    private void OnDrawGizmosSelected()
    {
        if (quadrants == null) return;

        Gizmos.color = Color.red;
        foreach (Quadrant q in quadrants)
        {
            Gizmos.DrawWireCube(q.spawnCenter, new Vector3(q.spawnSize.x, q.spawnSize.y, 0.1f));
        }
    }
}
