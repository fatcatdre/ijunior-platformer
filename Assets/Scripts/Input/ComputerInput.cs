using UnityEngine;

[RequireComponent(typeof(Character))]
public class ComputerInput : InputProvider
{
    [SerializeField] private float _edgeDetectionDistance = 2f;
    [SerializeField] private LayerMask _groundLayers;

    private Character _character;
    private float _movement = 1f;
    private bool _jumpPressed;

    public override float Movement => _movement;
    public override bool JumpPressed => _jumpPressed;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private void Update()
    {
        CheckPlatformEdges();
    }

    private void CheckPlatformEdges()
    {
        if (_character.IsFalling)
            return;

        Vector2 origin = new(transform.position.x, transform.position.y);

        bool rightEdgeDetected = Physics2D.OverlapBox(origin + Vector2.down + (Vector2.right * _edgeDetectionDistance), Vector2.one, 0f, _groundLayers) == null;
        bool leftEdgeDetected = Physics2D.OverlapBox(origin + Vector2.down + (Vector2.left * _edgeDetectionDistance), Vector2.one, 0f, _groundLayers) == null;

        if (rightEdgeDetected && leftEdgeDetected)
        {
            _movement = 0f;
            return;
        }

        if (rightEdgeDetected)
            _movement = -1f;
        
        if (leftEdgeDetected)
            _movement = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = new(transform.position.x, transform.position.y);
        Gizmos.DrawWireCube(origin + Vector2.down + (Vector2.right * _edgeDetectionDistance), Vector2.one);
        Gizmos.DrawWireCube(origin + Vector2.down + (Vector2.left * _edgeDetectionDistance), Vector2.one);
    }
}
