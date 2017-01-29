---
title: Open กับ Closed Type
date: 2016-01-14 22:54:15
tags: cs
---

การใช้งาน generic ถ้าระบุ type argument ครบจำนวนที่ประกาศไว้ในเครื่องหมาย  `<>` เราจะเรียก type นั้นว่า closed type ถ้ามีการระบุไว้เพียงบางส่วน จะเรียกว่า structured type แต่ถ้าไม่ระบุ type argument เลยจะเรียกว่า open type

## ความแตกต่างระหว่าง open และ closed type

* Open type ถือว่าเป็น type ที่ไม่สมบูรณ์ จึงไม่สามารถสร้าง instance ได้ แต่สามารถใช้เป็น input ของ operator `typeof`
* Close type สามารถสร้าง instance ได้เหมือนคลาสทั่วไป

## ตัวอย่าง

สร้าง GenericStruct เป็น generic type ง่าย ๆ โดยมีการระบุเงื่อนไขว่า type argument ต้องเป็น value type เท่านั้น

```csharp
using System;
using System.Runtime.InteropServices;
using static System.Console;

public class GenericStruct<T> where T: struct {
    public string Name {
        get {
            return typeof(T).Name;
        }
    }
}
```

## ทดสอบ

ทดสอบว่าสามารถสร้าง instance ของ open และ closed type ผ่าน `Activator.CreateInstance` ได้หรือไม่

```csharp
public class Program {
    static object CreateInstance(Type t) {
        try {
            return Activator.CreateInstance(t);
        }catch(Exception e) {
            WriteLine(e.Message);
            return null;
        }
    }
    public static void Main(string[] args) {
        var open1 = CreateInstance(typeof(GenericStruct<>));
        var close1 = CreateInstance(typeof(GenericStruct<int>));

        WriteLine(open1 == null);                           // true
        WriteLine(close1 != null);                          // true
        WriteLine((close1 as GenericStruct<int>).Name);     // Int32
    }
}
```

จากตัวอย่างจะเห็นว่าเราไม่สามารถ สร้าง  instance ของ `GenericStruct<>` ได้ เนื่องจากไม่ระบุ type argument ไว้ในเครื่องหมาย `<>` ในบรรทัดที่ `14` จึงได้ `open1` มีค่า `null`

ส่วน closed type `GenericStuct<int>` สามารถสร้าง instance และแสดงค่า `Name` ได้ปกติ

