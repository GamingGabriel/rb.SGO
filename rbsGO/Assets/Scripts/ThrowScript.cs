using UnityEngine;

public class ThrowScript : MonoBehaviour
{
    [SerializeField]
    Rigidbody body;

    [SerializeField]
    Collider coll;

    

   /*[SerializeField]
    bool thrown;

    

    public GameObject player;

    public float pickUpRange;

    public bool held;

    public static bool slotFull;
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void OnAwake()
    {
        body = GetComponent<Rigidbody>();
        if (thrown)
        {
            body.AddForce(Vector3.Scale(body.transform.forward, Vector3.up).normalized * 100);
        }
    }
    */

    // Update is called once per frame
    void OnAwake()
    {
        //player = GameObject.FindWithTag("Player");

    }
    void Update()
    {
        
    }

    public void PickUp()
    {
        body.isKinematic = true;
        coll.isTrigger = true;
    }
   
    public void Throw(Vector3 direction, float force)
    {   
        body.isKinematic = false;
        coll.isTrigger = false;
        transform.SetParent(null);
        //body.AddForce(Vector3.Scale(body.transform.forward, Vector3.up).normalized * force, ForceMode.VelocityChange);
        body.AddForce(direction * force);
        Destroy(gameObject, 2f);
    }
}
