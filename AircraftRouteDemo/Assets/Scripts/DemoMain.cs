using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = System.Random;


[RequireComponent(typeof(LineRenderer))]
public class DemoMain : MonoBehaviour
{

    public const int EARTH_RADIUS = 21;

    public Transform airports;
    public Transform aircraft;
    public float aircraftSpeed = 1f;
    public int airportsCount = 20;
    
    private LineRenderer _lineRenderer;
    public LineRenderer lineRender
    {
        get
        {
            if (!_lineRenderer)
            {
                _lineRenderer = GetComponent<LineRenderer>();
            }

            return _lineRenderer;
        }
    }

    private Transform _start;
    public Transform start
    {
        get { return _start; }
    }
    
    private Transform _end;
    public Transform end
    {
        get { return _end; }
    }

    private Vector3[] linePositions
    {
        get
        {
            Vector3[] ret = new Vector3[lineRender.positionCount];
            lineRender.GetPositions(ret);
            return ret;
        }
    }

    private int _flyProgress = 0;
    
    
    private void Start()
    {
        lineRender.positionCount = 0;
        InitAirportsPos();
        ResetRoute();
    }

    void Update()
    {
        //如果当前的行程没走完,还没飞到end
        if (_flyProgress < linePositions.Length)
        {
            //飞机朝向line上下一个点， 并且飞机下面朝向地心。
            Quaternion targetRotation = Quaternion.LookRotation(linePositions[_flyProgress] - aircraft.position, aircraft.position);
            aircraft.rotation = Quaternion.Slerp(aircraft.rotation, targetRotation, Time.deltaTime * 2);
            
            //飞机的移动，移动到下一个点
            Vector3 moveDir = linePositions[_flyProgress] - aircraft.position;
            aircraft.position += moveDir.normalized * Time.deltaTime * aircraftSpeed;
            
            //如果距离目标点够近，就开始飞向下一个点
            float des = Vector3.Distance(aircraft.position, linePositions[_flyProgress]);
            if (des < 0.1f)
            {
                _flyProgress++;
            }
        }
        else //已经飞到end, 以当前end 为起点，飞向下一个end.
        {
            ResetRoute();
        }
        
    }

    private void InitAirportsPos()
    {
        //随机出机场点坐标
        Vector3 randPos = new Vector3(EARTH_RADIUS, 0, 0);

        while (airports.childCount < airportsCount)
        {
            randPos = Quaternion.Euler(UnityEngine.Random.Range(30, 330), UnityEngine.Random.Range(30, 330),
                          UnityEngine.Random.Range(30, 330)) * randPos;
            if (IsPointTooNear(randPos))
            {
                continue;
            }
            Transform newObj = GameObject.Instantiate(airports.GetChild(0));
            newObj.parent = airports;
            newObj.transform.position = randPos;
        }
        
    }

    //避免随机生成的点太近。。
    private bool IsPointTooNear(Vector3 newPos)
    {
        for (int j = 0; j < airports.childCount; j++)
        {
            Transform child = airports.GetChild(j);
            if (Vector3.Distance(newPos, child.position) < 5f)
            {
                return true;
            }
        }

        return false;
    }

    ////已经飞到end, 以当前 end 为起点，飞向下一个end.
    private void ResetRoute()
    {
        if (airports.childCount != airportsCount) return;
        
        //随机设置本次飞行的起点、终点
        if (!_start)
        {
            _start = airports.GetChild(0);
        }
        else if(!_end)
        {
            _start = airports.GetChild(UnityEngine.Random.Range(0,airports.childCount ));
        }
        else
        {
            _start = _end;
        }

        while ( !_end || _end == _start || Vector3.Distance(_start.position, _end.position) < 2f )
        {
            _end = airports.GetChild(UnityEngine.Random.Range(0,airports.childCount ));
        }
        _flyProgress = 0;
  
        DrawRoute();
        
        aircraft.position = linePositions[_flyProgress];
        aircraft.LookAt(linePositions[_flyProgress + 1], aircraft.position);

    }

    //以当前 start 为起点  end 为终点， 画航线。
    private void DrawRoute()
    {
        // start 和 end 的向量夹角
        float angle = Vector3.Angle(_start.position, _end.position);

        //每隔一个小度数就增加一个 linerender 的路径点。
        int pointsCount = (int)(angle / 5f);
        //先设置点的总个数
        lineRender.positionCount = pointsCount + 1;

        // 与 起点、终点所在平面垂直的向量。相当于新建坐标系的z轴方向。
        Vector3 newZ = Vector3.Cross(_start.position, _end.position);
        // 相当于新建坐标系Y轴方向
        Vector3 newY = Vector3.Cross(newZ, _start.position);
        
        // 从世界坐标系变换到新建坐标系的 旋转变换。
        Quaternion q = Quaternion.LookRotation(newZ , newY);

        for (int i = 0; i < pointsCount + 1; i++) {
            //旋转之前的 圆弧 坐标值
            float x = Mathf.Cos( angle * i / pointsCount * Mathf.Deg2Rad) * EARTH_RADIUS ; 
            float y = Mathf.Sin(angle * i / pointsCount * Mathf.Deg2Rad) * EARTH_RADIUS ;
            
            // 把圆弧 从世界坐标系 旋转变换到 新坐标系。
            Vector3 pos = q * (new Vector3(x, y, 0));
            lineRender.SetPosition (i, pos);
            
        }
    }
    

}
