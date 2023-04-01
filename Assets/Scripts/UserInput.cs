using System;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public event Action<Vector3> onTouch;

    Vector3 pastFramePosition;
    Vector3 deltaPosition;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;

            deltaPosition = mousePosition - pastFramePosition;

            if (deltaPosition.magnitude < 15f)
            {
                return;
            }
           
            pastFramePosition = mousePosition;

            onTouch?.Invoke(deltaPosition);

        }
    }
}
