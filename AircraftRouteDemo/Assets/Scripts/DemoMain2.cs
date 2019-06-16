using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = System.Random;


public class DemoMain2 : MonoBehaviour
{

    public const float EARTH_RADIUS = 20.2f;
    public const int ROUTE_COUNT = 150;// 总共要生成的线路数，因为非真实航班，两个站点间只能有一条航线 
    public const int MAX_ROUTE_PER_AIRPORT = 5;//每个站点容纳的最大的线路数
    
    public Transform airportsRoot;
    public int airportsCount = 200;
    
    private List<Route> _routeMap = new List<Route>();
    private List<AirPort> _airPorts = new List<AirPort>();
    private List<AirPort> _fullyAirPorts = new List<AirPort>(); //达到 MAX_ROUTE_PER_AIRPORT 的站点
    
    private void Start()
    {
        InitAirportsPos();
        GenRouteMap();

    }

    private void Update()
    {
        DrawAllLine();
    }

    private void InitAirportsPos()
    {
        ClearAirPorts();
        //随机出机场点坐标
        Vector3 randPos = new Vector3(EARTH_RADIUS, 0, 0);
        
        while (_airPorts.Count < airportsCount)
        {
            randPos = Quaternion.Euler(UnityEngine.Random.Range(30, 330), UnityEngine.Random.Range(30, 330),
                          UnityEngine.Random.Range(30, 330)) * randPos;
            if (IsPointTooNear(randPos))
            {
                continue;
            }
            Transform newObj = GameObject.Instantiate(airportsRoot.GetChild(0), airportsRoot);
            newObj.transform.position = randPos;
            AirPort a = new AirPort(newObj);
            _airPorts.Add(a);
            
        }
    }

    //避免随机生成的点太近。。
    private bool IsPointTooNear(Vector3 newPos)
    {
        for (int j = 0; j < _airPorts.Count; j++)
        {
            if (Vector3.Distance(newPos, _airPorts[j].position) < 2f)
            {
                return true;
            }
        }

        return false;
    }

    private void DrawAllLine()
    {
        for (int i = 0; i < _routeMap.Count; i++)
        {
            _routeMap[i].start.DrawLines();
        }
      
    }

    // 为了不重复画线，两个站点之间只有一个 单向 路线
    private bool HasSameRoute(AirPort a, AirPort b)
    {
        return a.inAirports.Contains(b) || a.outAirports.Contains(b);
    }

    private void ClearRouteMap()
    {
        for (int i = 0; i < _routeMap.Count; i++)
        {
            _routeMap[i].Destroy();
        }
        _routeMap.Clear();

    }

    private void ClearAirPorts()
    {
        for (int i = 0; i < _airPorts.Count; i++)
        {
            _airPorts[i].Destroy();
        }
        _airPorts.Clear();

        for (int i = 0; i < _fullyAirPorts.Count; i++)
        {
            _fullyAirPorts[i].Destroy();
        }
        _fullyAirPorts.Clear();
    }

    private void CheckAirportFully(AirPort ap)
    {
        if (ap.LinkedCount() == MAX_ROUTE_PER_AIRPORT)
        {
            _airPorts.Remove(ap);
            _fullyAirPorts.Add(ap);
        }
    }

    private void GenRouteMap()
    {
        ClearRouteMap();
        while (_routeMap.Count < ROUTE_COUNT)
        {
            AirPort start = _airPorts[UnityEngine.Random.Range(0, _airPorts.Count)];
            AirPort end = _airPorts[UnityEngine.Random.Range(0, _airPorts.Count)];
            if (start == end || HasSameRoute(start, end) )
            {
                continue;
            }
            
            Route r = new Route(start, end);
            _routeMap.Add(r);
            
            CheckAirportFully(start);
            CheckAirportFully(end);

        }

    }


}
