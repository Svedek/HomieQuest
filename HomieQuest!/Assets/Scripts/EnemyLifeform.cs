using UnityEngine;

public interface EnemyLifeform
{
    public void HitEnemy(float damage, Vector2 knockbackDirection, float knockbackForce);
}
