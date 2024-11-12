using System.Collections;
using System.Collections.Generic;
using StackableTimedEffect;
using UnityEngine;


/* Status effect based on duration 
 * Calls a given coroutine once at the start and refreshes it every time the
 * expected duration is changed. This avoids constant Update() calls, however it means
 * that refreshing a duration each frame is more costly 
 */
public abstract class TimedStatusEffect : StatusEffect
{
    // extending / decreasing duration will not reset activated time
    float duration;
    public float Duration{get {return duration;}}

    // when effect was last activated
    float activatedTime;
    public float LastSetTime{get {return activatedTime;}}

    Coroutine timerCoroutine; //always active

    public TimedStatusEffect(StatusEffectManager target, float duration)
    {
        this.target = target;
        StartNewEffectDuration(duration);
        // Expand contructor to add effects
    }

    public float GetRemainingTime() 
    {
        return this.duration - (Time.time - activatedTime);
    }
    
    public float GetRemaningPercentage()
    {
        return GetRemainingTime() / this.duration;
    }


    // For a next timed call on duration seconds 
    // (instead of changing duration on a existing timer)
    public void StartNewEffectDuration(float duration)
    {
        this.duration = duration;
        activatedTime = Time.time;
        target.StopCoroutine(timerCoroutine);
        timerCoroutine = target.StartCoroutine(DurationCoroutine(duration));
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
        UpdateEffect();
    }

    public void AddToDuration(float time)
    {
        this.duration += time;
        UpdateEffect();
    }

    // Refreshes duration if its higher than current duration
    public void RefreshDuration(float duration)
    {
        float remainingTime = this.duration - (Time.time - activatedTime);
        if (remainingTime < duration)
        {
            StartNewEffectDuration(duration);
        }
    } 

    protected void UpdateEffect()
    {
        target.StopCoroutine(timerCoroutine);

        float remainingTime = this.duration - (Time.time - activatedTime);
        if (remainingTime <= 0)
        {
            Remove();
        }
        else
        {
            timerCoroutine = target.StartCoroutine(DurationCoroutine(remainingTime));
        }
    }

    IEnumerator DurationCoroutine(float remainingDuration)
    {
        yield return new WaitForSeconds(remainingDuration);
        Remove();
    }


}



namespace TimedEffects
{

    public abstract class AddToSpeedMultiplier : TimedStatusEffect
    {
        public float multiplierBonus = 1f; //default value

        public AddToSpeedMultiplier(StatusEffectManager target, float duration, 
            int multiplierBonus) : base(target, duration) 
        {

            this.multiplierBonus = multiplierBonus;
            

            Start();
        }

        void Start()
        {
            target.entityMovement.AddToSpeedMultiplier(multiplierBonus);
        }

        public override void Remove()
        {
            base.Remove();
            target.entityMovement.AddToSpeedMultiplier(-multiplierBonus);
        }

        
    }

    public abstract class AddToSpeedBonus : TimedStatusEffect
    {
        public float speedBonus = 1f; //default value

        public AddToSpeedBonus(StatusEffectManager target, float duration, 
            float bonus) : base(target, duration) 
        {

            this.speedBonus = bonus;
            

            Start();
        }

        void Start()
        {
            target.entityMovement.AddToSpeedBonus(speedBonus);
        }

        public override void Remove()
        {
            base.Remove();
            target.entityMovement.AddToSpeedBonus(-speedBonus);
        }
    }


    public abstract class AddToDamageMultiplier : TimedStatusEffect
    {
        public float damageMultiplier = 1f; //default value

        public AddToDamageMultiplier(StatusEffectManager target, float duration, 
            float multiplierBonus) : base(target, duration) 
        {

            this.damageMultiplier = multiplierBonus;
            

            Start();
        }

        void Start()
        {
            target.characterDamage.AddDamageMultiplier(damageMultiplier);
        }

        public override void Remove()
        {
            base.Remove();
            target.characterDamage.AddDamageMultiplier(-damageMultiplier);
        }
    }


    public abstract class AddToDamageBonus : TimedStatusEffect
    {
        public int damageBonus = 1; //default value

        public AddToDamageBonus(StatusEffectManager target, float duration, 
            int bonus) : base(target, duration) 
        {

            this.damageBonus = bonus;
            

            Start();
        }

        void Start()
        {
            target.characterDamage.AddDamageBonus(damageBonus);
        }

        public override void Remove()
        {
            base.Remove();
            target.characterDamage.AddDamageBonus(-damageBonus);
        }
    }


    public abstract class TakeDamageOverInterval : TimedStatusEffect
    {
        int damage;
        float interval;
        Coroutine coroutine;

        public TakeDamageOverInterval(StatusEffectManager target, float duration, 
            int damage, float interval) : base(target, duration) 
        {
            this.damage = damage;
            this.interval = interval;

            coroutine = target.StartCoroutine(Timer());
        }

        IEnumerator Timer()
        {
            while (true)
            {
                yield return new WaitForSeconds(interval);
                target.damageable.TakeDamage(this, this.damage);
            }
        }

        public override void Remove()
        {
            base.Remove();
            target.StopCoroutine(coroutine);
        }
    }

}
