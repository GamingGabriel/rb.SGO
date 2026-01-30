using UnityEngine;
using UnityEngine.InputSystem;

public class HandleCamera : MonoBehaviour
{

    float mouseSensitivity;
    
    [Header("Camera")] //What I need
    Vector2 mouseMovement;
    [SerializeField]
    GameObject cam; //Current Camera 
    float cameraUpRotation = 0;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleCam(mouseSensitivity);
    }   

    void OnLook(InputValue LookVal)
    {
        mouseMovement = LookVal.Get<Vector2>();
    }
    void HandleCam(float sense)
    {
        float lookX = mouseMovement.x * Time.deltaTime * mouseSensitivity;
        float lookY = mouseMovement.y * Time.deltaTime * mouseSensitivity;

        cameraUpRotation -= lookY;

        cameraUpRotation = Mathf.Clamp(cameraUpRotation, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(cameraUpRotation, 0, 0); //Manually adjusting the camera -- probably messing with it.

        transform.Rotate(Vector3.up * lookX);
    }
}
