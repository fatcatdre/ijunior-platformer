using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _despawnAnimationTime;
    [SerializeField] private float _selfDespawnTime;

    private Animator _animator;
    private Collider2D _collider;

    private bool _shouldMove = true;
    private bool _isDespawning = false;

    private WaitForSeconds _despawnTimer;
    private WaitForSeconds _selfDespawnTimer;

    private readonly int _hit = Animator.StringToHash("hit");

    public int Damage => _damage;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();

        _despawnTimer = new WaitForSeconds(_despawnAnimationTime);
        _selfDespawnTimer = new WaitForSeconds(_selfDespawnTime);

        StartCoroutine(DespawnAfterTime());
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IProjectileInteractable interactable))
            interactable.Interact(this);
    }

    public void Despawn()
    {
        if (_isDespawning)
            return;

        _isDespawning = true;

        StartCoroutine(PlayDespawnAnimation());
    }

    private void Move()
    {
        if (_shouldMove)
            transform.Translate(_speed * Time.deltaTime * Vector2.right);
    }

    private IEnumerator PlayDespawnAnimation()
    {
        _collider.enabled = false;
        _shouldMove = false;
        _animator.SetBool(_hit, true);

        yield return _despawnTimer;

        Destroy(gameObject);
    }

    private IEnumerator DespawnAfterTime()
    {
        yield return _selfDespawnTimer;

        Despawn();
    }
}
