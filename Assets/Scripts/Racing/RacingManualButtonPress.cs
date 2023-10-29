using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingManualButtonPress : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit)) return;

        if (!hit.collider.TryGetComponent<MenuButton>(out var button)) return;

        button.ManualPress();

    }
}
