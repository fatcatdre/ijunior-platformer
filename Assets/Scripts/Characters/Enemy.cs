public class Enemy : Character, ICollector
{
    protected override void UpdateSpriteAnimation()
    {
        base.UpdateSpriteAnimation();

        _animator.SetBool("isRunning", HasMoveInput);
    }

    public void Collect(ICollectable collectable)
    {
        collectable?.OnCollected(this);
    }
}
