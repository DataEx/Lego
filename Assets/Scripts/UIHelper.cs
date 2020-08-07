using UnityEngine;
using UnityEngine.EventSystems;

public static class UIHelper
{
    public static bool IsMouseOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
