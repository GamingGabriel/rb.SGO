using UnityEngine;

public class BulletTrailScript : MonoBehaviour
{
    public GameObject trail;

    public Vector3 spawnPoint;
    public Vector3 endPoint;

    [SerializeField]
    float duration;

    private float elapsedTime = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = transform.position;
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);
            float currentPosition = elapsedTime / duration; 
            transform.position = Vector3.Lerp(spawnPoint, endPoint, currentPosition);
            //Debug.Log(trail.transform.position);
            //Debug.Log(currentPosition);

            if( elapsedTime >= duration)
            {
                Destroy(gameObject);
            }
        
    }
}
