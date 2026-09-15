using UnityEngine;

public interface IProjectileCollider
{
    public RaycastHit GetSingleCollision();

    public RaycastHit[] GetMultipleCollisions();

    public void GiveAttackData(SO_Attack data);
}
