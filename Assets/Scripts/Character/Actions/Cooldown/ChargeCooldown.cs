using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

public class ChargeCooldown<TMonoBehaviour> : ICooldown
where TMonoBehaviour : MonoBehaviour
{
    public readonly TMonoBehaviour target;

    int baseCharges;
    int charges;

    float interval;
    float lastIntervalTime;

    Coroutine? intervalCoroutine;

    public ChargeCooldown(TMonoBehaviour target, int baseCharges, float interval)
    {
        this.target = target;
        this.baseCharges = baseCharges;
        this.charges = baseCharges;
        this.interval = interval;
        
        // lastIntervalTime set at IntervalCoroutine start
        intervalCoroutine = target.StartCoroutine(IntervalCoroutine(interval));
    }


    public int GetCharges() => charges;
    public void AddToCharges(int value) => SetCharges(value + charges);
    
    public void RechargeAllCharges()
    {
        charges = baseCharges;
    }

    public void SetCharges(int value)
    {
        if (value < 0)
        {
            Debug.LogError($"Charges cannot receive negative value: {value}. Setting to 0");
            charges = 0;
        }
        else
        {
            charges = value;
        }
    }
    

    public int GetBaseCharges() => baseCharges;
    public void AddToBaseCharges(int value) => SetCharges(value + charges);
    public void SetBaseCharges(int value)
    {
        if (value <= 0)
        {
            Debug.LogError($"Charges cannot receive value <= 0: {value}. Setting to 1");
            baseCharges = 1;
        }
        else
        {
            charges = value;
        }
    }
    
    public float GetInterval() => interval;
    public void SetInterval(float value)
    {
        if (value <= 0)
        {
            Debug.LogError($"Interval time must be <= 0: {value}");
        }
        interval = value;
    }



    public bool IsReadyForUse() => charges > 0;



    // Returns minimum value when charges maxed
    public float GetRemaningIntervalTime()
    {
        return (lastIntervalTime + interval) - Time.time;
    }

    public float FreezeIntervalCoroutine()
    {
        target.StopCoroutine(intervalCoroutine);
        return GetRemaningIntervalTime();
    }
    
    public void ChangeIntervalProgress(float time)
    {
        if (time < 0 || time > interval) 
        {
            Debug.LogError(
                "Unexpected refresh interval must contain positive time lower than interval: "
                + $"interval: {interval}, receivedTime: {time}"
            );
        }

        if (intervalCoroutine != null)
        {   
            target.StopCoroutine(intervalCoroutine);
        }
    
        intervalCoroutine = target.StartCoroutine(IntervalCoroutine(time));
    }

    public void ResetInterval()
    {
        if (intervalCoroutine != null)
        {   
            target.StopCoroutine(intervalCoroutine);
        }
    
        intervalCoroutine = target.StartCoroutine(IntervalCoroutine(interval));
    }



    IEnumerator IntervalCoroutine(float time)
    {
        lastIntervalTime = Time.time;
        while (charges < baseCharges)
        {
            yield return new WaitForSeconds(time);

            // In case a full refresh is done on the middle of a interval
            charges = Mathf.Min(baseCharges, charges+1);
            lastIntervalTime = Time.time;
        }
        intervalCoroutine = null;
    }

    

    bool ICooldown.AttemptActivation() => AttemptConsumeCharge();
    public bool AttemptConsumeCharge()
    {
        if (charges <= 0) return false;
        
        Debug.Log(charges);
        charges--;
        intervalCoroutine ??= target.StartCoroutine(IntervalCoroutine(interval));
        return true;
    }
}

