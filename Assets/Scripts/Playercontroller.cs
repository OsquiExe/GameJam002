using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;
    public float playerSpeed;
    public InputAction moveAction;
    private Vector2 moveInput;
    [SerializeField]private GameObject golpePrefab;
    public InputAction attackAction;

    private float activateInterval = 1.5f;
    private float waitActivateAttack = 0.0f;

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

        transform.Translate(Vector3.right * moveInput.x * Time.deltaTime * playerSpeed, Space.World);
        transform.Translate(Vector3.up * moveInput.y * Time.deltaTime * playerSpeed, Space.World);  
    }

    void Attack()
    {
        if (waitActivateAttack <= 0)
        {
            StartCoroutine(ActivateHit());
            waitActivateAttack = activateInterval;
        }
        
    }    
    IEnumerator ActivateHit()
    {
        golpePrefab.SetActive(true);
        yield return new WaitForSeconds(activateInterval);
        golpePrefab.SetActive(false);
    }
}