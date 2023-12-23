using UnityEngine;

public class HealthDisplay : ResourceDisplay
{
    [SerializeField] protected Health _source;

    private void OnEnable()
    {
        foreach (var renderer in _renderers)
            _source.Changed += renderer.Render;
    }

    private void OnDisable()
    {
        foreach (var renderer in _renderers)
            _source.Changed -= renderer.Render;
    }
}
