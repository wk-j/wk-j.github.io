---
title: String กับ string
date: 2016-01-10 18:11:57
tags: cs
---

ใน C# จะมี alias ที่ match กับ type ใน [base class library (BCL)](https://en.wikipedia.org/wiki/List_of_data_types_of_the_Standard_Libraries#Base_Class_Library) เป็น keyword ตัวพิมพ์เล็ก เช่น System.String มี alias คือ string ทั้ง String ใหญ่และ string เล็กในทางเทคนิกแล้วสามารถใช้แทนกันได้

Alias ของ C# มีทั้งหมด 15 ตัว โดย map อยู่กับ BCL type ดังนี้

```csharp
object:  System.Object
string:  System.String
bool:    System.Boolean
byte:    System.Byte
sbyte:   System.SByte
short:   System.Int16
ushort:  System.UInt16
int:     System.Int32
uint:    System.UInt32
long:    System.Int64
ulong:   System.UInt64
float:   System.Single
double:  System.Double
decimal: System.Decimal
char:    System.Char
```

Alias ทุกตัว คือ keyword ของภาษา

``` csharp
abstract as base bool break byte case catch char checked class const
continue decimal default delegate do double else enum event explicit
extern false finally fixed float for foreach goto if implicit in
in (generic modifier) int interface internal is lock long namespace
new null object operator out out (generic modifier) override params
private protected public readonly ref return sbyte sealed short sizeof
stackalloc static string struct switch this throw true try typeof uint
ulong unchecked unsafe ushort using virtual void volatile while
```

เมื่อ alias กับ BCL type ทำงานเหมือนกัน แล้วจะเลือกใช้ตัวไหน? เรื่องนี้มีความเห็นหลายแบบ เช่น

ใน [C# coding style](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md) ของทีม corefx บอกว่าพวกเขาใช้ alias ทั้งหมดทุกกรณี เหตุผลอย่างหนึ่งคือ alias เป็น keyword ประโยชน์ที่ได้จากมันก็คือ สามารถเรียกใช้เมื่อไหร่ก็ได้ โดยไม่ต้องเปิด namespace ใด ๆ แม้ได้ `System`

บางคนให้ความเห็นว่า engineer ของ Microsoft สร้าง alias เพื่อให้คล้ายกับ C++ เพื่อให้โปรแกรมเมอร์เข้าใจได้ง่าย เป็นแนวคิดเก่า ตอนนี้ก็ควรกลับมาใช้ BCL type ซึ่งเป็น design เริ่มต้นของ C# เสียที [[1]](http://stackoverflow.com/questions/7074/whats-the-difference-between-string-and-string)

ทำไมถึงควรใช้ BCL type ก็เนื่องจาก Api บางตัวของ .Net ใช้ชื่อเดียวกับ BCL type เช่น `Convert.ToSingle` ฟังก์ชันนี้ return ค่าเป็น Single (float) สามารถเขียนได้สองแบบ คือ

```csharp
float value = Convert.ToSingle("12.0")
Single value = Convert.ToSingle("12.0")
```

แบบแรก ต้องการ convert เป็น Single แต่กลับ return ค่าออกมาเป็น float
แบบที่สอง จะดูตรงไปตรงมา คือ convert เป็น Single และรับค่าด้วย Single

เรื่องนี้เกี่ยวกับ readability ถึงแม้โค้ดทั้งสองแบบจะทำงานเหมือนกัน แต่แบบที่สองจะเข้าใจได้ง่ายกว่า แม้กับคนที่ไม่เคยเขียน C# ก็ตาม

ความเห็นอีกแบบคือ ควรใช้ alias เป็นหลัก จะใช้ BCL type ก็ต่อเมื่อมีการอ้างถึงสมาชิกในคลาสเท่านั้น เพราะไม่ make sense ถ้าจะเรียกใช้ method จาก keyword ควรเรียกจาก class มากกว่า

จากความเห็นข้างต้น สามารถสรุป style การใช้ได้ 3 แบบ คือ

แบบที่ 1 ใช้ BCL ทั้งหมด

```csharp
String text;
String.IsNullOrEmpty(text);
Double number;
Double.TryParse(text, out number);
```

แบบที่ 2 ใช้ alias ทั้งหมด

```csharp
string text;
string.IsNullOrEmpty(text);
double number;
double.TryParse(text, out number);
```

แบบที่ 3 ใช้ alias ในประโยคประกาศตัวแปร และใช้ BCL เมื่อมีการเรียกใช้ method ใน class

```csharp
string text;
String.IsNullOrEmpty(text);
double number;
Double.TryParse(text, out number);
```

Links

1. [To String or to string.](http://haacked.com/archive/2015/12/16/to-string-or-not/)
2. [What's the difference between String and string?.](http://stackoverflow.com/questions/7074/whats-the-difference-between-string-and-string)
3. [Language or BCL data types.](https://github.com/dotnet/corefx/issues/391)
