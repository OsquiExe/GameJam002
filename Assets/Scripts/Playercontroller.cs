using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;
    public float playerSpeed;
    public InputAction moveAction;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    [SerializeField]private GameObject golpePrefab;
    private Vector2 positionToLook;
    public InputAction attackAction;

    private float activateInterval = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable();
        attackAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        if (attackAction.WasPressedThisFrame())
        {
            Attack();
        }   
    }

    void MovePlayer()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        transform.Translate(Vector3.right * moveInput.x * Time.deltaTime * playerSpeed, Space.World);
        transform.Translate(Vector3.up * moveInput.y * Time.deltaTime * playerSpeed, Space.World);  
    }

    void Attack()
    {
        StartCoroutine(ActivateHit()); 
    }    
    IEnumerator ActivateHit()
    {
        golpePrefab.SetActive(true);
        yield return new WaitForSeconds(activateInterval);
        golpePrefab.SetActive(false);
    }
}