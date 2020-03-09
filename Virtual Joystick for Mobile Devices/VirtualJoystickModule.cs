using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualJoystickModule : MonoBehaviour
{
    // 记得将「stick」设为「pivot」的子对象。
    // 可以有多个摇杆。
    // 务必保证一一对应！！！
    // 务必保证一一对应！！！
    // 务必保证一一对应！！！
    public RectTransform[] pivot;
    public RectTransform[] stick;

    public int[] touchId;

    private float[] maxDragDistance;

    public Vector2[] offset;
    public bool[] isNormalizedOffset;

    private void Start()
    {
        maxDragDistance = new float[pivot.Length];
        touchId = new int[pivot.Length];
        for (int i = 0; i < pivot.Length; i++)
        {
            stick[i].anchoredPosition = Vector2.zero;
            maxDragDistance[i] = stick[i].GetComponent<VirtualJoystick_StickPreference>().maxDragDistance;
        }
    }

    private void Update()
    {
        for (int i = 0; i < pivot.Length; i++)
        {
            if (isNormalizedOffset[i])
            {
                offset[i] = (stick[i].position - pivot[i].position).normalized;
            }
            else
            {
                offset[i] = (stick[i].position - pivot[i].position).normalized * 
                    (Vector2.Distance(pivot[i].position, stick[i].position) / maxDragDistance[i]);
            }
        }
    }
}
