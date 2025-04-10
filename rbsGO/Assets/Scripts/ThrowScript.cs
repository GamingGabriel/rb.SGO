using UnityEngine;

public class ThrowScript : MonoBehaviour
{
    [SerializeField]
    Rigidbody body;

    [SerializeField]
    bool thrown;
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
    void Update()
    {
        
    }

    public void Throw(Vector3 direction, float force)
    {
        //body.AddForce(Vector3.Scale(body.transform.forward, Vector3.up).normalized * force, ForceMode.VelocityChange);
        body.AddForce(direction * force);
        Destroy(gameObject, 2f);
    }
}
