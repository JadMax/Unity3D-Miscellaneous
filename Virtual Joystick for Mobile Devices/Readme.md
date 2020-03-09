# Virtual Joystick for Mobile Devices

## 思路

基本计算：

通过拖动手指按住的**UI控件**（stick），计算出其相对于设置好的**轴心**（pivot）的**向量偏移**（offset : Vector2）（此处可以标定offset会不会被「**标准化**」（normalized）。即如果标准化启用，就会无视拖动时离轴心远近造成的偏移量模长的改变；如果禁用，便如一般的控制玩家行走，拖得少，便走得慢、拖得多，便走得快），记录此偏移量，存储到数组（如果有多个摇杆）中，便于在其他代码中取用。

特殊处理：

> （此处所有的「距离」「坐标」全部指在 RectTransform 之下的参数）

1. 限制摇杆的**移动范围**（maxDragDistance）

   比如手游中控制角色行走的摇杆会被「固定」在一定的圆形范围内。通过检测 stick 和 pivot 之间的距离，当距离大于设定的距离（maxDragDistance）时，将其坐标设置为：
   
   ```c#
   stick.position = pivot.position + ((touch.position - pivot.position).normalized * maxDragDistance)
   ```
   
2. 多指触摸容易冲突：
  
   可将每次按下一个 stick 时的 fingerID 存储到 VJM 中该 stick 的 touchID 数组相应项中。比如：有两个摇杆，第一个在 VJM 中 ID 标记为 0（stick[0]），第二个标记为 1（stick[1]），如果 fingerID = 0 时按下 stick[0]，那么相应地，touchID[0] 便设置为 0 ；如果 fingerID = 3 时按下 stick[1]，那么相应地，touchID[1] 便设置为 3 ，此后便只读取 touches[0] 和 touches[3] 的信息，直至松开手并再次按下。
   

## 使用

1. 添加一个或多个 stick ，并逐一附加上 VJP 脚本；

2. 在空组件上（此处建议用 EventSystem 作为 Module（具有全局特征）类型脚本的载体）附加上 VJM 脚本；

3. 有几个 stick 便必须在 VJM 中设置相应的 pivot[]、stick[]、isNormalizedOffset[] 这三个需自定义的数组并使其一一对应。再在每个 stick 下添加 VJP，也要对应好在 VJM 中其对应的 ID、轴心以及自定义好移动范围；

4. 自己写需要用到它们的脚本（见例程「FromVirtualStick_PlayerModule.cs」和「PlayerPreference.cs」），取用 VJM 中对应 stick 的 offset 即可。

> 注：VJM = VirtualJoystickModule.cs；VJP = VirtualJoystick_StickPreference.cs

> 2020/3/9 夜
