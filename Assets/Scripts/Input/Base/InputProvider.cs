using UnityEngine;

[RequireComponent(typeof(Character))]
public abstract class InputProvider : MonoBehaviour
{
    protected float _movement;
    protected bool _jumpPressed;

    protected Character _character;

    protected virtual void Awake()
    {
        _character = GetComponent<Character>();
    }

    protected virtual void Update()
    {
        _character.SetInput(_movement);

        if (_jumpPressed)
            _character.TryJump();
    }
}
