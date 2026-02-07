using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStateManager : MonoBehaviour
{
    [Header("STATES")]
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

    [Header("INPUT")]
    public CharacterController controller; //Refrence to the Unity Character Controller. 

    public Vector2 movement; //Movement direction derived from input system

    [SerializeField]
    float mouseSensitivity;

    [Header("PHYSICS")]
    public float speed; //The current DESIRED speed

    public Vector3 velocity; //for jumping? Unsure 
    
    public float gravity = -10; //CURRENT of gravity for the player 
    
    public float BASE_GRAVITY; //BASE of gravity for the player 

    public float WALL_GRAVITY; //Gravity for the player WHEN WALLRUNNING

    [Header("CAMERA")] 
    Vector2 mouseMovement; //Mouse movement direction, derived from OnLook

    [SerializeField]
    GameObject cam; //Cinemachine Camera Target -- NOT THE ACTUAL CAMERA

    [SerializeField]
    GameObject camSystem; //Cinemachine Cameras Header Object

    [SerializeField]
    CinemachineCamera currentCamera; //Current Cinemachine Camera -- will be updated depending on state, from the array

    [SerializeField]
    CinemachineCamera[] cameras; //Array to hold all the cameras in camSystem

    
    
    float cameraUpRotation = 0; //Camera Up Rotataion

    [Header("WEAPON")]
    [SerializeField]
    GunScript gun; // the gun lol

    [SerializeField]
    bool canShoot; //variable which dicates you can shoot

    [SerializeField]
    float lastShot; //last shot fired; used to check if the gap between this and the current time >= canShoot

    [SerializeField]
    float fireRate; //delay between shots

    [Header("THROW")]
    [SerializeField]
    GameObject currentHeld; //Currently held throwable object

    [SerializeField]
    bool isHolding; //Bool to check if an object is already held

    [SerializeField]
    Transform throwPoint; //Transform to get the position of where player will hold object

    [SerializeField]
    float throwForce; //Force of thrown object

    public Vector3 storedMovement; //Prototype to make it so thrown objects inheret player velocity. 

    
    //public WallrunScript wallrunScript;

    [Header("WALLRUN")]
    public LayerMask wall; //LayerMask to detect Walls specifically
    public LayerMask ground; //LayerMask to detect the ground, ergo if the player is high enough to wallrun
    public float WALLRUN_SPEED; //Wallrun Speed
    public float maxWallrunTime; //Maximum duration of wallrun. NOTE: PLAYER CAN RESET TIMER ON SAME WALL -- MUST FIX
    [SerializeField]
    float wallrunTimer; //Current duration of wallrun

    public bool walljump; //Bool to determine if a jump is a jump or a walljump

    [Header("DETECTION")]
    public float wallCheckDistance; //Distance of wall raycast
    public float minJumpHeight;  // Distance of downward ratycaycast to check if player is high enough to wallrun
    private RaycastHit leftWallHit; //Storage of the raycast that detects if left wall is hit
    private RaycastHit rightWallHit; //Storage of the raycast that detects if left wall is hit
    public bool wallLeft; //Bool to dedect if left wall is hit
    public bool wallRight; //Bool to dedect if right wall is hit

    [SerializeField]
    Vector3 respawn; //Current respawn point

    [Header("MOVEMENT")]
    public float MOVE_SPEED; //BASE movement speed
    [SerializeField]
    float jumpHeight = 2; //Jump heigh; can change depending on state

    [SerializeField]
    bool canSprint; //Bool to see if the player can sprint

    public bool wallrunning; //Bool to see if the player is currently wallrunning. Different than seeing if they are ABLE to;

    [SerializeField]
    float lastSprint; //Time of last sprint;

    [SerializeField]
    float DASH_COOLDOWN; //The gap between dashes

    [SerializeField]
    float DASH_SPEED; //The maximum speed of your dash


    //[SerializeField]
    //float DASH_RATE; //The rate you will reach your dash's max speed

    [SerializeField]
    float DASH_DURATION;

    [Header("UI")]

    [SerializeField]
    Image DashBar; //Refrence to the dash bar fill image

    [SerializeField]
    float dashCharge; //Current dash energy

    [SerializeField]
    float maxDash; //Max energy

    [SerializeField]
    float dashCost;//Dash energy cost

    [SerializeField]
    float dashRegenRate;//Dash energy regeneration rate

    [SerializeField]
    Canvas canvas;


    //_________________________________________________________________________________________________________________________________


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        SwitchState(idleState);
        canSprint = true;
        BASE_GRAVITY = -10;
        isHolding = false;
        canShoot = true;
        respawn = transform.position;
        walljump = false;
        //canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();    
        DashBar = GameObject.Find("DashCharge").GetComponent<Image>();
        camSystem = GameObject.Find("CameraSystem");
        cameras = camSystem.GetComponentsInChildren<CinemachineCamera>();
        currentCamera = cameras[0];
    }
    


    void Update()
    {
        if (!controller.enabled)
        {
            controller.enabled = true;
        }
       
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
                if (wallrunning)
                {
                    speed = WALLRUN_SPEED;
                    gravity = WALL_GRAVITY;
                }
                else
                {
                    speed = MOVE_SPEED;
                    gravity = -10;
                }  
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
        
        if (dashCharge < 100)
        {
            dashCharge += dashRegenRate * Time.deltaTime;
            DashBar.fillAmount = dashCharge / maxDash;
        }
        Gravity();
    }


    //_________________________________________________________________________________________________________________________________

    
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
            if (!walljump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -gravity);  
                walljump = true;
            }
        }
    }  

    void OnPickup()
    {
        RaycastHit pickup;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out pickup, 100, LayerMask.GetMask("Bounce")))
        {

            if (!isHolding)
            {
            currentHeld = pickup.transform.gameObject;
            currentHeld.GetComponent<ThrowScript>().PickUp();
            currentHeld.transform.SetParent(throwPoint);
            currentHeld.transform.position = throwPoint.transform.position;
            currentHeld.layer = LayerMask.NameToLayer("On Top");
            isHolding = true;
            }
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
        if (isHolding)
        {
            currentHeld.GetComponent<ThrowScript>().Throw(throwPoint.forward, storedMovement, throwForce);
            isHolding = false;
        }
    }

    void HandleCamera(float sense)
    {
        float lookX = mouseMovement.x * Time.deltaTime * mouseSensitivity;
        float lookY = mouseMovement.y * Time.deltaTime * mouseSensitivity;

        cameraUpRotation -= lookY;

        cameraUpRotation = Mathf.Clamp(cameraUpRotation, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(cameraUpRotation, 0, 0); //Manually adjusting the camera -- probably messing with it.

        transform.Rotate(Vector3.up * lookX);
    }

    public void SwitchCamera(int n)
    {
        currentCamera.Priority = 0;
        cameras[n].Priority = 1;
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
            if (dashCharge >= dashCost)
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
                dashCharge -= dashCost;
                if (dashCharge < 0)
                {
                    dashCharge = 0;
                }
                DashBar.fillAmount = dashCharge / maxDash; 
            }
        }
    }
    private void CheckForWall()
    { 
        wallRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, wallCheckDistance, wall); //Chekcs for left wall
        wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallHit, wallCheckDistance, wall); //checks for right wall
        Debug.DrawRay(transform.position, transform.right * wallCheckDistance, Color.red, .5f);
        Debug.DrawRay(transform.position, -transform.right * wallCheckDistance, Color.red, .5f);
        Debug.DrawRay(transform.position, Vector3.down * minJumpHeight, Color.blue, .5f); //checks  if player is high enough above the ground
    }

    public bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, ground);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            respawn = other.transform.position;
        }
        if (other.CompareTag("Kill"))
        {
            print("respawn");
            controller.enabled = false;
            transform.position = respawn;
        }
        
    }


}
