---
title: F# Active Patterns
date: 2016-01-23 19:27:15
tags: fs
---

F# มี syntax พิเศษที่ช่วยให้เขียน Pattern matching ได้ง่ายขึ้น โดยใช้โครงสร้างคล้าย ๆ กับ [Union](https://msdn.microsoft.com/en-us/library/dd233226.aspx) ใช้วิธีประกาศ Test case ที่ต้องการในเครื่องหมาย `(||)` Active pattern มีหลายแบบ ขึ้นอยู่กับจำนวน Case ที่ใช้

## Single-Case Active Patterns

เป็น Active pattern ที่มี Case เดียวและมี Input เพียงตัวเดียว Active pattern แบบนี้ต้อง Return ผลลัพทธ์เสมอ

```fsharp
let (|Remainder2|) x = x % 2
let checkNumber = function
    | Remainder2 1 -> "even number"
    | Remainder2 0 -> "odd number"

// "even number"
checkNumber 1
```

จากตัวอย่าง `Remainder2` เป็น Pattern ที่มี Case โดยคำนวณค่า Mod จาก Input ที่ส่งเข้ามา

## Partial-Case Active Pattern

เป็น Active patten ที่ Return ผลลัพท์เป็น Option และสามารถใช้ Pattern หลายตัวมาประกอบกัน เมื่อต้องการ Matching


```fsharp
let (|LessThen10|_|) x = if x < 10 then Some x else None
let (|Btw10And20|_|) x = if x >= 10 && x < 20 then Some x else None

let checkNumber x =
    match x with
    | LessThen10 a -> "less then 10"
    | Btw10And20 a -> "between 10 and 20"
    | _ -> "that's a big number"

// "that's big number"
checkNumber 20
```

`LessThen10` และ `Btw10And20` เป็น Partial pattern ที่ทดสอบว่าตัวเลขอยู่ใน Range ของตัวเองหรือไม่ สังเกตว่า Output ที่ได้จะเป็น Option ซึ่งในกรณีที่ Return ค่าเป็น None แสดงว่า Input ที่รับเข้ามาอยู่นอก Range ในกรณีที่ Input ที่รับมาไม่อยู่ใน Range ใดเลย เมื่อทำการ Matching ก็จะถูก Evaluate เข้า Case default หรือ `_` นั่นเอง

## Multicase Active Pattern

เป็น Pattern matching ที่มีหลาย Case โดย Pattern แบบนี้ต้อง Return choice ตามจำนวนตาม Case ที่ประกาศไว้ใน `(||)`

```fsharp
let (|Q1|Q2|Q3|Q4|) (date:System.DateTime) =
    let month = date.Month
    match month with
    |1|2|3 -> Q1 month
    |4|5|6 -> Q2 month
    |7|8|9 -> Q3 month
    |_ -> Q4 month

let newYearResolution date =
    match date with
    | Q1 _ -> "read"
    | Q2 _ -> "write"
    | Q3 _ -> "execute"
    | Q4 _ -> "sleep"

// "sleep"
newYearResolution <| DateTime(2015,10,10)
```

`(|Q1|Q2|Q3|Q4|)` เป็น Pattern ที่ใช้หา Quarter ของ DateTime ที่รับเข้ามา สังเกตว่าฟังก์ชัน Match จะต้อง Return Q1 - Q4 ให้ครบ

## Parameterized Active Pattern

เป็น Pattern ที่สามารถเพิ่ม Parameter ตัวที่สอง ซึ่งแตกต่างจาก Pattern แบบอื่นที่สามารถมี Parameter ตัวแรกเพียงตัวเดียว

```fsharp
let(|Divisible|_|) x y =
    if y % x = 0 then Some Divisible
    else None

let f2 = function
    | Divisible 2 & Divisible 3 -> "divisible by 6"
    | _-> "other"

// "divisible by 6"
f2 12
```

`Divisible` เป็น Pattern ที่รับ Parameter สองตัว คือ `x` และ `y` จะเห็นว่าขั้นตอนการเรียกใช้ คือ ในฟังก์ชั่น `f2` มีการส่ง Parameter ตัวแรก คือ `2` และ `3` จากนั้นตอนเรียกใช้ฟังก์ชั่น `f2` มีการส่ง Parameter ตัวที่สองคือ `12`