---
title: Explicit conversion ใน C#
date: 2016-01-30 01:06:18
tags: cs
---

การแปลง type ใน C# มีหลายแบบ

- Implicit conversion - แปลงแบบอัติโนมัติโดย compiler
- Explicit conversion (cast) - แปลงโดยใช้ keyword `as` หรือ ใช้ operator cast
- User-define conversion - เขียน method สำหรับแปลง type ขึ้นเอง
- Helper class - ใช้ helper ที่มีอยู่แล้ว เช่น คลาส `Convert`

## Explicit conversion

#### ใช้ cast

``` csharp
object x = ...;
if (x is String) {
    var str = (String) x;
}
```

Cast มักใช้คู่กับ `is` เพื่อทำ type checking สำหรับกัน error  `InvalidCastException` ในกรณีที่ CLR ไม่สามารถแปลง object ให้เป็น type ที่ต้องการได้

#### ใช้ `as`

``` csharp
var str = x as string;
if (str != null) {
    // Use str
}
```

การแปลง type โดยใช้ `as` จะช่วยลดขั้นตอน type checking และไม่มีทาง error เนื่องจาก operator `as` จะคืนค่า `null` ในกรณีที่ไม่สามารถแปลง type จะต่างจาก cast ที่จะเกิด error `InvalidCastException` ข้อจำกัดของ `as` คือใช้ได้กับ type ที่เป็น `Nullable` เท่านั้น

#### ใช้ `as` ใน `for`

``` csharp
for (var str = x as string; str != null; str = null) {
    // Use str
}
```

เราสามารถใช้ `as` ว่างไว้ใน expression for เพื่อกันตังแปร `str` ให้อยู่ใน scope วิธีนี้เป็น programming trick ที่ทำให้โค้ดอ่านยาก จึงไม่นิยมใช้


## Performance

การ convert type มีผลกับประสิทธิภาพของโปรแกรม ถ้าไม่สามารถหลีกเลี่ยงได้ ก็ควรทำให้น้อยที่สุด การใช้ `as` แทน cast จะช่วยลดจำนวน conversion ได้หนึ่งครั้ง เนื่องจากการทำ type checking ด้วย `is` จะเกิด type conversion เช่นกัน

อย่างไรก็ตามการใช้ as กับข้อมูลประเภท `Nullable` ของ value type เช่น `int?` จะ drop performance ของโปรแกรมมากกว่าการใช้ `is` + cast อย่างมีนัยยสำคัญ เนื่องจากการแปลง value type ให้เป็น `Nullable` จะมี operator อื่นเพิ่มเข้ามา

``` csharp
class Test {
    const int Size = 30000000;
    static void Main() {
        object[] values = new object[Size];
        for (int i = 0; i < Size - 2; i += 3) {
            values[i] = null;
            values[i+1] = "";
            values[i+2] = 1;
        }
        FindSumWithCast(values);
        FindSumWithAs(values);
        FindSumWithLinq(values);
    }
    static void FindSumWithCast(object[] values) {
        Stopwatch sw = Stopwatch.StartNew();
        int sum = 0;
        foreach (object o in values) {
            if (o is int) {
                int x = (int) o;
                sum += x;
            }
        }
        sw.Stop();
        Console.WriteLine("Cast: {0} : {1}", sum, (long) sw.ElapsedMilliseconds);
    }
    static void FindSumWithAs(object[] values) {
        Stopwatch sw = Stopwatch.StartNew();
        int sum = 0;
        foreach (object o in values) {
            int? x = o as int?;
            if (x.HasValue) {
                sum += x.Value;
            }
        }
        sw.Stop();
        Console.WriteLine("As: {0} : {1}", sum, (long) sw.ElapsedMilliseconds);
    }
    static void FindSumWithLinq(object[] values) {
        Stopwatch sw = Stopwatch.StartNew();
        int sum = values.OfType<int>().Sum();
        sw.Stop();
        Console.WriteLine("LINQ: {0} : {1}", sum, (long) sw.ElapsedMilliseconds);
    }
}
```

จากตัวอย่าง `FindSumWithCast` การ cast แบบปกติ สามารถ `unbox` object ให้เป็น type `int` ได้โดยตรง ส่วน `FindWithAs` จะต้อง `unbox` ให้เป็น `Nullable int` หรือ `int?` ซึ่งจะมี internal operation เกิดขึ้นในฟังก์ชั่น `JIT_Unbox_Nullable` ของ `JIT`

```bash
Cast: 10000000 : 74
As: 10000000 : 316
LINQ: 10000000 : 913
```

ผลการรัน ในกรณีที่ใช้ cast ให้ประสิทธิภาพที่ดีกว่าการใช้ `as` ถึงแม้จะมี type conversion เกิดขึ้นสองครั้งก็ตาม

Links

- https://msdn.microsoft.com/en-us/library/ms173105.aspx
- http://stackoverflow.com/questions/1583050/performance-surprise-with-as-and-nullable-types
- http://ericlippert.com/2015/10/14/casts-and-type-parameters-do-not-mix

