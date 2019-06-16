using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour
{
    private Vector3 _oldPos = Vector3.zero;
    private Vector3 _curPos;
    private Vector3 _deltaPos;


    private bool _isPressing = false;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isPressing = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isPressing = false;
        }

        if (_isPressing)
        {
            _curPos = Input.mousePosition;
            if (_oldPos == Vector3.zero)
            {
                _oldPos = _curPos;
                return;
            }

            _deltaPos = _curPos - _oldPos;
            if (_deltaPos.sqrMagnitude > 1)
            {
                transform.Rotate(new Vector3(_deltaPos.y, -_deltaPos.x, 0), Space.World);
                _oldPos = _curPos;
            }
          
        }
        else
        {
            _oldPos = Vector3.zero;
        }



    }






}
