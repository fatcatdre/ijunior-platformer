using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class Player : Character, ICollector
{
    private int _isRunning = Animator.StringToHash("isRunning");
    private int _isJumping = Animator.StringToHash("isJumping");
    private int _isFalling = Animator.StringToHash("isFalling");

    private Health _health;
    private bool _isDead;

    public event UnityAction Died;

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

    public void TakeDamage(int damage)
    {
        _health.Damage(damage);
    }

    public void Collect(ICollectable collectable)
    {
        collectable?.OnCollected(this);

        if (collectable is Medkit)
            _health.Heal((collectable as Medkit).HealAmount);
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

        Died?.Invoke();
    }

    protected override void UpdateSpriteAnimation()
    {
        base.UpdateSpriteAnimation();

        _animator.SetBool(_isRunning, HasMoveInput);
        _animator.SetBool(_isJumping, _rigidbody.velocity.y > _minVelocityForJump);
        _animator.SetBool(_isFalling, _rigidbody.velocity.y < _minVelocityForFall);
    }
}
