using UnityEngine;

public class RotateScript : MonoBehaviour
{
    
    [SerializeField]
    float rotateSpeed;
    // Update is called once per frame
    void Update()
    {
       transform.Rotate(rotateSpeed, 0, 0); 
    }
}
