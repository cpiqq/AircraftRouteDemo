# AircraftRouteDemo


unity 2018.4  
open Assets/Scenes/SampleScene

- 游戏运行一开始会随机生成30个站点， 起飞点叫 start 终点叫 end .  目前只在本次飞行的 start  和 end 之间画航线，飞完这段航线后以上一个end 为 起点， 随机飞往下一个 end.

- 最短路线算法用的大圆航线：https://baike.baidu.com/item/%E5%A4%A7%E5%9C%86%E8%88%AA%E7%BA%BF
- 地球半径 是 20 个 unit。
- 随机的站点的任何两点间的直线距离不会小于5.
- 航线用的linerender绘制，每隔5度一个路径点。
- 相机在飞机斜后方跟随着飞机。
- ui 用红字标识出 start 和 end 站点 .
