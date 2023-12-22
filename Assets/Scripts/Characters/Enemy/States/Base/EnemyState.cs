using UnityEngine;
using FSM;

namespace EnemyStates
{
    public abstract class EnemyState : State
    {
        protected Enemy _owner;

        protected override void Awake()
        {
            base.Awake();

            _owner = GetComponentInParent<Enemy>(true);

            if (_owner == null)
                Debug.LogError($"State {name} could not find an Enemy script on any parent objects.");
        }
    }
}
