using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    // Server-side variables
    [SerializeField] private float speed = 5f;
    private Vector2 movementInput;
    private NetworkVariable<int> health = new NetworkVariable<int>(5);
    [SerializeField] private GameObject bulletPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        transform.Translate(movementInput * speed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;
        MoveRpc(ctx.ReadValue<Vector2>());
    }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (!IsOwner || !ctx.started) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 shootDir = (mousePos - transform.position).normalized;
        ShootRpc(shootDir);
    }

    [Rpc(SendTo.Server)]
    public void MoveRpc(Vector2 movement)
    {
        movementInput = movement;
    }

    [Rpc(SendTo.Server)]
    public void ShootRpc(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.direction = direction;
        bulletComponent.clientId = OwnerClientId;
        bullet.GetComponent<NetworkObject>().Spawn();
        Destroy(bullet, 5f);
    }

    public void Damage()
    {
        if (!IsServer) return;
        health.Value -= 1;
        if (health.Value <= 0)
        {
            Destroy(gameObject);
        }
    }
}
