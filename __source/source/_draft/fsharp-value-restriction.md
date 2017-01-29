---
title: F# Value Restriction
date: 2016-01-26 00:56:04
tags: fs
---

Parametric polymorphism เราสามารถ define type หรือ function ที่สามารถกัดการกับ argument โดยไม่จำกัด type ของ argument ที่ส่งเข้ามา เช่น

```fsharp
let len<'T> (l: 'T list) = List.length l    // 'T list -> int
let len2 l = List.length l                  // 'T list -> int

let l1 = len [1;2;3]
let l2 = len ["1";"2";"3"]

let l3 = len2 [1;2;3]
let l4 = len2 ["1";"2";"3"]
```

`len` และ `len2` เป็น polymorphism function ที่จัดการกับ list ของ type ไหนก็ได้

ใน F# สามารถประกาศ polymorphism value ด้วย list เช่น

```fsharp
let nil = []        // 'a list
let a = 1 :: nil    // int list
let b = "1" :: nil  // string list
```

option

```fsharp
let none = None
let intRes = defaultArg none 1      // int
let stringRes = defaultArg none "1" // string
```

แต่ใช้ได้กับ mutable value เท่านั้น

```
let v = ref None
```

มันจะทำให้เกิด Error ตอนรันได้

```
do v := Some 1
let r = defaultArg !v "A"
```

## Value Restriction

ต้องแบ่ง let binding ออกเป็นสองกลุ่ม

กลุ่มที่สามารถทำ polymorphism

กลุ่มที่ต้องผูกกับ type

หลักการแบ่งกลุ่ม

Apply polymorphism function กับ polymorphism value

```fsharp
let v = id []
let map_id = Seq.map id
```

สร้าง object ที่มี constructor infer ด้วย type argument

```fsharp
type A<'T> = class end
let v1 = A<_>()
```

สร้าง object โดยไม่ระบุ type argument

```fsharp
type A<'T>() = class end
let create<'T> = A<'T>()
let a = create
```

sss

```fsharp
let (|Fail|) s v = failwith s
let getAge (Some age | Fail "Age is missing!" age) = age

let a = getAge(Some 10)
let b = getAge(None)
```

// http://v2matveev.blogspot.com/2010/04/nature-of-value-restriction.html
// http://blogs.msdn.com/b/mulambda/archive/2010/05/01/value-restriction-in-f.aspx