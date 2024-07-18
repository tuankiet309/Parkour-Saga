using DigitalRuby.RainMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainBehavior : MonoBehaviour
{
    private RainScript2D rainController =>GetComponent<RainScript2D>();
    [Range(0f, 1f)]
    [SerializeField] float intensity;
    [SerializeField] float targetIntensity;

    [SerializeField] float changeRate;
    [SerializeField] float minValue;
    [SerializeField] float maxValue;

    [SerializeField] private float chanceToRain = 40;
    [SerializeField] private float rainCheckCooldown;
    float rainCheckTimer;
    private bool canChangeIntensity;
    private void Update()
    {
        rainCheckTimer -=Time.deltaTime;
        
        rainController.RainIntensity = intensity;
        CheckForRain();
        if(canChangeIntensity )
            ChangeIntensity();
    }
    private void CheckForRain()
    {
        if(rainCheckTimer <0)
        {
            rainCheckTimer = rainCheckCooldown;
            if(Random.Range(0,100)<chanceToRain) 
                targetIntensity = Random.Range(minValue, maxValue );
            else 
                targetIntensity = 0;
            canChangeIntensity = true;
        }
    }
    private void ChangeIntensity()
    {
        if(intensity < targetIntensity)
        {
            intensity += changeRate * Time.deltaTime;
            if(intensity > targetIntensity )
            {
                intensity = targetIntensity;
                canChangeIntensity = false;
            }
        }
        if(intensity > targetIntensity)
        {
            intensity -=changeRate * Time.deltaTime;
            if(intensity<targetIntensity)
            {
                intensity = targetIntensity;
                canChangeIntensity = false;
            }
        }
    }
}
