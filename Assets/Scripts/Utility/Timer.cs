using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer
{
    public EventHandler onStart { get; set; }
    public EventHandler onEnd { get; set; }
    public EventHandler onUpdate { get; set; }

    float endTime;
    public float CurrentTime { get; private set; }
    public bool isActive { get; private set; }
    public bool isLooping { get; private set; }

    bool isCallingStart;
    bool isCallingEnd;
    bool isCallingUpdate;
    public Timer(float _endTime, bool _isLooping = true)
    {
        endTime = _endTime;
        isLooping = _isLooping;
    }
    //1 = Start, 2 = End, 3 = Update
    public void SetCalling(int index ,bool value)
    {
        switch (index)
        {
            case 1:
                isCallingStart = value;
                break;
            case 2:
                isCallingEnd = value;
                break;
            case 3:
                isCallingUpdate = value;
                break;
        }
    }

    public void Start()
    {
        CurrentTime = 0f;
        isActive = true;

        if (isCallingStart)
        {
            EventHandler handler = onStart;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Stop()
    {
        CurrentTime -= CurrentTime;
        isActive = false;

        if (isCallingEnd)
        {
            EventHandler handler = onEnd;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Reset()
    {
        Stop();
        Start();
    }

    public void Pause()
    {
        isActive = false;
    }

    public void Resume()
    {
        isActive = true;
    }
    public void SetTime(float value)
    {
        endTime = value;
    }

    public void Update()
    {
        CurrentTime += Time.deltaTime;

        if (isCallingUpdate)
        {
            EventHandler handler = onUpdate;
            handler?.Invoke(this, EventArgs.Empty);
        }

        if(CurrentTime >= endTime)
        {
            if (!isLooping)
            {
                Stop();
                return;
            }
            Reset();
        }
    }
}
