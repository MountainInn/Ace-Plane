using System.Collections.Generic;
using UnityEngine;

public abstract class RadioGroup<T> : MonoBehaviour
    where T : Component
{
    protected List<T> radios;

    protected void Awake()
    {
        InitRadios();
    }

    public void RadioToggle(T obj)
    {
        foreach (var item in radios)
        {
            if (item == obj)
                ToggleOn(item);
            else
                ToggleOff(item);
        }
    }

    protected abstract void InitRadios();

    protected abstract void ToggleOn(T obj);
    protected abstract void ToggleOff(T obj);
}
