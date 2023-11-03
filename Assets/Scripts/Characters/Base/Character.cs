using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Character : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] protected InputProvider _input;

    [Header("Movement")]
    [SerializeField] protected float _moveSpeed = 12f;
    [SerializeField] protected float _groundAcceleration = 70f;
    [SerializeField] protected float _airAcceleration = 40f;
    [SerializeField] protected float _groundDeceleration = 80f;
    [SerializeField] protected float _airDeceleration = 20f;
    [SerializeField] protected float _minVelocityForMove = 1f;

    [Header("Jump")]
    [SerializeField] protected int _maxExtraJumps = 1;
    [SerializeField] protected float _jumpHeight = 8f;
    [SerializeField] protected float _timeToApex = 0.45f;
    [SerializeField] protected float _timeFromApex = 0.35f;
    [SerializeField] protected float _minVelocityForJump = 1f;
    [SerializeField] protected float _minVelocityForFall = -2f;

    [Header("Ground Check")]
    [SerializeField] protected float _groundCheckDistance = 0.1f;
    [SerializeField] protected float _ceilingBounceVelocity = -2f;
    [SerializeField] protected LayerMask _groundLayers;

    protected Rigidbody2D _rigidbody;
    protected Animator _animator;
    protected Collider2D _collider;
    protected SpriteRenderer _sprite;

    protected float _moveInput;
    protected bool _isGrounded;
    protected int _jumpCount;
    protected Vector2 _targetVelocity;

    protected float _jumpVelocity;
    protected float _jumpGravity;
    protected float _fallGravity;

    protected virtual bool HasMoveInput => _input.HasMoveInput;

    public bool IsGrounded => _isGrounded;
    public bool IsFalling => _rigidbody.velocity.y < _minVelocityForFall;

    protected virtual void Awake()
    {
        CalculateJumpParameters();

        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        ReadInput();

        CheckHeadCollision();
        CalculateGravity();
        CalculateMovement();
        CalculateJump();
        CheckFeetCollision();

        UpdateSpriteAnimation();

        Move();
    }

    protected virtual void OnValidate()
    {
        CalculateJumpParameters();
    }

    protected virtual void ReadInput()
    {
        _moveInput = _input.Movement;
    }

    protected virtual void CheckHeadCollision()
    {
        bool hitCeiling = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, Vector3.up, _groundCheckDistance, _groundLayers);

        if (hitCeiling)
            _targetVelocity.y = _ceilingBounceVelocity;
    }

    protected virtual void CheckFeetCollision()
    {
        _isGrounded = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, Vector2.down, _groundCheckDistance, _groundLayers);

        if (_isGrounded)
        {
            _jumpCount = 0;
            _targetVelocity.y = Mathf.Max(_targetVelocity.y, 0f);
        }
    }

    protected virtual void CalculateMovement()
    {
        if (_isGrounded)
            CalculateMovement(_groundAcceleration, _groundDeceleration);
        else
            CalculateMovement(_airAcceleration, _airDeceleration);
    }

    protected virtual void CalculateMovement(float acceleration, float deceleration)
    {
        if (HasMoveInput)
            _targetVelocity.x = Mathf.MoveTowards(_targetVelocity.x, _moveInput * _moveSpeed, acceleration * Time.deltaTime);
        else
            _targetVelocity.x = Mathf.MoveTowards(_targetVelocity.x, 0f, deceleration * Time.deltaTime);
    }

    protected virtual void CalculateJump()
    {
        if (_input.JumpPressed && (_isGrounded || _jumpCount < _maxExtraJumps))
            Jump();
    }

    protected virtual void Jump()
    {
        _targetVelocity.y = _jumpVelocity;

        if (_isGrounded == false)
            _jumpCount++;
    }

    protected virtual void CalculateGravity()
    {
        if (_isGrounded && _targetVelocity.y < 0f)
        {
            _targetVelocity.y = 0f;
            return;
        }

        if (_rigidbody.velocity.y > 0f)
            _targetVelocity.y += _jumpGravity * Time.deltaTime;
        else
            _targetVelocity.y += _fallGravity * Time.deltaTime;
    }

    protected virtual void UpdateSpriteAnimation()
    {
        if (HasMoveInput)
            _sprite.flipX = _moveInput < 0f;
    }

    protected virtual void Move()
    {
        _rigidbody.velocity = _targetVelocity;
    }

    protected virtual void CalculateJumpParameters()
    {
        _jumpVelocity = 2f * _jumpHeight / _timeToApex;
        _jumpGravity = -2f * _jumpHeight / (_timeToApex * _timeToApex);
        _fallGravity = -2f * _jumpHeight / (_timeFromApex * _timeFromApex);
    }
}
