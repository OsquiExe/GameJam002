using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour
{
    [SerializeField] private GameObject kissObject;
    [SerializeField] private float kissDuration = 1f;
    [SerializeField] private float separation = 0.6f;

    private bool touchMale = false;
    private bool touchFemale = false;
    private bool kissing = false;

    private PlayerController male;
    private PlayerController female;

    private GameManager gameManager;
    private Spawner spawner;
    private AudioSource kissSound;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        spawner = FindFirstObjectByType<Spawner>();
        kissSound = GameObject.Find("Kiss Sound").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!kissing && touchMale && touchFemale)
        {
            StartCoroutine(KissRoutine());
        }
    }

    IEnumerator KissRoutine()
    {
        kissing = true;
        
        male.SetKissMode(true);
        female.SetKissMode(true);

        GetComponent<SpriteRenderer>().enabled = false;
        kissObject.SetActive(true);

        yield return new WaitForSeconds(kissDuration/3);
        if (kissSound) kissSound.Play();
        yield return new WaitForSeconds(kissDuration * 2 / 3);

        // Posicionarlos a cada lado del corazón
        Vector3 center = transform.position;
        male.transform.position = new Vector3(center.x + separation, center.y, male.transform.position.z);
        female.transform.position = new Vector3(center.x - separation, center.y, female.transform.position.z);

        male.SetKissMode(false);
        female.SetKissMode(false);

        gameManager.AddScore(1000);
        gameManager.AddTime();
        spawner.NextLevel();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chico"))
        {
            touchMale = true;
            male = collision.GetComponent<PlayerController>();
        }
        if (collision.CompareTag("Chica"))
        {
            touchFemale = true;
            female = collision.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (kissing) return; // al desactivar los colliders se dispara el Exit, hay que ignorarlo

        if (collision.CompareTag("Chico")) touchMale = false;
        if (collision.CompareTag("Chica")) touchFemale = false;
    }
}