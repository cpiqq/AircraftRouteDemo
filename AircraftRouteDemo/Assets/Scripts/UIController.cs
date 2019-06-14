using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public DemoMain demoMain;

    public Transform startText;
    
    public Transform endText;

    public Vector2 offset; //偏移

    // Update is called once per frame
    void Update()
    {
        if (!demoMain.start || !demoMain.end) return;
        
        Vector2 screenPos = Camera.main.WorldToScreenPoint(demoMain.start.position);
        startText.position = screenPos + offset;
        
        screenPos = Camera.main.WorldToScreenPoint(demoMain.end.position);
        endText.position = screenPos + offset;
        
    }
}
