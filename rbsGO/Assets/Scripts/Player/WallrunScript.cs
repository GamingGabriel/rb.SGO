using UnityEngine;

public class WallrunScript : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask wall;
    public LayerMask ground;
    public float wallrunSpeed;
    public float maxWallrunTime;
    [SerializeField]
    float wallrunTimer;

    [Header("Input")]
    private float horInput; 
    private float vertInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight; 
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    public bool wallLeft;
    private bool wallRight;

    [Header("Refrence")]
    public Transform orientation;
    [SerializeField]
    private PlayerStateManager stateManager;


    void Start()
    {
        stateManager = GetComponent<PlayerStateManager>();
    }

    void Update()
    {
        CheckForWall();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, wall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, ground);
    }


}
