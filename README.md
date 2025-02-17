# FixedNumeric
Unity3d的定点数运算库(Fixed Point)，设计上兼容BurstCompiler

提供S1Q31Q32的定点数的完整解决方案

- fp => float
- fpmath => Mathf
- fprandom => Random
- fpvector2 => Vector2
- fpvector3 => Vector3
- fpvector4 => Vector4
- fpquat => Quaternion
- fpmatrix4x4 => Matrix4x4
- fprect => Rect

采用多项式逼近的方式生成查找表进行计算

附带测试用例，注意用例可能无法覆盖所有的计算情况，有问题请上报issue

基准性能测试（fp vs float，单位ms，所有计算均为1000000迭代）

| 计算 | fp | float |
|-----|-----|-----|
| 加法 | 9 | 4 |
| 减法 | 9 | 4 |
| 乘法 | 18 | 7 |
| 除法 | 72 | 30 |
| 求余 | 34 | 37 |

耗时大概是float的2到3倍，仅供参考
