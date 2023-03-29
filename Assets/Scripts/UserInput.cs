using System;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public event Action<Vector2> onTouch;

    void Update()
    {
        onTouch?.Invoke(Input.GetTouch(0).deltaPosition);
    }
}
