using UnityEngine;
using System.Collections;

namespace EnemyStates
{
    public class TargetLost : EnemyState
    {
        [SerializeField] private float _confusedTime = 1f;
        [SerializeField] private GameObject _targetLostFace;

        private float _movement = 0f;
        private WaitForSeconds _confusedTimer;
        private Coroutine _confusedRoutine;

        private void OnValidate()
        {
            _confusedTime = Mathf.Max(_confusedTime, 0f);

            _confusedTimer = new WaitForSeconds(_confusedTime);
        }

        public override void Enter()
        {
            _targetLostFace.SetActive(true);

            _owner.SetInput(_movement);

            _confusedRoutine = StartCoroutine(StayConfused());
        }

        public override void Exit()
        {
            _targetLostFace.SetActive(false);

            StopCoroutine(_confusedRoutine);
        }

        private IEnumerator StayConfused()
        {
            yield return _confusedTimer;

            ChangeState("Patrol");
        }
    }
}
