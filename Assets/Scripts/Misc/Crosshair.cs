using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        transform.position = mousePosition;
    }
}
