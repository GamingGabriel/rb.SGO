using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FirstPersonController : MonoBehaviour
{
    Vector2 movement;

    [SerializeField]
    GameObject player;

    [SerializeField]
    float gravity = -10;

    [SerializeField]
    float jumpHeight = 2;

    //[SerializeField]
    //Boolean isGrounded;

    /*[SerializeField]
    Transform groundCheck;*/

    [SerializeField]
    LayerMask groundLayer;
    
    Vector3 velocity;
    Vector2 mouseMovement;

    float cameraUpRotation = 0;
    float mouseSensitivity = 50;
    CharacterController controller;
    
    [SerializeField] 
    float speed;

    [SerializeField]
    float MOVE_SPEED;

    [SerializeField] 
    GameObject cam;
    
    [SerializeField]
    bool canSprint;

    [SerializeField]
    float lastSprint;

    [SerializeField]
    float DASH_COOLDOWN = 1f; //The gap between dashes

    [SerializeField]
    float DASH_SPEED = 5.0f; //The maximum speed of your dash


    [SerializeField]
    float DASH_RATE = 0.5f; //The rate you will reach your dash's max speed

    [SerializeField]
    float DASH_DECAY = 1f; //the rate your dash Decays


    Vector3 spawnPoint;

    public bool dead;

    Animator anim;

    //[SerializeField]
    //MeshRenderer pistol;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        canSprint = true;
        //isGrounded = true;
        spawnPoint = player.transform.position;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            //Debug.Log(spawnPoint);
            if (speed < MOVE_SPEED)
            {
                speed = MOVE_SPEED;
            }
            
            if (!canSprint)
            {
                if (Mathf.Abs(lastSprint - Time.time) >= DASH_COOLDOWN) // if the difference between the last Sprint and now is greater than 5
                {
                    canSprint = true;
                }
            }        
            float lookX = mouseMovement.x;
            float lookY = mouseMovement.y * Time.deltaTime * mouseSensitivity;

            cameraUpRotation -= lookY;

            cameraUpRotation = Mathf.Clamp(cameraUpRotation, -90, 90);

            cam.transform.localRotation = Quaternion.Euler(cameraUpRotation, 0, 0);

            transform.Rotate(Vector3.up * lookX);

            float moveX = movement.x;
            float moveZ = movement.y;

            Vector3 actual_movement = (transform.forward * moveZ) + (transform.right * moveX);

            /*
            if (hasJumped)
            {
                hasJumped = false;
                actual_movement.y = 10;
            }

            */

            //actual_movement.y -= 10 * Time.deltaTimme;
            controller.Move(actual_movement * Time.deltaTime * speed);

            if (speed > MOVE_SPEED)
                {
                    speed -= DASH_DECAY * Time.deltaTime;
                }
            Gravity();

            print(controller.isGrounded);
        }
    }

    void OnMove(InputValue moveVal)
    {
        movement = moveVal.Get<Vector2>();
    }

    void OnLook(InputValue LookVal)
    {
        mouseMovement = LookVal.Get<Vector2>();
    }



    void OnSprint()
    {
        if (canSprint)
        {
            lastSprint = Time.time; //Sets the time of lastSprint to time of input
            canSprint = false; //sets the ability to sprint to false            
            while (speed <= DASH_SPEED)
                {
                    speed += DASH_RATE * Time.deltaTime;
                }
        }
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

   
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Kill"))
            {
                //velocity = Vector3.zero;
                Debug.Log("Kill");
                SceneManager.LoadScene("FPSGame");
            }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            dead = true;
            Debug.Log("YOU DIED");
        }
    }
}

/*


*/
