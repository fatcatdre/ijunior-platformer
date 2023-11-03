using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Collectable _prefab;

    private bool _isFull;

    public delegate void OnSpawnedHandler(SpawnPoint spawnPoint);
    public delegate void OnEmptiedHandler(SpawnPoint spawnPoint);

    public event OnSpawnedHandler OnSpawned;
    public event OnEmptiedHandler OnEmptied;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ICollectable _))
        {
            _isFull = true;

            OnSpawned?.Invoke(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out ICollectable _))
        {
            _isFull = false;

            OnEmptied?.Invoke(this);
        }
    }

    public void Spawn()
    {
        if (_isFull)
            return;

        Instantiate(_prefab, transform.position, Quaternion.identity);
    }
}
