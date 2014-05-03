ComponentOrderAttribute
=======================

コンポーネントの「Move Up」「Move Down」をコードで自動管理

![](https://dl.dropboxusercontent.com/u/153254465/screenshot/%E3%82%B9%E3%82%AF%E3%83%AA%E3%83%BC%E3%83%B3%E3%82%B7%E3%83%A7%E3%83%83%E3%83%88%202014-05-04%203.10.50.png)



## ComponentOrderAttribute


### ComponentOrderAttribute(uint order)

指定した順番通りに並び替える。

**ただしTransformは必ず一番上でなければいけない。**

```
using UnityEngine;
using System.Collections;

[ComponentOrder(1)]
public class Order1 : MonoBehaviour
{
}
```

![](https://dl.dropboxusercontent.com/u/153254465/screenshot/%E3%82%B9%E3%82%AF%E3%83%AA%E3%83%BC%E3%83%B3%E3%82%B7%E3%83%A7%E3%83%83%E3%83%88%202014-05-04%203.15.42.png)

### ComponentOrderAttribute(Type type)

指定したTypeの直下に移動する

```
using UnityEngine;
using System.Collections;

[ComponentOrder(typeof(Rigidbody))]
public class OrderRigidbody : MonoBehaviour
{
}
```

![](https://dl.dropboxusercontent.com/u/153254465/screenshot/%E3%82%B9%E3%82%AF%E3%83%AA%E3%83%BC%E3%83%B3%E3%82%B7%E3%83%A7%E3%83%83%E3%83%88%202014-05-04%203.16.06.png)