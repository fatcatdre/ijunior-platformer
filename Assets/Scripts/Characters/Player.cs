public class Player : Character, ICollector
{
    protected override void UpdateSpriteAnimation()
    {
        base.UpdateSpriteAnimation();

        _animator.SetBool("isRunning", HasMoveInput);
        _animator.SetBool("isJumping", _rigidbody.velocity.y > _minVelocityForJump);
        _animator.SetBool("isFalling", _rigidbody.velocity.y < _minVelocityForFall);
    }

    public void Collect(ICollectable collectable)
    {
        collectable?.OnCollected(this);
    }
}
