using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger con: " + collision.name + " tag: " + collision.tag);
        if (collision.CompareTag("Chico") || collision.CompareTag("Chica"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Trapped();
            }
            Destroy(gameObject);
        }
        if (collision.CompareTag("PlayerAttack"))
        {
                Destroy(gameObject);
        }
    }
}
