
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RevolverScript : MonoBehaviour
{
    [SerializeField]
    bool addBulletSpread;

    [SerializeField]
    Vector3 bulletSpread = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField]
    ParticleSystem shootParticle;

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    ParticleSystem impactParticle;

    [SerializeField]
    TrailRenderer bulletTrail;

    [SerializeField]
    float shootDelay = 0.5f;

    [SerializeField]
    LayerMask mask;

    private Animator animator;
    private float lastShot;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        if (lastShot + shootDelay < Time.time)
        {
            
            Vector3 direction = GetDirection();
            Debug.DrawRay(transform.position, transform.forward * 10, Color.magenta);

            if (Physics.Raycast(spawnPoint.position, direction, out RaycastHit hit, float.MaxValue, mask))
            {
                Debug.Log("Trigger");
                TrailRenderer trail = Instantiate(bulletTrail, spawnPoint.position, Quaternion.identity);
                
                StartCoroutine(SpawnTrail(trail, hit));

                lastShot = Time.time;
            }
        }
    }

    private Vector3 GetDirection()
    {  
        Vector3 direction = transform.forward;

        if (addBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-bulletSpread.x, bulletSpread.x),
                Random.Range(-bulletSpread.y, bulletSpread.y),
                Random.Range(-bulletSpread.z, bulletSpread.z)
            );

            direction.Normalize();
        }

        //direction.Normalize();
        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        //Animator line
        trail.transform.position = hit.point;
        Instantiate(impactParticle, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
        Debug.Log("Trail Made");

    }
}
