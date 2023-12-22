using UnityEngine;
using System.Collections;

namespace EnemyStates
{
    public class Dead : EnemyState
    {
        [SerializeField] private float _despawnTime = 1f;
        [SerializeField] private GameObject _deadFace;

        private WaitForSeconds _despawnTimer;

        private int _isDying = Animator.StringToHash("isDying");

        public override void Enter()
        {
            _deadFace.SetActive(true);

            _owner.SetInput(0f);

            _despawnTime = Mathf.Max(_despawnTime, 0f);
            _despawnTimer = new WaitForSeconds(_despawnTime);

            _owner.Animator.SetBool(_isDying, true);

            StartCoroutine(Despawn());
        }

        private IEnumerator Despawn()
        {
            yield return _despawnTimer;

            Destroy(_owner.gameObject);
        }
    }
}
