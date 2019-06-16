using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


//站点
public class AirPort
{
   public Transform obj;
   public Vector3 position
   {
      get { return obj.position; }
   }

   //自己作为终点，所有的起点
   public List<AirPort> inAirports = new List<AirPort>();
   //自己作为起点，所有的终点
   public List<AirPort> outAirports = new List<AirPort>();

   //linerender 集合，每个负责一个条线。数量和 outAirports 相同，因为只画 从自己出发的线。
   private Dictionary<int, LineRenderer> _lines = new Dictionary<int, LineRenderer>();
   
   
   public AirPort(Transform trans)
   {
      obj = trans;
   }

   public int LinkedCount()
   {
      return inAirports.Count + outAirports.Count;
   }

   public void DrawLines()
   {
      for (int i = 0; i < outAirports.Count; i++)
      {
         DrawOneLine(this, outAirports[i]);
      }
   }
   private void DrawOneLine(AirPort start, AirPort end)
   {
      LineRenderer lineRender;
      if (_lines.ContainsKey(end.GetHashCode()))
      {
         lineRender = _lines[end.GetHashCode()];
      }
      else
      {
         lineRender = new GameObject("linerender").AddComponent<LineRenderer>();
         lineRender.transform.parent = obj;
         _lines[end.GetHashCode()] = lineRender;

         lineRender.startWidth = 0.05f;
         lineRender.endWidth = 0.05f;
         lineRender.material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
         lineRender.receiveShadows = false;
         lineRender.shadowCastingMode = ShadowCastingMode.Off;
         lineRender.startColor = Color.yellow;
         lineRender.endColor = Color.yellow;

      }

      float angle = Vector3.Angle(start.position, end.position);
      float k = 1f; //角度太大曲线会穿过地球。
      if (angle <= 60) k = 1.1f;
      else if (angle > 60 && angle < 120) k = 1.3f;
      else if (angle >= 120 && angle < 160) k = 1.5f;
      else if (angle >= 160) k = 1.8f;

      
      //确定bezier中间的两个点， 曲线不能太直会穿过地面，也不能太弯会不美观，所以用 k 调整一下。
      Vector3 p1 = Quaternion.AngleAxis(angle * 0.3f, Vector3.Cross(start.position, end.position)) * start.position * k;
      Vector3 p2 = Quaternion.AngleAxis(-angle * 0.3f, Vector3.Cross(start.position, end.position)) * end.position * k;

      lineRender.positionCount = 100;

      for(int i =1; i <= 100; i++)
      {
         //参数的取值范围 0 - 1 返回曲线没一点的位置
         //为了精确这里使用i * 0.01 得到当前点的坐标
         Vector3 vec = Bezier.Bezier_3(start.position, p1, p2, end.position, (float)(i *0.01) );
         //把每条线段绘制出来 完成白塞尔曲线的绘制
         lineRender.SetPosition(i -1,vec);
      }

   }

   public void Destroy()
   {
      if(obj != null) GameObject.Destroy(obj);
   }

}

//飞行线路
public class Route
{
   //起点
   public AirPort start;
   
   //终点
   public AirPort end;

   public Route(AirPort s, AirPort e)
   {
      start = s;
      end = e;
      start.outAirports.Add(end);
      end.inAirports.Add(start);
   }

   public void Destroy()
   {
      if (start != null) start.Destroy();
      if(end != null) end.Destroy();
   }

}

