---
title: สร้าง Instance ด้วย Lambda Expression
date: 2016-02-13 16:30:31
tags: cs
---

## Note

สามารถสร้าง instance ด้วย lambda expression โดยไม่ต้องสนใจ accessibility ของคลาส

## ตัวอย่าง

ตัวอย่างนี้เป็นการใช้ expression ที่ประกาศไว้ใน namespace `System.Linq.Expression`

```csharp
using System.Linq.Expressions
class C {
    private C(){ Console.WriteLine("init.."); }
}
Expression<Func<C>> e =
    Expression.Lambda<Func<C>>(Expression.New(typeof(C)),
        Enumerable.Empty<ParameterExpression>());
Func<C> f = e.Compile();
```

ในโค้ดมีการใช้ `Expression.New` เพื่อสร้าง instance ของคลาส `C` คำสั่งนี้จะไม่มีการเช็ค accessibility ของคลาสโดยสังเกตจากการเรียกฟังก์ชั่น `f` ที่ได้จากการคอมไพล์ expression จะไม่เกิด error เกี่ยวกับ protection level ทั้งที่ constructor ของคลาส `C` ประกาศเป็น `private`

```csharp
var c1 = f();
```

ผลลัพธ์

```bash
init..
```

จะต่างจากการสร้าง instance โดยใช้ `new` ซึ่ง compiler จะเช็ด accessibility ของคลาสเสมอ

```csharp
var c2 = new C();
```

ผลลัพท์ คือ โค้ดไม่สามารถ compile โดยจะแสดง message ดังนี้

```bash
(1,11): error CS0122: `C.C()' is inaccessible due to its protection level
(2,13): (Location of the symbol related to previous error)
```