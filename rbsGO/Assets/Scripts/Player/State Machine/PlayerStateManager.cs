using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerBaseState currentState;

    [HideInInspector]
    public PlayerIdleState idleState = new PlayerIdleState();

    [HideInInspector]
    public PlayerRunState runState = new PlayerRunState();

    [HideInInspector]
    public PlayerJumpState jumpState = new PlayerJumpState();

    public CharacterController controller;

    public Vector2 movement;

    public float speed;

    Vector3 velocity; //for jumping? Unsure 

    [SerializeField]

    float jumpHeight = 2;
    
    [SerializeField]
    float gravity = -10;

    Vector2 mouseMovement;

    [SerializeField] 
    GameObject cam;    
    float cameraUpRotation = 0;

    float mouseSensitivity = 50;

    [SerializeField]
    GunScript gun;

    [SerializeField]
    GameObject throwable;

    [SerializeField]
    Transform throwPoint;

    [SerializeField]
    float throwForce;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        SwitchState(idleState);

    }

    void Update()
    {
        HandleCamera(mouseSensitivity);
        currentState.UpdateState(this);
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


    
}
