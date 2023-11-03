using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _delay;

    private List<SpawnPoint> _spawnPoints;
    private WaitForSeconds _spawnDelay;
    private Coroutine _spawnCoroutine;

    private void Awake()
    {
        _spawnPoints = new List<SpawnPoint>(GetComponentsInChildren<SpawnPoint>());

        foreach (SpawnPoint spawnPoint in _spawnPoints)
        {
            spawnPoint.OnSpawned += OnSpawned;
            spawnPoint.OnEmptied += OnEmptied;
        }

        UpdateSpawnDelay();
    }

    private void OnEnable()
    {
        _spawnCoroutine = StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopCoroutine(_spawnCoroutine);
    }

    private void OnValidate()
    {
        UpdateSpawnDelay();
    }

    private void OnSpawned(SpawnPoint spawnPoint)
    {
        if (spawnPoint == null)
            return;

        _spawnPoints.Remove(spawnPoint);
    }

    private void OnEmptied(SpawnPoint spawnPoint)
    {
        if (spawnPoint == null)
            return;

        _spawnPoints.Add(spawnPoint);
    }

    private IEnumerator Spawn()
    {
        while (enabled)
        {
            SpawnPoint spawnPoint = GetRandomSpawnPoint();

            if (spawnPoint != null)
                spawnPoint.Spawn();

            yield return _spawnDelay;
        }
    }

    private SpawnPoint GetRandomSpawnPoint()
    {
        if (_spawnPoints.Count == 0)
            return null;

        return _spawnPoints[Random.Range(0, _spawnPoints.Count)];
    }

    private void UpdateSpawnDelay()
    {
        _spawnDelay = new WaitForSeconds(_delay);
    }
}
