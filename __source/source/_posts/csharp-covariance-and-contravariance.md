---
title: Covariance กับ Contravariance (Draft)
date: 2016-01-14 15:31:49
tags: cs
---

 ใน C# concept ที่เรียกว่า covariance และ contravariance ถูกใช้กับอะไรบ้าง

* ใช้กับ array ในเวอร์ชัน 1.0
* ใช้กับ delegate ในเวอร์ชั่น 2.0
* ใช้กับ generic ในเวอร์ชั่น 4.0

## Substitution

ก่อนจะรู้จักกับ covariance ต้องทำความเข้าใจกับหลักการ substitution ใน C# เสียก่อน

```csharp
public class Animal { .. }
public class Cat: Animal { .. }
```

จาก class hierarchy จะเห็นว่า Animal เป็น supertype ของ Cat

```csharp
Animal a = new Cat();    // ok
Cat b = new Animal();    // error
```

จากตัวอย่าง `Animal` เป็นเซ็ตที่ใหญ่กว่าตามหลัก *substitution principle* เราสามารถใช้ instance ของ `Cat` แทน instance ของ `Animal` ได้ แต่ไม่สามารถใช้ instance ของ `Animal` แทน `Cat` ให้นึกภาพว่า เราไม่สามารถนำกล่องที่ใหญ่มาใส่ในกล่องที่เล็กกว่านั่นเอง

## Array

Array ที่มี element เป็น reference type ใน C# ถือว่าเป็น covariance

``` csharp
Animal[] animals = new Cat[10];
```

จากตัวอย่าง `Animal[]` compatible กับ `Cat[]` สามารถใช้ instance ของ `Cat` แทน `Animal` เราเรียกการ assign ค่าที่ type เล็กกว่าอยู่ด้านขวาของเครื่องหมาย `=` ว่า covariance

แต่ covariance ของ array เป็น covariance แบบไม่ safe เนื่องจากเราสามารถใส่ instance ของคลาสอื่นนอกเหนือจาก `Cat` แต่อยู่ hierarch  ถัดจาก `Animal` ซึ่งจะทำให้เกิด `ArrayTypeMismatchException` ขณะรัน


``` csharp
// Attempted to access an element as a type incompatible with the array.
animals[0] = new Dog()
```

จากตัวอย่างจะเกิด error ขณะรัน เนื่องจาก backing store ของ array ถูกกำหนดให้เก็บ `Cat ` ตั้งแต่ต้น จึงไม่สามารถเก็บ element ที่มี type อื่นได้ ถึงแม้จะเป็น subtype ของ `Animal` เหมือนกันก็ตาม

Covariance ใน array ถูก implement ตั้งแต่ C# 1.0 เพื่อให้เหมือนกับ Java ในขณะนั้น ถึงปัจจุบันปัญหา `TypeMissMatch` ก็ยังอยู่ ไม่สามารถตรวจสอบขณะ compile ได้

ใน C# รุ่นต่อมา มีการ implement covariance กับส่วนอื่น ซึ่งจะไม่มีปัญหาลักษณะนี้ เนื่องจากมีการออกแบบให้มีความปลอดภัยตั้งแต่ต้น

## Method group

#### Covariance

``` csharp
Cat MakeCat() { ... }
Func<Animal> makeAnimal = MakeCat
```

Covariant จะทำให้ `Func<Animal>` compatible กับ `MakeCat` เนื่องจาก `Animal` ใหญ่กว่า `Cat` จำสามารถทำ implicit conversion


#### Contravaraince

ตัวอย่างนี้มีสอง method คือ `ShowCat` ที่รับ `Cat` เป็น parameter และ `Animal` ที่รับ `Animal` เป็น parameter

``` csharp
void ShowCat(Cat c) {}
void ShowAnimal(Animal c) {}
```

จากตัวอย่างเราสามารถ assign method ไปยัง delegate `Action` ได้ดังนี้

``` csharp
Action<Cat> action2 = ShowAnimal;   // legal
Action<Animal> action1 = ShowCat;   // illegal
```

จากหลัก *substution principle* `Cat` นั้นเล็กว่า `Animal`

- บรรทัดแรก `Action<Cat>` compatible กับ `ShowAnimal` เนื่องจาก เราสามารถใช้ `Cat` แทน `Animal` ได้ ในตัวอย่างนี้ถึงแม้จะใช้หลัก `substitution` เหมือนเดิม แต่ทิศทางการ assign ค่าด้วยเครื่องหมาย `=` จะกลับข้างกัน เราเรียก operation ลักษณะนี้ว่า `contravariance`  
- บรรทัดที่สอง `Action<Animal>` ไม่สามารถใช้แทน `ShowCat` ได้ เนื่องจากจะผิดหลัก `substitution` เพราะว่า `Animal` ใหญ่กว่า `Cat` จึงไม่สามารถใช้ `Animal` แทน `Cat` นั่นเอง

## Generic delegate

ใน C# 4.0 มีสิ่งที่เรียกว่า safe generic variant ที่ใช้ได้กับ interface และ delegate

``` csharp
public delegate TResult Func<in T, out TResult>(T arg);
```

จากตัวอย่าง เราสามารถประกาศ delegate ชื่อ `Func` ที่มี type parameter ตัวแรกเป็น contravariance `T` และมี type parameter ตัวที่สองเป็น covariance `TResult`

- ใช้ keyword `in` เพื่อระบุว่า type parameter เป็น contravariance
- ใช้ keyword `out` เพื่อระบุว่า type parameter เป็น variance

เรียกใช้งาน

```csharp
Func<Animal, Cat> f1 = (x) => new Cat();
Func<Cat,Animal> f2 = f1;
```

`Func<Animl,Cat>` compatible กับ `Func<Cat, Animal>` เนื่องจากจาก type parameter ตัวแรกเป็น contravariance `Cat` ซึ่งเป็น type ที่เล็กกว่าจึงสามารถส่งเข้าไปในฟังก์ชั่นแทน `Animal` ส่วน type parameter ตัวที่สองเป็น covariance เราจึงใช้ `Animal` เพื่อรับค่าแทน `Cat` ที่ return มาจากฟังกํชั่นได้เช่นเดียวกัน

Links

- [The theory behind covariance and contravariance in C# 4](http://tomasp.net/blog/variance-explained.aspx)
- [Covariance and Contravariance in C#](https://blogs.msdn.microsoft.com/ericlippert/2007/10/16/covariance-and-contravariance-in-c-part-one/)