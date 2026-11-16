using UnityEditor.XR;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private bool touchMale = false;
    private bool touchFemale = false;
    private GameManager gameManager;
    private Spawner spawner;
    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        spawner = FindFirstObjectByType<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(touchMale && touchFemale)
        {
            gameManager.AddScore(1000);
            gameManager.AddTime();
            spawner.NextLevel();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chico"))
        {
            touchMale = true;
        }
        if (collision.CompareTag("Chica"))
        {
            touchFemale = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Chico"))
        {
            touchMale = false;
        }
        if (collision.CompareTag("Chica"))
        {
            touchFemale = false;
        }
    }

}
