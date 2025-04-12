using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    [Header("States")]
    [HideInInspector]
    public PlayerBaseState currentState;

    [HideInInspector]
    public PlayerIdleState idleState = new PlayerIdleState();

    [HideInInspector]
    public PlayerRunState runState = new PlayerRunState();

    [HideInInspector]
    public PlayerJumpState jumpState = new PlayerJumpState();

    [HideInInspector]
    public PlayerWallrunState wallrunState = new PlayerWallrunState();

    [Header("Input")]
    public CharacterController controller;

    public Vector2 movement;

    [Header("Physics")]
    public float speed; //The current DESIRED speed

    Vector3 velocity; //for jumping? Unsure 
    
    [SerializeField]
    float gravity = -10;

    [Header("Camera")]
    Vector2 mouseMovement;

    [SerializeField] 
    GameObject cam;    
    float cameraUpRotation = 0;

    float mouseSensitivity = 50;

    [Header("Weapon")]
    [SerializeField]
    GunScript gun;

    [SerializeField]
    GameObject throwable;

    [SerializeField]
    Transform throwPoint;

    [SerializeField]
    float throwForce;

    
    //public WallrunScript wallrunScript;

    [Header("Wallrunning")]
    public LayerMask wall;
    public LayerMask ground;
    public float wallrunSpeed;
    public float maxWallrunTime;
    [SerializeField]
    float wallrunTimer;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight; 
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    public bool wallLeft;
    public bool wallRight;

    [SerializeField]
    float WALLRUN_SPEED;  

    [Header("Movement")]
    public float MOVE_SPEED;
    [SerializeField]

    float jumpHeight = 2;

    [SerializeField]
    bool canSprint;

    public bool wallrunning;

    [SerializeField]
    float lastSprint;

    [SerializeField]
    float DASH_COOLDOWN; //The gap between dashes

    [SerializeField]
    float DASH_SPEED; //The maximum speed of your dash


    //[SerializeField]
    //float DASH_RATE; //The rate you will reach your dash's max speed

    [SerializeField]
    float DASH_DURATION;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        SwitchState(idleState);
        canSprint = true;

    }

    void Update()
    {
        HandleCamera(mouseSensitivity);
        currentState.UpdateState(this);
        if (!canSprint)
        {
            if (Mathf.Abs(lastSprint - Time.time) >= DASH_COOLDOWN) // if the difference between the last Sprint and now is greater than 5
            {
                canSprint = true;
            }

            if (Mathf.Abs(lastSprint - Time.time) >= DASH_DURATION) // if the difference between the last Sprint and now is greater than 5
            {
                speed = MOVE_SPEED;
                gravity = -10;
            }
        }
        CheckForWall();
        Gravity();
    }

    void OnMove(InputValue moveVal)
    {
        movement = moveVal.Get<Vector2>();
    }

    void OnJump()
    {
        //hasJumped = true;
        if (controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);  
                //isGrounded = false;  
            }
    }  

    void Gravity()
    {

        //isGrounded = Physics.CheckSphere(groundCheck.position, .2f, groundLayer);
        
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    void OnLook(InputValue LookVal)
    {
        mouseMovement = LookVal.Get<Vector2>();
    }

    void OnAttack()
    {
        gun.Shoot();
        //Debug.Log("shooting");
    }

    void OnThrow()
    {
        GameObject thrown = Instantiate(throwable, throwPoint.position, Quaternion.Euler(Vector3.forward));
        thrown.GetComponent<ThrowScript>().Throw(throwPoint.forward, throwForce);
    }

    void HandleCamera(float sense)
    {
        float lookX = mouseMovement.x;
        float lookY = mouseMovement.y * Time.deltaTime * mouseSensitivity;

        cameraUpRotation -= lookY;

        cameraUpRotation = Mathf.Clamp(cameraUpRotation, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(cameraUpRotation, 0, 0);

        transform.Rotate(Vector3.up * lookX);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    void OnSprint()
    {
        if (canSprint)
        {
            velocity.y = 0;
            lastSprint = Time.time; //Sets the time of lastSprint to time of input
            canSprint = false; //sets the ability to sprint to false            
            /*while (speed <= DASH_SPEED)
                {
                    speed += DASH_RATE * Time.deltaTime;
                }
            */
            speed = DASH_SPEED;
            gravity = 0;
        }
    }
    private void CheckForWall()
    { //*
        wallRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, wallCheckDistance, wall);
        Debug.DrawRay(transform.position, transform.right * wallCheckDistance, Color.red, .5f);
        wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallHit, wallCheckDistance, wall);
        Debug.DrawRay(transform.position, -transform.right * wallCheckDistance, Color.red, .5f);

        Debug.DrawRay(transform.position, Vector3.down * minJumpHeight, Color.blue, .5f);
    }

    public bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, ground);
    }


    
}
