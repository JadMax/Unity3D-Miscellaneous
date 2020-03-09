using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromVirtualStick_PlayerModule : MonoBehaviour
{
    private VirtualJoystickModule joystickModule;

    public int playerStickId = 0;

    public Vector2 offset = Vector2.zero;

    private void Start()
    {
        joystickModule = GetComponent<VirtualJoystickModule>();
    }

    private void Update()
    {
        offset = joystickModule.offset[playerStickId];
    }
}
