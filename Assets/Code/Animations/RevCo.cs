using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevCo : MonoBehaviour
{
    const float reversalSpeed = .1f;

    Action<float> action;

    float t = 0;
    float deltaT = 0;
    float deltaDeltaT;
    float tFinal;

    int direction = Direction.Forward;
    bool isRunning = false;

    float duration;

    Coroutine coroutine;
    Func<bool> stillRunning;

    public void Init(Action<float> action, float duration = 1)
    {
        this.action = action;
        this.duration = duration;
    }

    public void Action()
    {
        if (isRunning)
        {
            Reverse();
        }
        else
        {
            Go();
        }
    }

    public void Go() 
    {
        if (isRunning)
        {
            return;
        }

        isRunning = true;

        if (direction == Direction.Forward)
        {
            deltaDeltaT = reversalSpeed;
            tFinal = 1;
            stillRunning = () => t < tFinal;
        }
        else
        {
            deltaDeltaT = -reversalSpeed;
            tFinal = 0;
            stillRunning = () => t > tFinal;
        }

        coroutine = StartCoroutine(WrapperRoutine());
    }

    public void Stop()
    {
        if (!isRunning)
        {
            return;
        }

        StopCoroutine(coroutine);

        isRunning = false;
    }

    public void Reverse()
    {
        direction = -direction;

        if (direction == Direction.Forward)
        {
            deltaDeltaT = reversalSpeed;
            tFinal = 1;
            stillRunning = () => t < tFinal;
        }
        else
        {
            deltaDeltaT = -reversalSpeed;
            tFinal = 0;
            stillRunning = () => t > tFinal;
        }
    }

    public void Reset()
    {
        Stop();

        t = 1 - tFinal;
    }

    public void Complete()
    {
        Stop();

        t = tFinal;
    }

    IEnumerator WrapperRoutine()
    {
        while (stillRunning())
        {
            deltaT = Mathf.Clamp(deltaT + deltaDeltaT, -1, 1);

            t = t + deltaT * Time.deltaTime;

            action(t);

            yield return null;
        }

        t = tFinal;

        action(t);

        isRunning = false;

        deltaT = 0;

        Reverse();

        yield break;
    }
}