using UnityEngine;
using System.Collections.Generic;

namespace FSM
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private State _initialState;

        private State _currentState;
        private Dictionary<string, State> _states;

        private void Awake()
        {
            AddStates();
        }

        private void Start()
        {
            ChangeState(_initialState.name);
        }

        private void Update()
        {
            if (_currentState != null)
                _currentState.Process();
        }

        public void ChangeState(string newState)
        {
            if (_states.TryGetValue(newState, out State state) == false)
            {
                Debug.LogError($"StateMachine {name} doesn't have a state named {newState}.");
                return;
            }

            if (_currentState == null)
            {
                _currentState = state;
                _currentState.Enter();
            }
            else if (_currentState != state)
            {
                _currentState.Exit();
                _currentState = state;
                _currentState.Enter();
            }
        }

        private void AddStates()
        {
            _states = new Dictionary<string, State>();

            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out State state) == false)
                    continue;

                if (_states.ContainsKey(child.name))
                {
                    Debug.LogError($"StateMachine {name}: duplicate state name found: {child.name}. Ignoring...");
                    continue;
                }

                _states.Add(child.name, state);
            }
        }
    }
}
