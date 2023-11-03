using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _lead;
    [SerializeField] private float _easing;

    private Vector3 _targetPosition;

    private void Update()
    {
        UpdateTargetPosition();
        UpdateCameraPosition();
    }

    private void UpdateTargetPosition()
    {
        _targetPosition = new Vector3(_target.position.x, _target.position.y, transform.position.z);
    }

    private void UpdateCameraPosition()
    {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, _easing * Time.deltaTime);
    }
}
