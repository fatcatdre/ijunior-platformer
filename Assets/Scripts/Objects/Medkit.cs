using UnityEngine;

public class Medkit : Collectable
{
    [SerializeField] private int _healAmount = 1;

    public int HealAmount => _healAmount;
}
