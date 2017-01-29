---
title: + กับ StringBuilder
date: 2016-02-02 11:04:13
tags: cs
---

## Concat String ด้วยเครื่องหมาย +

```csharp
DateTime start = DateTime.Now;
string x = "";
for (int i=0; i < 100000; i++) {
 x += "!";
}
DateTime end = DateTime.Now;
Console.WriteLine ("Time taken: {0}", end-start);
```

เนื่องจาก `string` เป็น immutable type ทุกครั้งที่ `x += "!"` ทำงานจะมีการสร้าง `string` ขึ้นใหม่ โดยคัดลอก `string` เดิมต่อกับเครื่องหมาย `!` ไปเก็บใน memory ที่มีการ allocate ขึ้นใหม่ จากโค้ดข้างบนจะมีการคัดลอก `string` ถึง 100,000 ครั้ง แต่ละครั้งจะใช้เวลามากขึ้นเรื่อยๆ เนื่องจากจากขนาดของ `string` ยาวขึ้น

```bash
Time taken: 00:00:09.3561040
```

## Concat String ด้วย StringBuilder

```csharp
DateTime start = DateTime.Now;
StringBuilder builder = new StringBuilder();
for (int i=0; i < 100000; i++) {
    builder.Append("!");
}
string x = builder.ToString();
DateTime end = DateTime.Now;
Console.WriteLine ("Time taken: {0}", end-start);
```

`StringBuilder` จะมี buffer อยู่ภายใน เมื่อมีการการต่อ string ด้วยฟังก์ชัน `Append` จะไม่จำเป็นต้องจอง memory ใหม่ทุกครั้ง แต่จะเกิดขึ้นเมื่อ memory หรือ buffer ที่มีอยู่ไม่พอ โดยจะเพิ่มขนาดของ buffer เป็นสองเท่าของขนาดเดิม จากโค้ดทั้งสองแบบ การใช้ `StringBuilder` จึงให้ประสิทธิภาพที่ดีกว่า

```bash
Time taken: 00:00:00.0527960
```

Links

- http://jonskeet.uk/csharp/stringbuilder.html

