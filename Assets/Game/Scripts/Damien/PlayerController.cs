using System;
using UnityEngine;
using EMILtools.Extensions;
using static UnityEngine.CursorLockMode;

public class PlayerController : MonoBehaviour
{
    public MouseLookEX.MouseLookSettings mouseLookSettings;

    private void Awake()
    {
        CursorEX.Set(false, Locked);
    }

    public void Update()
    {
        mouseLookSettings.UpdateMouseLook(Input.mousePositionDelta);
    }

}
