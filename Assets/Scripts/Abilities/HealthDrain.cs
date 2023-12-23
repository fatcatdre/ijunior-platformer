using UnityEngine;

public class HealthDrain : Ability
{
    [Header("Health Drain Settings")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _drainRange;
    [SerializeField] private int _drainAmount;
    [SerializeField] private Health _ownerHealth;

    private Health _closestDrainTarget;

    private void OnValidate()
    {
        _drainAmount = Mathf.Max(_drainAmount, 0);
    }

    protected override void OnAbilityStart()
    {
        _closestDrainTarget = FindClosestHealth();

        if (_closestDrainTarget != null)
            _lineRenderer.enabled = true;
    }

    protected override void OnAbilityUpdate()
    {
        DrawDrainLine();
    }

    protected override void OnAbilityTick()
    {
        if (_closestDrainTarget == null)
            return;

        _closestDrainTarget.Damage(_drainAmount);
        _ownerHealth.Heal(_drainAmount);
    }

    protected override void OnAbilityFinish()
    {
        _closestDrainTarget = null;
        _lineRenderer.enabled = false;
    }

    private void DrawDrainLine()
    {
        if (_closestDrainTarget == null)
        {
            _lineRenderer.enabled = false;
            return;
        }

        _lineRenderer.SetPosition(0, _ownerHealth.transform.position);
        _lineRenderer.SetPosition(1, _closestDrainTarget.transform.position);
    }

    private Health FindClosestHealth()
    {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, _drainRange);

        foreach (Collider2D result in results)
        {
            if (result.TryGetComponent(out Health health))
            {
                if (health != _ownerHealth)
                    return health;
            }
        }

        return null;
    }
}
