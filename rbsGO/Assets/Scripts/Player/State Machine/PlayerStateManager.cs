using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    float mouseSensitivity;

    [Header("Physics")]
    public float speed; //The current DESIRED speed

    public Vector3 velocity; //for jumping? Unsure 
    
    public float gravity = -10;
    
    public float BASE_GRAVITY;

    public float WALL_GRAVITY;

    [Header("Camera")]
    Vector2 mouseMovement;

    [SerializeField]
    GameObject cam;    
    float cameraUpRotation = 0;



    [Header("Weapon")]
    [SerializeField]
    GunScript gun;

    [SerializeField]
    bool canShoot; //variable which dicates you can shoot

    [SerializeField]
    float lastShot; //last shot fired; used to check if the gap between this and the current time >= canShoot

    [SerializeField]
    float fireRate; //delay between shots

    [Header("Throw")]
    [SerializeField]
    GameObject currentHeld;

    [SerializeField]
    bool isHolding;

    [SerializeField]
    Transform throwPoint;

    [SerializeField]
    float throwForce;

    public Vector3 storedMovement;

    
    //public WallrunScript wallrunScript;

    [Header("Wallrunning")]
    public LayerMask wall;
    public LayerMask ground;
    public float WALLRUN_SPEED;  
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
        BASE_GRAVITY = -10;
        isHolding = false;
        canShoot = true;

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
        if ((!wallLeft && !wallRight) && AboveGround())
        {
            if (gravity >= BASE_GRAVITY)
            {
                gravity -= .2f;
            }
        }
        if (!canShoot)
        {
            if (Mathf.Abs(lastShot - Time.time) >= fireRate) // if the difference between the last Sprint and now is greater than 5
            {
                canShoot = true;
                gun.anim.ResetTrigger(gun.animName);
            }
        }
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
        else if (wallrunning)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -gravity);  
        }
    }  

    void OnPickup()
    {
        RaycastHit pickup;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out pickup, 100, LayerMask.GetMask("Bounce")))
        {
            
            currentHeld = pickup.transform.gameObject;
            currentHeld.GetComponent<ThrowScript>().PickUp();
            currentHeld.transform.SetParent(throwPoint);
            currentHeld.transform.position = throwPoint.transform.position;
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
        if (canShoot)
        {
            gun.Shoot();
            lastShot = Time.time; //Sets the time of lastShot to time of input
            canShoot = false; //sets the ability to shoot to false       
            gun.anim.SetTrigger(gun.animName);
            //Debug.Log("shooting");
        }
    }

    void OnThrow()
    {
        //GameObject thrown = Instantiate(throwable, throwPoint.position, Quaternion.Euler(Vector3.forward));
        if (currentHeld != null)
        {
            currentHeld.GetComponent<ThrowScript>().Throw(throwPoint.forward, throwForce);
            currentHeld = null;
        }
    }

    void HandleCamera(float sense)
    {
        float lookX = mouseMovement.x * Time.deltaTime * mouseSensitivity;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kill"))
        {
            SceneManager.LoadScene("TestScene");
        }
    }


}
