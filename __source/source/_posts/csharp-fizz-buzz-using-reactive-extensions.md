---
title: Fizz Buzz โดยใช้ Reactive Extensions
date: 2016-02-03 21:58:03
tags: cs
---

## Tools

- paket - https://fsprojects.github.io/Paket
- xbuild - http://www.mono-project.com/docs/tools+libraries/tools/xbuild

## Dependencies

ไลบรารี่ [Reactive Extensions](http://reactivex.io) ของ .Net ชื่อ `Rx-Main` เมื่อตั้งตั้งแล้วจะได้ dll ทั้งหมด 4 ไฟล์ คือ `System.Reactive.dll` `System.Reactive.Interfes.dll` `System.Reactive.Linq.dll` และ `System.Reactive.PlatformServices.dll` เราสามารถระบุ dependencies นี้ไว้ในไฟล์ `paket.dependencies`

ไฟล์ `paket.dependencies`

```bash
source https://www.nuget.org/api/v2
nuget Rx-Main
```

ทำการติดตั้ง `Rx-Main` โดยใช้ paket ผ่านคำสั่งใช้คำสั่ง

```bash
paket install
```

โปรแกรม paket จะดาวโหลด dll ทั้งหมดไว้ในโฟลเดอร์ packages

## Project

เมื่อตั้งตั้ง dependencies ครบแล้วทำการสร้างไฟล์ project โดยตั้งชื่อว่า `FizzBuzz.xml` และระบุ dependencies และ `HintPath` ไว้ในแท็ก `ItemGroup`

สำหรับไฟล์ `Program.cs` ที่เป็นโค้ดของโปรแกรมจะเก็บไว้ใน `ItemGroup` แยกอีกอัน

ไฟล์ `FizzBuzz.xml`

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="12.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolVersion)\Microsoft.Common.props"
        Condition="Exists('$(MSBuildExtensionPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"/>
    <ItemGroup>
        <Reference Include="System"/>
        <Reference Include="System.Core"/>
        <Reference Include="System.Reactive.Core">
            <HintPath>packages/Rx-Core/lib/net45/System.Reactive.Core.dll</HintPath>
        </Reference>
        <Reference Include="System.Reactive.Interfaces">
            <HintPath>packages/Rx-Interfaces/lib/net45/System.Reactive.Interfaces.dll</HintPath>
        </Reference>
        <Reference Include="System.Reactive.Linq">
            <HintPath>packages/Rx-Linq/lib/net45/System.Reactive.Linq.dll</HintPath>
        </Reference>
        <Reference Include="System.Reactive.PlatformServices">
            <HintPath>packages/Rx-PlatformServices/lib/net45/System.Reactive.PlatformServices.dll</HintPath>
        </Reference>
    </ItemGroup>
    <ItemGroup>
        <Compile Include="Program.cs"/>
    </ItemGroup>
    <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets"/>
</Project>
```

## Source

ในโปรแกรมจะมี static เมธอดชื่อ `Generate` ข้างในมีการใช้ `Observable` ที่เป็น Reactive Extensions โดยมีการใช้ filter ทั้งหมด 4 ตัวคือ `dividedByThree` `dividedByFive` `dividedByThreeAndFive` และ `simpleNumbers` ในโปรแกรมมีการ compose filter ทั้งหมดรวมกันโดยใช้ฟังก์ชัน `Merge`

ไฟล์ `Program.cs`

```csharp
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;
public class Program {
    public static IEnumerable<string> Generate(int max) {
        var result = new List<string>();
        if (max <= 0) { return result; }
        var observable = Observable.Range(1, max);
        var dividedByThree = observable.Where(i => i % 3 == 0).Select(_ => "Fizz");
        var dividedByFive = observable.Where(i => i % 5 == 0).Select(_ => "Buzz");
        var dividedByThreeAndFive = observable.Where(i => i % 15 == 0).Select(_ => "FizzBuzz");
        var simpleNumbers = observable.Where(i => i % 3 != 0 && i % 5 != 0).Select(i => i.ToString());
        var specialCases = (dividedByThreeAndFive)
            .Merge(dividedByThree)
            .Merge(dividedByFive);
        simpleNumbers.Merge(specialCases).Subscribe(s => result.Add(s));
        return result;
    }
    public static void Main(String[] args) {
        Generate(100).ToList().ForEach(Console.WriteLine);
    }
}
```

## Build

คอมไพล์โค้ดโดยใช้ `xbuild` ผ่านคำสั่ง

```bash
xbuild FizzBuzz.xml
```

`xbuild` จะสร้างไฟล์ `FizzBuzz.exe` เก็บไว้ในโพลเดอร์ `bin/Debug`

### Run

```
mono bin/Debug/FizzBuzz.exe
```

Links

- http://blog.drorhelper.com/2015/02/fizzbuzz-tdd-kata-using-reactive.html