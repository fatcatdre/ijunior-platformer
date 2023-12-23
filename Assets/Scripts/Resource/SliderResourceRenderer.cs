using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderResourceRenderer : ResourceRenderer
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _easeSpeed;
    [SerializeField] private bool _isFullAtStart = true;

    private float _targetResource;
    private Coroutine _resourceUpdateCoroutine;

    private void Awake()
    {
        if (_isFullAtStart)
            StartCoroutine(UpdateSliderOnAwake());
    }

    public override void Render(int resource, int maxResource)
    {
        StopAnimation();

        SetMaxResource(maxResource);

        _targetResource = Mathf.Min(_targetResource, maxResource);

        if (_easeSpeed > 0f)
        {
            _resourceUpdateCoroutine = StartCoroutine(UpdateResource(resource));
        }
        else
        {
            SetResource(resource);
            _targetResource = resource;
        }
    }

    private IEnumerator UpdateResource(int resource)
    {
        while (Mathf.Approximately(_targetResource, resource) == false)
        {
            _targetResource = Mathf.MoveTowards(_targetResource, resource, _easeSpeed * Time.deltaTime);
            SetResource(_targetResource);

            yield return null;
        }
    }

    private IEnumerator UpdateSliderOnAwake()
    {
        float originalEaseSpeed = _easeSpeed;
        _easeSpeed = 0f;

        yield return null;

        _easeSpeed = originalEaseSpeed;
    }

    private void StopAnimation()
    {
        if (_resourceUpdateCoroutine != null)
            StopCoroutine(_resourceUpdateCoroutine);
    }

    private void SetResource(float resource)
    {
        _slider.value = resource;
    }

    private void SetMaxResource(float maxResource)
    {
        _slider.maxValue = maxResource;
    }
}
