using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRaycastDebugger : MonoBehaviour
{
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    void Start()
    {
        raycaster = GetComponentInParent<GraphicRaycaster>(); // �θ𿡼� GraphicRaycaster ã��
        eventSystem = EventSystem.current; // ���� Ȱ��ȭ�� EventSystem ��������

        if (raycaster == null)
            Logging.LogWarning("GraphicRaycaster not found in parent!");

        if (eventSystem == null)
            Logging.LogWarning("EventSystem not found in scene!");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerEventData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);

            Logging.Log("---- Raycast UI Results ----");
            foreach (RaycastResult result in results)
            {
                Logging.Log($"Hit: {result.gameObject.name}");
            }
        }
    }
}
