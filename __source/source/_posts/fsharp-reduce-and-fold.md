---
title: ฟังก์ชัน Fold กับ Reduce
date: 2016-02-07 16:30:31
tags: fs
---

 ใน F# มีฟังก์ชั่นที่ใช้สำหรับ aggregate ข้อมูลคล้ายกับ `Aggregate` ของ LINQ คือ `fold` กับ `reduce`

## Aggregate ใน C#.

```csharp
var data = new int[] { 1,2,3};
var sum = data.Aggregate(0, (acc,x) => acc + x); // sum = 6
var pro = data.Aggregate(1, (acc,x) => acc * x); // pro = 6
```

## Fold

```fsharp
open System.Linq
let data = [|1;2;3|]
let sum = Array.fold (fun acc x -> acc + x) 0 data // sum = 6
let pro = Array.fold (fun acc x -> acc * x) 1 data // pro = 6
```

ฟังก์ชั่น `fold` รับ parameter ทั้งหมด 3 ตัว คือ

- *folder*  คือ ฟังก์ชันสำหรับ transform หรือ update state
- *state* คือ initial value หรือค่าเริ่มต้นที่จะถูกใช้ในฟังก์ชัน *folder*
- *input* คือ ข้อมูลที่ต้องการ process

## Reduce

```fsharp
let data = [1;2;3]
let empty = []

let sum1 = List.reduce(fun acc x -> acc + x) data   // sum1 = 6
let sum2 = List.reduce(fun acc x -> acc + x) empty  // ERROR: The input list was empty.

let sum3 = List.fold(fun acc x -> acc + x) 0 data   // sum3 = 6
let sum4 = List.fold(fun acc x -> acc + x) 0 empty  // sum4 = 0
```

ฟังก์ชั่น `reduce` จะคล้ายกับ `fold` ต่างกันที่เราไม่สามารถใส่ state หรือ initial value แบบ explicit ได้ `fold` จึงต้องดึง element ตัวแรกของ input เป็น initial value เสมอ ทำให้เกิดข้อจำกัด คือ ข้อมูล input จำเป็นต้องมีอย่างน้อย 1 element มิฉะนั้นจะเกิด error `System.ArgumentException: The input list was empty`

ความแตกต่างอีกข้อคือ initial value ที่ใสในฟังก์ชั่น `fold` สามารถเป็น type ใดก็ได้ ส่งผลให้ลัพท์สุดท้ายไม่จำเป็นต้องเป็น type เดียวกับ input element จะแตกต่างจาก `reduce` ที่ output ต้องเป็น type เดียวกับ input element เสมอ

```fsharp
[1 .. 3] |> List.fold (fun str n -> str + "," + (string n)) "" // ,1,2,3
```

Links

- http://stackoverflow.com/questions/9055837/difference-between-fold-and-reduce