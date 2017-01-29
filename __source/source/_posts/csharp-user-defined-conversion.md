---
title: User-Defined Conversion ใน C#
date: 2016-01-31 00:09:02
tags: cs
---

## Implicit กับ Explicit Conversion

```csharp
float f = 105.5f;
double d = f;
```

`float` `f` สามารถแปลงเป็น `double` `d` ผ่านการ assign ด้วยเครื่องหมาย `=` แบบอัตโนมัติ การ convert type แบบนี้เรียกว่า implicit conversion

```csharp
double d = 305.5;
float f = (float) d;
```

ในทางตรงกันข้าม เราไม่สามารถแปลง `double` `d` เป็น `float` `f` ได้โดยตรง ต้องใช้ operator cast โดยระบุ `float` เป็น parameter เราเรียก convert ที่ต้องระบุ type ปลายทางว่า explicit conversion

## User-Defined Conversion

เนื่องจาก `float` และ `double` เป็น `build-in` `type` เราจึงสามารถใช้คุณบัติ `implict` และ `explict conversion` ที่มีอยู่

ในกรณีที่ประกาศ type ใหม่ เราสามารถเขียน operator สำหรับ convert type โดยใช้ keyword `implicit` และ `explicit` โดย operator ต้องประกาศเป็น `public` `static` เสมอ

#### Implicit

```csharp
class MagicNumber {
    public int Number { set; get;}

    public static implicit operator MagicNumber(int value) {
        return new MagicNumber { Number = value };
    }
}
```

จากตัวอย่าง ในคลาส `MagicNumber` มีการใช้ keyword `implicit` `operator` เพื่อ overload operator ชื่อ `MagicNumber` ซึ่งรับ `int` ตัวเดียวเป็น parameter สิ่งที่ได้จากการ overload ชื่อคลาสลักษณะนี้ จะทำให้ `MagicNumber` มีคุณสมบัติ `implicit conversion`

```csharp
int i = 3;
MagicNumber n = i;
```

คุณสมบัติที่เพิ่มเข้ามาใหม่ ทำให้ `MagicNumber n` สามารถ assign ค่าใหม่ที่มี type `int` ได้โดยตรง

#### Explicit

```csharp
public static explicit operator int(MagicNumber value) {
    return value.Number;
}
```

การเขียน `explict convertsion` ต่างจาก `implict convertsion` เล็กน้อย คือ เปลี่ยนจาก keyword `implicit` เป็น `explicit` เท่านั้น

```csharp
MagicNumber n = new MagicNumber { Number = 2 };
int i = (int) n;
```

Links

- https://msdn.microsoft.com/en-us/library/aa288476(v=vs.71).aspx
- http://rbwhitaker.wikidot.com/c-sharp-user-defined-conversions
