using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Collectable _prefab;

    private bool _isFull;

    public event Action<SpawnPoint> Spawned;
    public event Action<SpawnPoint> Emptied;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ICollectable _))
        {
            _isFull = true;

            Spawned?.Invoke(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out ICollectable _))
        {
            _isFull = false;

            Emptied?.Invoke(this);
        }
    }

    public void Spawn()
    {
        if (_isFull)
            return;

        Instantiate(_prefab, transform.position, Quaternion.identity);
    }
}
