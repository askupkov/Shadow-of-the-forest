using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private float runSpeed = 20f;
    public float speed;
    private Rigidbody2D rb;
    private Vector2 inputVector;
    public VectorValue pos;

    private NavMeshAgent navMeshAgent;
    public bool isMovingToDestination = false;
    private Transform targetDestination;

    public GameObject shadow;
    public GameObject newshadow;
    private int isWalking = 0;
    private int isRunning = 0;
    private bool checkAnimation = true;
    private int lastHorizontalDirection = 1;

    public bool lighting;

    public bool stealth = false;

    private float walkStepInterval = 0.8f;
    private float runStepInterval = 0.4f;
    private bool canPlayFootstep = true;
    private Coroutine footstepCoroutine;
    private bool isRunningState = false;

    public AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.enabled = false;
    }

    private void Start()
    {
        transform.position = pos.initialValue;
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterSfx(audioSource);
        SetFootstepClip();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Inventory.Instance.HasItem(10))
            {
                Candle();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!stealth && checkAnimation)
        {
            HandleMovent();
        }
        else if (stealth)
        {
            HandleMoventStealth();
        }
    }

    private void SetFootstepClip()
    {
        if (SurfaceZone.Instance != null)
        {
            if (SurfaceZone.Instance.surface == "grass")
            {
                audioSource.clip = sounds[0];
            }
            else
            {
                audioSource.clip = sounds[1];
            }
        }
    }

    public void Noice()
    {
        if (Player.Instance.speed == 2 && inputVector != Vector2.zero)
        {
            NoiseManager.Instance.IncreaseNoise(1f * Time.deltaTime);
        }
        else if (Player.Instance.speed == 5 && inputVector != Vector2.zero)
        {
            NoiseManager.Instance.IncreaseNoise(5f * Time.deltaTime);
        }
        else
        {
            NoiseManager.Instance.DecreaseNoise(0.8f * Time.deltaTime);
        }
    }

    public void Candle()
    {
        lighting = !lighting;
        audioSource.PlayOneShot(sounds[2]);
    }

    public void StartDie()
    {
        StartCoroutine(Die());
    }
    private IEnumerator Die()
    {
        GameInput.Instance.OnDisable();
        yield return new WaitForSeconds(0.4f);
        checkAnimation = false;
        shadow.SetActive(false);
        newshadow.SetActive(false);
        PlayerVisual.Instance.TriggerDie();
        yield return new WaitForSeconds(4f);
        GameOver.Instance.ShowGameOverScreen();
        GameInput.Instance.OnDisable();
    }

    private void HandleMovent()
    {
        bool wasRunning = isRunningState;
        if (isMovingToDestination)
        {
            speed = movingSpeed;
            navMeshAgent.SetDestination(targetDestination.position);
            inputVector = ((Vector2)targetDestination.position - rb.position).normalized;
        }
        else
        {
            inputVector = GameInput.Instance.GetMovementVector();
            speed = Input.GetKey(KeyCode.LeftShift) && lighting == false ? runSpeed : movingSpeed;
            rb.MovePosition(rb.position + inputVector * (speed * Time.fixedDeltaTime));
        }

        isRunningState = (speed == runSpeed);
        if (wasRunning != isRunningState)
        {
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
            }
            canPlayFootstep = true;
        }

        UpdateShadow();
        UpdateDirection();


        if (inputVector != Vector2.zero)
        {
            PlayFootsteps();
        }
    }

    private void UpdateShadow()
    {
        if (speed == runSpeed && inputVector.x != 0)
        {
            shadow.SetActive(false);
            newshadow.SetActive(true);
        }
        else
        {
            shadow.SetActive(true);
            newshadow.SetActive(false);
        }
    }

    private void UpdateDirection()
    {
        if (inputVector.x < 0)
        {
            isWalking = 3;
        }
        else if (inputVector.x > 0)
        {
            isWalking = 1;
        }
        else if (inputVector.y > 0)
        {
            isWalking = 4;
        }
        else if (inputVector.y < 0)
        {
            isWalking = 2;
        }
        else
        {
            isWalking = 0;
        }

        isRunning = (speed == runSpeed && inputVector != Vector2.zero) ? isWalking : 0;
    }

    private void PlayFootsteps()
    {
        if (!canPlayFootstep) return;

        if (footstepCoroutine != null)
        {
            StopCoroutine(footstepCoroutine);
        }
        footstepCoroutine = StartCoroutine(PlayFootstepCoroutine());
    }

    private IEnumerator PlayFootstepCoroutine()
    {
        canPlayFootstep = false;

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();

        float stepInterval = (speed == runSpeed) ? runStepInterval : walkStepInterval;

        yield return new WaitForSeconds(stepInterval);

        canPlayFootstep = true;
    }

    private void HandleMoventStealth()
    {
        inputVector = GameInput.Instance.GetMovementVector();
        speed = movingSpeed;
        rb.MovePosition(rb.position + inputVector * (speed * Time.fixedDeltaTime));
        if (inputVector.x < 0)
        {
            isWalking = 3;
            lastHorizontalDirection = isWalking;
        }
        else if (inputVector.x > 0)
        {
            isWalking = 1;
            lastHorizontalDirection = isWalking;
        }
        else if (inputVector.y != 0)
        {
            isWalking = lastHorizontalDirection;
        }
        else
        {
            isWalking = 0;
        }
    }

    public void StartToMove(Transform destination)
    {
        StartCoroutine(MoveToDestination(destination));
    }

    private IEnumerator MoveToDestination(Transform destination)
    {
        GameInput.Instance.OnDisable();
        GameInput.Instance.panelOpen = true;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        isMovingToDestination = true;

        targetDestination = destination;

        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(destination.position);
            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                yield return null;
            }
            navMeshAgent.enabled = false;
        }

        isMovingToDestination = false;
        GameInput.Instance.panelOpen = false;
        GameInput.Instance.OnEnabled();
    }

    public bool IsLighting() => lighting;
    public int IsWalking() => isWalking;
    public int IsRunning() => isRunning;
}
