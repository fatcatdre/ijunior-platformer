using UnityEngine;

public class ProjectileDespawner : MonoBehaviour, IProjectileInteractable
{
    public void Interact(Projectile projectile)
    {
        projectile.Despawn();
    }
}
