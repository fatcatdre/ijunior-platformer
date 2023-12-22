using UnityEngine;

namespace FSM
{
    public abstract class State : MonoBehaviour
    {
        protected StateMachine _stateMachine;

        protected virtual void Awake()
        {
            if (transform.parent.TryGetComponent(out _stateMachine) == false)
            {
                Debug.LogError($"{name} needs to be a child of a StateMachine.");
            }
        }

        protected void ChangeState(string newState)
        {
            _stateMachine.ChangeState(newState);
        }

        public virtual void Enter() { }
        public virtual void Process() { }
        public virtual void Exit() { }
    }
}

