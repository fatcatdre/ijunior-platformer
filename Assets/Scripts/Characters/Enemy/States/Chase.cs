using UnityEngine;
using System.Collections;

namespace EnemyStates
{
    public class Chase : EnemyState
    {
        [SerializeField] private GameObject _chaseFace;

        [Header("Player Detection")]
        [SerializeField] private float _initialPlayerDetectionRadius = 9f;
        [SerializeField] private float _chasePlayerDetectionRadius = 12f;
        [SerializeField] private float _scanDelay = 0.1f;

        [Header("Attack Settings")]
        [SerializeField] private float _attackRange = 2f;
        [SerializeField] private int _attackDamage = 1;
        [SerializeField] private float _attackDelay = 0.5f;

        private float _movement = 1f;
        private WaitForSeconds _playerDetectionTimer;
        private Coroutine _playerChaseRoutine;

        private Player _player;

        private float _attackDelayRemaining;

        private void OnValidate()
        {
            _scanDelay = Mathf.Max(_scanDelay, 0f);
            _attackRange = Mathf.Max(_attackRange, 0f);
            _attackDelay = Mathf.Max(_attackDelay, 0f);
            _attackDamage = Mathf.Max(_attackDamage, 0);

            _playerDetectionTimer = new WaitForSeconds(_scanDelay);
        }

        private void OnDrawGizmosSelected()
        {
            Vector2 origin = new(transform.position.x, transform.position.y);

            Gizmos.DrawWireSphere(origin, _initialPlayerDetectionRadius);
            Gizmos.DrawWireSphere(origin, _chasePlayerDetectionRadius);
        }

        public override void Enter()
        {
            _chaseFace.SetActive(true);

            _playerDetectionTimer = new WaitForSeconds(_scanDelay);

            _player = TryFindPlayer(_initialPlayerDetectionRadius);

            if (_player == null)
            {
                ChangeState("TargetLost");
                return;
            }

            _playerChaseRoutine = StartCoroutine(ChasePlayer());
        }

        public override void Process()
        {
            _owner.SetInput(_movement);

            _attackDelayRemaining = Mathf.Max(_attackDelayRemaining - Time.deltaTime, 0f);

            TryAttack(_player);
        }

        public override void Exit()
        {
            _chaseFace.SetActive(false);

            StopCoroutine(_playerChaseRoutine);
        }

        private IEnumerator ChasePlayer()
        {
            while (true)
            {
                float sqrDistanceToPlayer = Vector2.SqrMagnitude(_player.transform.position - _owner.transform.position);

                if (sqrDistanceToPlayer > _chasePlayerDetectionRadius * _chasePlayerDetectionRadius)
                {
                    ChangeState("TargetLost");
                    yield break;
                }

                bool isPlayerOnTheRight = _owner.transform.position.x < _player.transform.position.x;

                _movement = isPlayerOnTheRight ? 1f : -1f;

                yield return _playerDetectionTimer;
            }
        }

        private void TryAttack(Player player)
        {
            float sqrDistanceToPlayer = Vector2.SqrMagnitude(player.transform.position - _owner.transform.position);

            if (sqrDistanceToPlayer < _attackRange * _attackRange)
            {
                if (Mathf.Approximately(_attackDelayRemaining, 0f))
                {
                    player.TakeDamage(_attackDamage);

                    _attackDelayRemaining = _attackDelay;
                }
            }
        }

        private Player TryFindPlayer(float detectionRadius)
        {
            Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

            foreach (Collider2D result in results)
            {
                if (result.TryGetComponent(out Player player))
                    return player;
            }

            return null;
        }
    }
}
