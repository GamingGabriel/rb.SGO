using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    Transform respawnPoint;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Respawn");
            player.transform.position = respawnPoint.transform.position;
        }
    }
}
