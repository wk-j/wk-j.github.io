
---
title: Checked กับ Unchecked
date: 2016-01-11 18:11:57
tags: cs
---

## Max & Min

Primitive type ใน C# เช่น short float หรือ int จะมี min และ max value เป็นค่าต่ำสุดและสูงสุดที่เป็นไปได้ ค่านี้ถูกกำหนดโดยจำนวน bit ของ type เช่น int มี 32 bit เลขฐานสองขนาด 32 bit สามารถแสดงตัวเลขได้ถึง 4294967296 แต่เนื่องจาก int เป็น sign integer ต้องใช้ 1 bit สำหรับเก็บ flag + หรือ - ดังนั้น bit ที่ใช้สำหรับเก็บตัวเลขจึงเหลือ 31 bit ทำให้สามารถเก็บค่าต่ำสุดและสูงสุดดังนี้

```csharp
public const Int32 MaxValue = 2147483647;
public const Int32 MinValue = -2147483648;
```

จากปัจจัยนี้ทำให้ int ไม่สามารถใช้เก็บค่าที่ต่ำหรือสูงกว่า min และ max นี้ได้ ถ้าพยายาม assign ค่านอกจาก range นี้ก็จะเกิดสิ่งที่เรียกกว่า overflow

ภาษาต่าง ๆ จะมีวิธีจัดการกับ overflow ที่ต่างกัน เช่น C, C++ จะไม่สนใจ overflow และไม่ถือว่า overflow คือ error ดังนั้นโปรแกรมเมอร์ ต้องควบคุมจัดการเอง

ภาษา Visual Basic จะตรวจจับ overflow ขณะทำงาน และจะ throw exception ออกมาเมื่อเจอเคสแบบนี้

ส่วน C# โดย default จะปิดการตรวจจับ overflow ไว้ คือ unchecked นั่นเอง แต่สามารถเปิดให้ checked ได้ด้วยการ compile ด้วย flag `/checked+` ซึ่ง compiler จะเพิ่มโค้ดชุดพิเศษแทนที่ชุดปกติ เช่น เปลี่ยน IL instruction จาก `add` เป็น `add.ovf`

## ทดสอบ

มาดูกันว่าโปรแกรม `Test.cs` ที่จะใช้ทดสอบเมื่อ compile ด้วย flag `/checked+` โปรแกรมที่ได้นั้นต่างกับโปรแกรมปกติอย่างไร

``` csharp
using System;
public class Test {
    public static void Main(String[] args) {
        var i = Int32.MaxValue + Int32.Parse("1");
        Console.WriteLine(i);
    }
}
```

ลอง compile Test.cs เป็นสองแบบ คือ แบบธรรมดา และแบบมี `/checked+`

1. Compile Test.cs เป็น exe ก่อนด้วย dmcs
2. Decompile exe ที่ได้จากข้อ 1 ด้วย monodis เพื่อเช็ค IL instruction

Compile และ decompile แบบธรรมดา

``` bash
dmcs -out:Unchecked.exe Unchecked.cs
monodis --output=Unchecked.il Unchecked.exe
```

Compile ด้วย `/checked+`

``` bash
dmcs /checked+ -out:Checked.exe Checked.cs
monodis --output=Checked.il Checked.exe
```

## IL instruction

IL instruction ของโปรแกรมต่างกันอย่างไร

**แบบที่ 1** Unchecked.il ได้จาก Unchecked.exe

``` csharp
// method line 2
.method public static hidebysig
       default void Main (string[] args)  cil managed
{
    // Method begins at RVA 0x2058
.entrypoint
// Code size 24 (0x18)
.maxstack 2
.locals init (
    int32	V_0)
IL_0000:  ldc.i4 2147483647
IL_0005:  ldstr "1"
IL_000a:  call int32 int32::Parse(string)
IL_000f:  add
IL_0010:  stloc.0
IL_0011:  ldloc.0
IL_0012:  call void class [mscorlib]System.Console::WriteLine(int32)
IL_0017:  ret
} // end of method Test::Main
```
**แบบที่ 2** Checked.il ได้จาก Checked.exe

``` csharp
// method line 2
.method public static hidebysig
       default void Main (string[] args)  cil managed
{
// Method begins at RVA 0x2058
.entrypoint
// Code size 24 (0x18)
.maxstack 2
.locals init (
    int32	V_0)
IL_0000:  ldc.i4 2147483647
IL_0005:  ldstr "1"
IL_000a:  call int32 int32::Parse(string)
IL_000f:  add.ovf
IL_0010:  stloc.0
IL_0011:  ldloc.0
IL_0012:  call void class [mscorlib]System.Console::WriteLine(int32)
IL_0017:  ret
} // end of method Test::Main
```

จะเห็นว่า IL instruction ของทั้งสองโปรแกรมมีหน้าแทบเหมือนกัน ยกเว้นเพียงบรรทัดที่ 14 ที่มี instruction ต่างกัน คือ `add` กับ `add.ovf`


## การทำงาน

จาก IL ที่ได้จะเห็นว่า มีเพียง instruction เดียวที่ต่างกัน แล้วโปรแกรมทั้งสองจะทำงานต่างกันหรือไม่

**1. รันโปรแกรมแบบ unchecked**

รันโปรแกรมได้ปกติ ไม่เกิด exception แต่ได้ผลลัพท์ คือ `-2147483648` ซึ่งผิดคาดจากที่ตั้งใจไว้ ตาม sense การบวกเลขเราน่าจะได้ผลลัพท์ลักษณะนี้ `2147483647 + 1 = 21474836478`

``` bash
$ mono Unchecked.exe
-2147483648
```

**2. โปรแกรมแบบ checked**

จะมี error คือ System.OverflowException ซึ่งหมายความว่า CLR ตรวจเจอ overflow จึงทำการ throw exception ออกมา

``` bash
$ mono Checked.exe

Unhandled Exception:
System.OverflowException: Arithmetic operation resulted in an overflow.
   at Test.Main (System.String[] args) in <filename unknown>:line 0
[ERROR] FATAL UNHANDLED EXCEPTION: System.OverflowException: Arithmetic operation resulted in an overflow.
   at Test.Main (System.String[] args) in <filename unknown>:line 0
```

## สรุป

จากการทดสอบจะเห็นว่า โปรแกรมที่ไม่เช็ค overflow สามารถทำงานได้ โดยไม่มี error แต่ผลลัพท์ไม่ถูกต้อง ในการใช้งานจริงโปรแกรมเมอร์ต้องแน่ใจว่า โค้ดที่เขียนจะต้องอยู่ภายใน range ของ type นั้น มิฉะนั้นโปรแกรมก็จะทำงานผิดพลาด

ส่วนโปรแกรมที่เช็ค overflow ซึ่งโดยปกติจะมีประสิทธิภาพต่ำกว่าแบบ unchecked เล็กน้อย จะมี runtime exception และไม่แสดงผลลัพท์ใด ๆ ในทางปฏิบัติโปรแกรมเมอร์สามารถจัดการ exception ที่เกิดขึ้นโดยใช้ประโยค `try ... catch` จากนั้นก็เขียนโลจิกเพิ่มเติมเพื่อกัดการกับ error เพื่อช่วยป้องกันความผิดพลาดของโปรแกรม

สรุปสั้น ๆ คือ

- unchecked เร็วกว่า
- checked ปลอดภัยกว่า


## หมายเหตุ

นอกจากการใช้ compiler flag `/checked+` ซึ่งจะมีผลกับ instruction ของทั้งโปรแกรม เราสามารถเขียนโค้ดสำหรับ checked หรือ unchecked overflow เฉพาะส่วนที่ต้องการได้ โดยใช้ operator `checked` กับ `unchecked` ตามตัวอย่างนี้

```csharp
using System;
public class Test {
    public void Checked1() {
        Int32 i = Int32.MaxValue;
        i = checked (i + 1);
    }
    public void Checked2() {
        checked {
            Int32 i = Int32.MaxValue;
            i = i + 1;
        }
    }
    public void Unchecked() {
        unchecked {
            Int32 i = Int32.MaxValue;
            i = i + 1;
        }
    }
}
```

