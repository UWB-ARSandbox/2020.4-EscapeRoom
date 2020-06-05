using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool isOn = false;
    private float time = 0.0f;
    public static Timer Instance { get; private set; }
    private bool pass = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isOn)
            time += Time.deltaTime;
    }

    public void startTimer()
    {
        isOn = true;
    }

    public void stopTimer()
    {
        isOn = false;
    }

    public float getTime()
    {
        return time;
    }

    public int getSeconds()
    {
        return (int)(time % 60.0f);
    }

    public int getMinutes()
    {
        return (int)(time / 60);
    }

    public int getHours()
    {
        return (int)(time / 3600);
    }

    public void resetTimer()
    {
        time = 0.0f;
    }

    public void setPass(bool p)
    {
        pass = p;
        Debug.Log(true);
    }

    public bool didPass()
    {
        return pass;
    }
}
