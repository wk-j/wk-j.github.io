---
title: เขียน Entity Framework Code First ด้วย F#
date: 2016-02-04 23:19:15
tags: fs
---

## Dependencies

ตัวอย่างนี้จะใช้ EF เชื่อมกับฐานข้อมูล PostgreSQL ซึ่งจะต้องใช้ dependencies สองตัว คือ `EntityFramework` และ `Npgsql.EntityFramework` สามารถติดตั้งผ่าน Package Manager Console ดังนี้

```bash
Install-Package EntityFramewok
Install-Package Npgsql.EntityFramework
```

## Config

ในไฟล์ `App.config` ต้องเพิ่มแท็ก `configSections` `system.Data` `entityFramework` และ `connectionString` โดยชื่อ connection ในที่นี่จะใช้คำว่า `production` พร้อมระบุรายละเอียดของฐานข้อมูลไว้ใน attribute ชื่อ `connectionString`

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
  </configSections>
  <system.data>
    <DbProviderFactories>
      <remove invariant="Npgsql" />
      <add name="Npgsql Data Provider" invariant="Npgsql" support="FF" description=".Net Framework Data Provider for Postgresql" type="Npgsql.NpgsqlFactory, Npgsql" />
    </DbProviderFactories>
  </system.data>
  <entityFramework>
    <defaultConnectionFactory type="System.Data.Entity.Infrastructure.SqlConnectionFactory, EntityFramework" />
    <providers>
      <provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
      <provider invariantName="Npgsql" type="Npgsql.NpgsqlServices, Npgsql.EntityFramework" />
    </providers>
  </entityFramework>
  <connectionStrings>
    <add name="production" providerName="Npgsql" connectionString="Server=192.168.0.xxx;Port=5432;Database=DbName; User Id=xxx;Password=xxx;" />
  </connectionStrings>
</configuration>
```

## Models

EF Code First ต้องประกาศ Entity class ในตัวอย่างนี้จะมีเพียงคลาสเดียว คือ `QUser` ซึ่ง extend จาก abstract class ชื่อ `Common` ที่ประกาศ primary key ไว้

```fsharp
[<AbstractClass>]
type Common() =
    [<Key>]
    member val Id = 0 with get, set
type QUser() =
    inherit Common()
    member val FirstName = "" with set,get
    member val LastName = "" with set,get
    member val Position = "" with set,get
    member val Email = "" with set,get
```

## Context

Context ใน EF ใช้เป็น interface สำหรับ `insert` `update` `delete` และ  `query` ข้อมูล โดยการประกาศ context ต้อง inherit class ชื่อ `DbContext`

```fsharp
type AppContext(connection:string) =
    inherit DbContext(connection)
    [<DefaultValue>]
    val mutable private users: DbSet<QUser>
    member this.Users
        with get() = this.users
        and set v = this.users <- v
```

`AppContext` มี parameter 1 ตัวชื่อ `connection` เพื่อใช้สำหรับส่งชื่อ `connection` ที่ระบุไว้ในไฟล์ `App.config`

## Create Schema

เมื่อเขียน Entity class และ context ครบแล้ว ต่อไปคือการ generate ฐานข้อมูลให้ได้ table และ field ตรง ที่ตามที่ประกาศไว้ใน Entity class วิธีการคือ ให้เพิ่มโค้ด `Database.SetInitializer` ก่อนที่จะทำ operation ใด ๆ ก็ตามกับฐานข้อมูล

```fsharp
Database.SetInitializer<AppContext>
    (new DropCreateDatabaseIfModelChanges<AppContext>());
use context = new AppContext("production")
let rs = context.Users.Where(fun x -> x.FirstName = "").FirstOrDefault()
```

จากตัวอย่างใช้วิธีส่งคำสั่ง `new DropCreateDatabaseIfModelChanges<AppContext>()` ซึ่งหมายความว่าให้ drop ฐานข้อมูลและสร้างใหม่ถ้าตรวจเจอว่า Entity class มีการแก้ไข

หลังจาก Generate schema เราสามารถ `insert` `update` ฐานข้อมูลได้โดยใช้ syntax แบบ LINQ method หรือผ่าน LINQ query ก็ได้ สำหรับ LINQ query จะมี syntax ที่แตกต่างจาก C# สามารถดูตัวอย่างได้ตามลิงค์ด้านล่างนี้

Links

- http://fsprojects.github.io/FSharp.Linq.ComposableQuery/QueryExamples.html

