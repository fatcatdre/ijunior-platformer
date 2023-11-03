using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectable : MonoBehaviour, ICollectable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ICollector collector))
        {
            collector.Collect(this);
        }
    }

    public void OnCollected(ICollector collector)
    {
        Destroy(gameObject);
    }
}
