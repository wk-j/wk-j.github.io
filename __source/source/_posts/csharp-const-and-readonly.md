---
title: Const กับ Readonly
date: 2016-01-18 22:22:24
tags:
---

## Constant

ค่าคงที่แบบ compile time constant ใน C# สามารถประกาศด้วย keyword `const` ซึ่งจะใช้ได้กับ data ประเภท primitive type ดังต่อไปนี้ เท่านั้น

```csharp
Boolean, Char, Byte, SByte, Int16, UInt16,
Int32, UInt32, Int64, UInt64, Single, Double, Decimal, String
```

ค่าคงที่แบบ `const` จะไม่มีทางเปลี่ยนแปลง มันจึงถูก define เป็นส่วนหนึ่งของ type แบบ static member ไม่ใช่ instance member เหมือน field ทั่วไป

เมื่อมีการอ้างอิง `const` ณ ส่วนใดของโค้ด compiler จะดึงค่าที่ผูกไว้ไปฝั่งไว้ใน IL ทำให้ขณะรันโปรแกรมไม่จำเป็นต้อง allocate memory

### ข้อจำกัดของ constant เพื่อเทียบกับ field

- ไม่สามารถหาค่า address ของ constant ได้
- ไม่สามารถส่งค่า `const` แบบ pass by reference ผ่าน keyword `ref`
- ในกรณีที่มีการเปลี่ยนค่า `const` ใน assembly ที่อ้างอิง จำเป็น ต้อง compile โปรแกรมใหม่

### ตัวอย่าง

``` csharp
using System;
public class Program {
    private const string ProductName = "Visual C#";
    private static string _version = "1.0";
    public static void Main(String[] args) {
        var name = ProductName;
        var version = _version;
        Console.WriteLine("{0} {1}", name, version);
    }
}
```

จากตัวอย่าง มีการประกาศตัวแปรแบบ static ชื่อ `_version` และมีค่า constant ชื่อ `ProductName`

``` csharp
.method public hidebysig static
	void Main (
		string[] args
	) cil managed
{
	// Header Size: 12 bytes
	// Code Size: 27 (0x1B) bytes
	// LocalVarSig Token: 0x11000001 RID: 1
	.maxstack 3
	.entrypoint
	.locals init (
		[0] string,
		[1] string
	)

	/* 0x0000025C 00           */ IL_0000: nop
	/* 0x0000025D 7201000070   */ IL_0001: ldstr     "Visual C#"
	/* 0x00000262 0A           */ IL_0006: stloc.0
	/* 0x00000263 7E02000004   */ IL_0007: ldsfld    string Program::_version
	/* 0x00000268 0B           */ IL_000C: stloc.1
	/* 0x00000269 7215000070   */ IL_000D: ldstr     "{0} {1}"
	/* 0x0000026E 06           */ IL_0012: ldloc.0
	/* 0x0000026F 07           */ IL_0013: ldloc.1
	/* 0x00000270 280400000A   */ IL_0014: call      void [mscorlib]System.Console::WriteLine(string, object, object)
	/* 0x00000275 00           */ IL_0019: nop
	/* 0x00000276 2A           */ IL_001A: ret
} // end of method Program::Main
```

เมื่อ compile เป็นคำสั่ง IL จะพบความแตกต่างระหว่างตัวแปรแบบ `static` ธรรมดากับค่า `static const` ดังนี้

- บรรทัดที่ 17 จะเห็นว่าตำแหน่งที่อ้างถึง ค่า `const` มีการฝั่งค่า string literal `Visual C#` ไว้เลย
- บรรทัดที่ 19 ตำแหน่งตัวแปรแบบ `static` ยังมีการอ้างอิงไปยัง ชื่อตัวแปรโดยใช้คำสั่ง `ldsfld string Program::_version` อยู่


## Readonly

C# มี keyword `readonly` สำหรับกำหนดค่าคงที่แบบ runtime constant จะต่างจาก `const` คือ `readonly` สามารถใช้ได้กับ reference type และสามารถย้ายการกำหนดค่ามาไว้ใน constructure ได้

### ตัวอย่าง

```csharp
public class Cat {
    private static readonly int Leg;
    private static readonly int Eye = 2;
    public Cat() {
        Leg = 4
    }
}
```

จากตัวอย่างเราสามารถกำหนดค่า constant แบบ readonly ได้ใน constructure หรือกำหนดค่าบรรทัดเดียวกับการประกาศชื่อตัวแปรก็ได้

### `readonly` ต่างจาก `const` อย่างไร

- สามารถใช้กับ reference type
- ค่าคงที่แบบ `readonly` จะถูก evaluate ขณะรัน จึงไม่มีปัญหา cross-assembly versioning เหมือน `const`
- `readonly` ไม่จำเป็นต้องเป็น `static`
- แต่ละ instance ของคลาสสามารถมีค่าต่างกันได้ เพราะสามารถกำหนดค่าผ่าน constructure