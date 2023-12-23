using UnityEngine;

[DefaultExecutionOrder(10)]
public abstract class ResourceDisplay : MonoBehaviour
{
    [SerializeField] protected ResourceRenderer[] _renderers;
}
