using Unity.Mathematics;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{   
    [SerializeField]
    float health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    float range;
    [SerializeField]
    float rotationSpeed;

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject spawnPoint;

    [SerializeField]
    GameObject bullet;

    [SerializeField]
    float rateOfFire; 

    [SerializeField]
    float lastShot;



    void Start()
    {
        player = GameObject.FindWithTag("Player");
        lastShot = 0;
    }

    // Update is called once per frame
    void Update()
    {
       

        //print(Vector3.Distance(transform.position, player.transform.position));
        if (Vector3.Distance(transform.position, player.transform.position) < range)
        {
            print("in range");
            transform.LookAt(player.transform);
            //RotateTowardsPlayer();
        } 

        if (Time.time - lastShot > rateOfFire)
        {
            Fire();
        }

    }

    private void RotateTowardsPlayer()
    {
        Vector3 playerDir = player.transform.position - transform.position;
        float rotationStep = rotationSpeed * Time.deltaTime;
        Vector3 newLookDir  = Vector3.RotateTowards(transform.forward, playerDir, rotationStep, 0f);

        transform.rotation = Quaternion.LookRotation(newLookDir);
    }

    private void Fire()
    {
        Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        lastShot = Time.time;
    }
    public void TakeDamage(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
