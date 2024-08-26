public interface IDamagable
{
    void TakeDamage(float amount);
    void TakeDamageOverTime(float amount, float tickMultiplier);
}
