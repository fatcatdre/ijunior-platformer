using System.Collections;
using UnityEngine;

namespace EnemyStates
{
    public class Patrol : EnemyState
    {
        [Header("Environment")]
        [SerializeField] private float _edgeDetectionDistance = 1.6f;
        [SerializeField] private LayerMask _groundLayers;

        [Header("Player Detection")]
        [SerializeField] private float _playerDetectionRadius = 7f;
        [SerializeField] private float _scanDelay = 0.25f;

        private float _movement = 1f;
        private WaitForSeconds _playerDetectionTimer;
        private Coroutine _playerDetectionRoutine;

        private void OnValidate()
        {
            _scanDelay = Mathf.Max(_scanDelay, 0f);

            _playerDetectionTimer = new WaitForSeconds(_scanDelay);
        }

        private void OnDrawGizmosSelected()
        {
            Vector2 origin = new(transform.position.x, transform.position.y);

            Gizmos.DrawWireCube(origin + Vector2.down + (Vector2.right * _edgeDetectionDistance), Vector2.one);
            Gizmos.DrawWireCube(origin + Vector2.down + (Vector2.left * _edgeDetectionDistance), Vector2.one);

            Gizmos.DrawWireSphere(origin, _playerDetectionRadius);
        }

        public override void Enter()
        {
            _playerDetectionTimer = new WaitForSeconds(_scanDelay);

            _movement = _owner.Sprite.flipX ? -1f : 1f;

            _playerDetectionRoutine = StartCoroutine(ScanForPlayer());
        }

        public override void Process()
        {
            CheckPlatformEdges();

            _owner.SetInput(_movement);
        }

        public override void Exit()
        {
            StopCoroutine(_playerDetectionRoutine);
        }

        private void CheckPlatformEdges()
        {
            if (_owner.IsFalling)
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

        private IEnumerator ScanForPlayer()
        {
            while (true)
            {
                yield return _playerDetectionTimer;

                bool playerFound = TryFindPlayer();

                if (playerFound)
                    ChangeState("Chase");
            }
        }

        private bool TryFindPlayer()
        {
            Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, _playerDetectionRadius);

            foreach (Collider2D result in results)
            {
                if (result.TryGetComponent(out Player _))
                    return true;
            }

            return false;
        }
    }
}

