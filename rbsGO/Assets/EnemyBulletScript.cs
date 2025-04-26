using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{

    [SerializeField]
    float speed;
    [SerializeField]
    float spawnTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       spawnTime = Time.time; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0f, 0f, speed * Time.deltaTime));
    }
}
