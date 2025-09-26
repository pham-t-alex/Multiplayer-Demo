using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float speed = 10f;
    public Vector2 direction;
    public ulong clientId;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer || !IsSpawned) return;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer || !IsSpawned) return;
        Debug.Log("!");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")
            && collision.gameObject.GetComponent<NetworkObject>().OwnerClientId != clientId)
        {
            collision.gameObject.GetComponent<Player>().Damage();
            Destroy(gameObject);
        }
    }
}
