using UnityEngine;

public class HumanInput : InputProvider
{
    protected override void Update()
    {
        _movement = Input.GetAxisRaw("Horizontal");
        _jumpPressed = Input.GetButtonDown("Jump");

        base.Update();
    }
}
