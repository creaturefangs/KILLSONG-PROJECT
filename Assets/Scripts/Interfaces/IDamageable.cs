public interface IDamageable
{
    void TakeDamage(float amount);
    void TakeDamageOverTime(float amount, float tickMultiplier);
}