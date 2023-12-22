using UnityEngine;
using FSM;

[RequireComponent(typeof(Health))]
public class Enemy : Character, ICollector, IProjectileInteractable
{
    [Header("Enemy")]
    [SerializeField] private StateMachine _stateMachine;

    private Health _health;
    private int _isRunning = Animator.StringToHash("isRunning");

    private bool _isDead = false;

    protected override void Awake()
    {
        base.Awake();

        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.Change += OnHealthChanged;
    }

    private void OnDisable()
    {
        _health.Change -= OnHealthChanged;
    }

    private void OnHealthChanged(int health, int maxHealth)
    {
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        if (_isDead)
            return;

        _isDead = true;
        _stateMachine.ChangeState("Dead");
    }

    protected override void UpdateSpriteAnimation()
    {
        base.UpdateSpriteAnimation();

        _animator.SetBool(_isRunning, HasMoveInput);
    }

    public void Collect(ICollectable collectable)
    {
        collectable?.OnCollected(this);
    }

    public void Interact(Projectile projectile)
    {
        if (_isDead)
            return;

        _health.Damage(projectile.Damage);

        projectile.Despawn();
    }
}
