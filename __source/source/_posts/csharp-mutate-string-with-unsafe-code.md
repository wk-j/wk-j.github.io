---
title: อัปเดต String ด้วย Pointer
date: 2016-02-14 16:11:10
tags:
---

## Note

String เป็น immutable type ที่ไม่มี public api สำหรับแก้ไขข้อมูล เราจึงไม่สามารถเปลี่ยนค่าใน memory ที่เก็บ string ได้ ในกรณีที่มีการ assign ค่าใหม่ CLR จะสร้าง string ชุดใหม่เก็บใน memory ตำแหน่งใหม่

## Pointer

ใน C# ไม่มี type ไหนเป็น immutable ที่สมบูรณ์ เนื่องจากเราสามารถใช้ pointer เข้าถึงตำแหน่งใน memory ได้โดยตรง การใช้ pointer ใน C# ต้องทำใน unsafe block เท่านั้น

## ตัวอย่าง

```csharp
class Test {
    public static string T() {
        string s = "cat";
        unsafe {
            fixed (char* i = s) {
                *i = 'b';
                char* p2 = i + 1;
                *p2 = 'o';
            }
        }
        return s;
    }
}
Test.T(); // bot
```


## Links

- http://blog.paranoidcoding.com/2014/10/15/invalid-uses-of-a-type.html
- http://stackoverflow.com/questions/8176585/unsafe-code-to-change-length-by-mutation-of-a-string-object
- https://msdn.microsoft.com/en-us/library/ms228599.aspx