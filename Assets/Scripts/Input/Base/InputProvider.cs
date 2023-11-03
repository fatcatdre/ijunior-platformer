using UnityEngine;

public abstract class InputProvider : MonoBehaviour
{
    public virtual float Movement { get; } = 0f;
    public virtual bool JumpPressed { get; } = false;

    public bool HasMoveInput => Mathf.Approximately(Movement, 0f) == false;
}
