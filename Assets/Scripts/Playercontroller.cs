using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;
    public float playerSpeed;
    public InputAction moveAction;
    private Vector2 moveInput;
    [SerializeField] private GameObject golpePrefab;
    public InputAction attackAction;

    private float activateInterval = 1.5f;
    private float waitActivateAttack = 0.0f;

    // Última dirección en la que te moviste (por defecto, hacia arriba)
    private Vector2 lastDirection = Vector2.up;

    void Start()
    {
        moveAction.Enable();
        attackAction.Enable();
    }

    void Update()
    {
        MovePlayer();
        if (waitActivateAttack > 0)
        {
            waitActivateAttack -= Time.deltaTime;
        }
        else if (attackAction.WasPressedThisFrame())
        {
            Attack();
        }
    }

    void MovePlayer()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        // Solo actualizamos la dirección si realmente hay input
        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastDirection = moveInput.normalized;
        }

        transform.Translate(Vector3.right * moveInput.x * Time.deltaTime * playerSpeed, Space.World);
        transform.Translate(Vector3.up * moveInput.y * Time.deltaTime * playerSpeed, Space.World);
    }

    void Attack()
    {
        if (waitActivateAttack <= 0)
        {
            AimHit();
            StartCoroutine(ActivateHit());
            waitActivateAttack = activateInterval;
        }
    }

    void AimHit()
    {
        // Ángulo de lastDirection respecto a "arriba" (que es la orientación por defecto del golpe)
        float angle = Vector2.SignedAngle(Vector2.up, lastDirection);

        // Rotamos el golpe sobre el eje Z
        golpePrefab.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    IEnumerator ActivateHit()
    {
        golpePrefab.SetActive(true);
        yield return new WaitForSeconds(activateInterval);
        golpePrefab.SetActive(false);
    }
}