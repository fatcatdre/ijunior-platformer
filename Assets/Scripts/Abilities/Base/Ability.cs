using UnityEngine;
using System.Collections;

public abstract class Ability : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private KeyCode _key = KeyCode.Q;
    [SerializeField] private float _duration = 6f;
    [SerializeField] private float _cooldown = 7f;
    [SerializeField] private float _updateTickDelay = 0.5f;

    private float _cooldownRemaining;

    private Coroutine _abilityRoutine;

    protected virtual void Update()
    {
        if (_cooldownRemaining > 0f)
        {
            _cooldownRemaining -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(_key))
        {
            if (_abilityRoutine != null)
            {
                StopCoroutine(_abilityRoutine);
                OnAbilityFinish();
            }

            _abilityRoutine = StartCoroutine(ActivateAbility());
        }
    }

    private void OnValidate()
    {
        _duration = Mathf.Max(_duration, 0f);
        _cooldown = Mathf.Max(_cooldown, 0f);
    }

    private IEnumerator ActivateAbility()
    {
        _cooldownRemaining = _cooldown;

        OnAbilityStart();

        float currentDuration = 0f;
        float currentTickTime = _updateTickDelay;

        while (currentDuration <= _duration)
        {
            OnAbilityUpdate();

            if (currentTickTime >= _updateTickDelay)
            {
                OnAbilityTick();
                currentTickTime = 0f;
            }
            else
            {
                currentTickTime += Time.deltaTime;
            }

            currentDuration += Time.deltaTime;

            yield return null;
        }

        _abilityRoutine = null;

        OnAbilityFinish();
    }

    protected abstract void OnAbilityStart();
    protected abstract void OnAbilityTick();
    protected abstract void OnAbilityUpdate();
    protected abstract void OnAbilityFinish();
}
