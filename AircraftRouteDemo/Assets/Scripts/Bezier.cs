
//代码原理参考 ref: https://www.cnblogs.com/msxh/p/6270468.html

using UnityEngine;

public static class Bezier
{
    /// <summary>
    /// 二次贝塞尔
    /// </summary>
    public static Vector3 Bezier_2(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return (1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2);
    }
    public static void Bezier_2ref(ref Vector3 outValue, Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        outValue = (1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2);
    }

    /// <summary>
    /// 三次贝塞尔
    /// </summary>
    public static Vector3 Bezier_3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return (1 - t) * ((1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2)) + t * ((1 - t) * ((1 - t) * p1 + t * p2) + t * ((1 - t) * p2 + t * p3));
    }
    public static void Bezier_3ref(ref Vector3 outValue , Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        outValue = (1 - t) * ((1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2)) + t * ((1 - t) * ((1 - t) * p1 + t * p2) + t * ((1 - t) * p2 + t * p3));
    }
    
    
//    /// <summary>
//    /// 根据T值，计算贝塞尔曲线上面相对应的点
//    /// </summary>
//    /// <param name="t"></param>T值
//    /// <param name="p0"></param>起始点
//    /// <param name="p1"></param>控制点
//    /// <param name="p2"></param>目标点
//    /// <returns></returns>根据T值计算出来的贝赛尔曲线点
//    private static  Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
//    {
//        float u = 1 - t;
//        float tt = t * t;
//        float uu = u * u;
// 
//        Vector3 p = uu * p0;
//        p += 2 * u * t * p1;
//        p += tt * p2;
// 
//        return p;
//    }
// 
//    /// <summary>
//    /// 获取存储贝塞尔曲线点的数组
//    /// </summary>
//    /// <param name="startPoint"></param>起始点
//    /// <param name="controlPoint"></param>控制点
//    /// <param name="endPoint"></param>目标点
//    /// <param name="segmentNum"></param>采样点的数量
//    /// <returns></returns>存储贝塞尔曲线点的数组
//    public static Vector3 [] GetBeizerList(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint,int segmentNum)
//    {
//        Vector3 [] path = new Vector3[segmentNum];
//        for (int i = 1; i <= segmentNum; i++)
//        {
//            float t = i / (float)segmentNum;
//            Vector3 pixel = CalculateCubicBezierPoint(t, startPoint,
//                controlPoint, endPoint);
//            path[i - 1] = pixel;
//        }
//        return path;
// 
//    }
}
