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

    Vector2 movement;

    Vector2 mouseMovement;

    [SerializeField] 
    GameObject cam;    
    float cameraUpRotation = 0;

     float mouseSensitivity = 50;

    void Start()
    {

    }

    void Update()
    {
        HandleCamera(mouseSensitivity);
    }

    void OnMove(InputValue moveVal)
    {
        movement = moveVal.Get<Vector2>();
    }

    void OnLook(InputValue LookVal)
    {
        mouseMovement = LookVal.Get<Vector2>();
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
    
}
