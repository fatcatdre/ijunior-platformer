using UnityEngine;

public class HumanInput : InputProvider
{
    private float _movement;
    private bool _jumpPressed;

    public override float Movement => _movement;
    public override bool JumpPressed => _jumpPressed;

    private void Update()
    {
        _movement = Input.GetAxisRaw("Horizontal");
        _jumpPressed = Input.GetButtonDown("Jump");
    }
}
