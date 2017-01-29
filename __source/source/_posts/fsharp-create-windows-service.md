---
title: เขียน Windows Service ด้วย  F#
date: 2016-03-10 22:15:14
tags: fs
---


## Dependencies

- `System.ServiceProcess`
- `System.Configuration.Install`


## Service

เขียน Service โดย extend คลาสที่ชื่อ `ServiceBase` และ override `OnStart()` และ `OnStop()` ซึ่งเป็นฟังก์ชันที่จะถูกเรียกเมื่อมีการ Start/Stop service

``` fsharp
type WindowsService() =
    inherit ServiceBase()
    let mutable service : IDisposable = null

    override x.OnStart(args) =
        let url = LoadAppConfig().Uri
        service <- WebApp.Start<Startup.Startup>(url)
        base.OnStart(args)

    override x.OnStop() =
        if obj.ReferenceEquals(null, service) |> not then service.Dispose()
        base.OnStop()
```


## Installer

Installer เป็นคลาสที่ใช้สำหรับระบุรายละเอียดของ Service เช่น ชื่อ Service วิธีการ Start จะให้ Start แบบ Auto หรือ Manual สามารถเขียน Installer โดย extend คลาส `Installer` และระบุ attribute `RunInstaller` ไว้ที่ชื่อคลาส

``` fsharp
[<RunInstaller(true)>]
type MyInstaller() as this =
    inherit Installer()
    do
        let spi = new ServiceProcessInstaller()
        let si = new ServiceInstaller()
        spi.Account <- ServiceAccount.LocalSystem
        spi.Username <- null
        spi.Password <- null
        si.DisplayName <- "MyService"
        si.Description <- "MyService"
        si.StartType <- ServiceStartMode.Automatic
        si.ServiceName <- "MyService"
        this.Installers.Add(spi) |> ignore
        this.Installers.Add(si) |> ignore
```


## Entry Point

หลังจากเขียน Service และ Installer แล้ว ใน EntryPoint ของโปรแกรม สามารถเรียก Service โดยใช้ Static Method `Run` ของคลาส `ServiceBase`

``` fsharp
[<EntryPoint>]
let main argv =
    use service = new WindowsService()
    ServiceBase.Run(service)
    0
```

## Install / Uninstall Script

เมื่อ Compile โปรแกรมเป็น .exe แล้ว สามารถติดตั้งโปรแกรมให้รันเป็น Windows Service โดยใช้ utility ที่มาพร้อมกัน .Net Framework คือ `installutil.exe`

- ใช้ option `/I` สำหรับติดตั้ง
- ใช้ option `/U` สำหรับ Uninstall

### Install Service

```bash
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe" /I "%~dp0MyService.exe"
```

### Uninstall ServiceName

```bash
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe" /U "%~dp0MyService.exe"
```

การบุชื่อโปรแกรมจะต้องใช้ Absolute path จากตัวอย่างจะใช้ ตัวแปร `%-dp0` เพื่อดึง path เต็มของโปรแกรม
