using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick_StickPreference : MonoBehaviour
{
    private GameObject events;
    private VirtualJoystickModule module;

    private RectTransform thisTrans;

    public int thisStickId = 0; // 请保证与VJM中对应！

    public bool isHolding = false;
    private Vector2 holdPos = Vector2.zero;

    public RectTransform pivot;
    public float maxDragDistance = 100.0f;

    private void Start()
    {
        events = GameObject.FindGameObjectWithTag("e");
        module = events.GetComponent<VirtualJoystickModule>();

        thisTrans = GetComponent<RectTransform>();
    }

    private int touchId;
    private void Update()
    {
        #region 判断是否按下（拖动）
        if (Application.platform == RuntimePlatform.Android)
        {
            foreach (Touch _touch in Input.touches)
            {
                if (_touch.phase == TouchPhase.Began && IsPointerOverThis(Input.GetTouch(_touch.fingerId).position))
                {
                    module.touchId[thisStickId] = _touch.fingerId;
                }
            }
            touchId = module.touchId[thisStickId];
            if (Input.GetTouch(touchId).phase == TouchPhase.Began && IsPointerOverThis(Input.GetTouch(touchId).position))
            {
                isHolding = true;
            }
            if (Input.GetTouch(touchId).phase == TouchPhase.Ended)
            {
                isHolding = false;
                thisTrans.position = module.pivot[thisStickId].position;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && IsPointerOverThis(Input.mousePosition))
            {
                isHolding = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isHolding = false;
                thisTrans.position = module.pivot[thisStickId].position;
            }
        }
        #endregion
        #region 开始拖
        if (isHolding)
        {
            if (isHolding)
            {
                if (Application.platform == RuntimePlatform.Android)
                    holdPos = Input.GetTouch(touchId).position;
                else
                    holdPos = Input.mousePosition;
            }
            thisTrans.position = holdPos;
        }
        #endregion
        #region 限制范围
        if (maxDragDistance > 0)
        {
            float spDis = Vector2.Distance(thisTrans.position, pivot.position);
            if (spDis > maxDragDistance)
            {
                thisTrans.position = pivot.position + ((thisTrans.position - pivot.position).normalized * maxDragDistance);
            }
        }
        #endregion
    }

    private bool IsPointerOverThis(Vector2 mousePos)
    {
        // 创建点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePos;
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        // 射线检测开始
        EventSystem.current.RaycastAll(eventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.name == gameObject.name)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
