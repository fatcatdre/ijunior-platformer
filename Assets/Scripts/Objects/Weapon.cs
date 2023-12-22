using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _delayBetweenShots;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Transform _muzzle;

    private Camera _camera;

    private float _shotDelayRemaining;

    private bool CanShoot => _shotDelayRemaining <= 0f;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        _shotDelayRemaining = Mathf.Max(0f, _shotDelayRemaining - Time.deltaTime);

        LookAtCamera();

        if (Input.GetButton("Fire1"))
            Shoot();
    }

    private void LookAtCamera()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        transform.right = direction;
    }

    private void Shoot()
    {
        if (CanShoot == false)
            return;

        _shotDelayRemaining = _delayBetweenShots;

        Instantiate(_projectile, _muzzle.position, transform.rotation);
    }
}
