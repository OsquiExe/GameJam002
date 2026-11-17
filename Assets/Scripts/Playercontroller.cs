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
    [SerializeField] private GameObject colliderGolpePrefab;
    public InputAction attackAction;
    private GameManager gameManager;
    private Animator animator;

    private bool isTrapped = false;
    private float activateInterval = 0.5f;
    private float waitActivateAttack = 0.0f;

    // Última dirección en la que te moviste (por defecto, hacia arriba)
    private Vector2 lastDirection = Vector2.up;

    private SpriteRenderer spriteRenderer;

    [Header("Sounds")]
    public AudioSource attackSound;
    public AudioSource trappedSound;
    public AudioSource freedSound;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
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
        if(animator != null)
        {
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);
        }



        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastDirection = moveInput.normalized;

            // Solo voltear cuando hay movimiento horizontal claro
            if (Mathf.Abs(moveInput.x) > 0.2f)
            {
                if ((CompareTag("Chica") && gameManager.PlayersTrapped > 0) == false)
                {
                    spriteRenderer.flipX = moveInput.x < 0;
                }
                else
                {
                    spriteRenderer.flipX = moveInput.x > 0;
                }
            }
        }
        if (CompareTag("Chica") && gameManager.PlayersTrapped > 0)
        {
            transform.Translate(Vector3.left * moveInput.x * Time.deltaTime * playerSpeed, Space.World);
        }
        else
        {
            transform.Translate(Vector3.right * moveInput.x * Time.deltaTime * playerSpeed, Space.World);
        }
        
        transform.Translate(Vector3.up * moveInput.y * Time.deltaTime * playerSpeed, Space.World);
    }

    void Attack()
    {
        if (waitActivateAttack <= 0)
        {
            AimHit();
            if(attackSound) attackSound.Play();
            PlayAttackAnimation();
            StartCoroutine(ActivateHit());
            waitActivateAttack = activateInterval;
        }
    }

    public void SetKissMode(bool active)
    {
        if (active)
        {
            moveAction.Disable();
            attackAction.Disable();
        }
        else
        {
            moveAction.Enable();
            attackAction.Enable();
        }

        spriteRenderer.enabled = !active;
        GetComponent<Collider2D>().enabled = !active; // para que ningún enemigo los atrape en pleno beso
    }

    void PlayAttackAnimation()
    {
        if (animator == null) return;

        Vector2 dir = lastDirection;

        // Quedarnos solo con el eje dominante (arriba, abajo o lado)
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            dir = new Vector2(Mathf.Sign(dir.x), 0f);
        else
            dir = new Vector2(0f, Mathf.Sign(dir.y));

        animator.SetFloat("AttackX", Mathf.Abs(dir.x));
        animator.SetFloat("AttackY", dir.y);
        animator.SetTrigger("Attack");
        
    }

    void AimHit()
    {
        // Ángulo de lastDirection respecto a "arriba" (que es la orientación por defecto del golpe)
        float angle = Vector2.SignedAngle(Vector2.up, lastDirection);
        if (CompareTag("Chica") && gameManager.PlayersTrapped > 0)
        {
            angle = Vector2.SignedAngle(Vector2.down, lastDirection);
        }
        

        // Rotamos el golpe sobre el eje Z
        golpePrefab.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    IEnumerator ActivateHit()
    {
        golpePrefab.SetActive(true);
        yield return new WaitForSeconds(activateInterval/2);
        colliderGolpePrefab.SetActive(true);
        yield return new WaitForSeconds(activateInterval / 2);
        golpePrefab.SetActive(false);
        colliderGolpePrefab.SetActive(false);
    }
    public void Trapped()
    {
        if (!isTrapped)
        {
            if(trappedSound) trappedSound.Play();
            animator.SetBool("Trapped", true);
            gameManager.PlayerTrapped();
            isTrapped = true;
            spriteRenderer.color = Color.lightBlue;
            moveAction.Disable();
            attackAction.Disable();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Trapped();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Chico") || collision.gameObject.CompareTag("Chica"))
        {
            gameManager.PlayerFreed();
            if (freedSound && isTrapped) freedSound.Play();
            isTrapped = false;
            animator.SetBool("Trapped", false);
            if (CompareTag("Chico"))
            {
                spriteRenderer.color = Color.white;
            }
            else if (CompareTag("Chica"))
            {
                spriteRenderer.color = Color.white;
                
            }
            moveAction.Enable();
            attackAction.Enable();
        }
    }
}