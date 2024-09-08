using System;

public class HealthSysytem
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnDeath;  

    private int health;
    private int healthMax;

    public HealthSysytem(int healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public int GetHealth()
    {
        return health;
    }

    public float GethealthPercent()
    {
        return (float)health / healthMax;
    }

    //public float GetEnemyhealthPercent()
    //{
    //    return (float)health / healthMax;
    //}

    public void Damage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        if (health <= 0)
        {
            OnDeath?.Invoke(this, EventArgs.Empty);  
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > healthMax) health = healthMax;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }
}
