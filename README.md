# Snake
## 2018年7月24日 v0.1
### 游戏的界面一共四个：开始界面，规则界面，皮肤商店界面，游戏界面</br>
## 1 游戏的主界面
![image](https://github.com/li-zheng-hao/Snake/raw/master/DisplayGIF/游戏功能完整.gif)</br>
## 2 商店的皮肤
![image](https://github.com/li-zheng-hao/Snake/raw/master/DisplayGIF/皮肤功能.gif)</br>
## 3 游戏规则的介绍
![image](https://github.com/li-zheng-hao/Snake/raw/master/DisplayGIF/规则功能.gif)</br>

# 游戏实现的一些主要的功能包括：
## 1 游戏界面UI的设计以及适配
###### 项目采用的是UGUI 屏幕适配通过将Canvas的Scaler中的Scale Mode设置为Scale With Screen Size，在设计时采用的分辨率为1334×750（因此参考分辨率也是这个）,优先参考宽度；同时设置UI元素的锚点到相应的位置，使得在分辨率发生变化的情况下，UI元素在屏幕中的相对位置不会发生变化。（四个锚点集中在一点，不想对父容器进行拉伸）
