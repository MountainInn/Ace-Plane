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

            var newDeltaPosition = mousePosition - pastFramePosition;

            if (newDeltaPosition.magnitude < 10f)
            {
                return;
            }

            deltaPosition = newDeltaPosition;
           
            pastFramePosition = mousePosition;

            onTouch?.Invoke(deltaPosition);

        }
    }
}
