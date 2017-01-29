---
title: เขียน F# แบบ Inline evaluation ด้วย Light Table
date: 2016-01-22 19:38:28
tags: fs
---

โปรแกรม [Light Table](http://lighttable.com) เป็น Text editor ที่สามารถเขียนโปรแกรมแบบ Inline evaluation คือ เราสามารถพิมพ์โค้ดและสั่ง Execute คำสั่งในบรรทัดนั้น เพื่อดูผลลัพธ์ได้ทันที

ข้อดีของการเขียนโปรแกรมแบบนี้ คือ เราสามารถใช้ทดสอบโค้ดได้ง่าย ไม่ต้องใช้ IDE หรือเขียนโปรแกรมพิมพ์ผลลัพธ์ผ่าน Console  ที่มีความยุ่งยาก

## เครื่องมือที่ต้องใช้

1. โปรแกรม F# interactive fsharpi (Mac) / fsi (Windows)
2. โปรแกรม Light Table
3. F# plugin ของ Light Table

## ติดตั้ง fsharpi / fsi

- Mac - http://fsharp.org/use/mac
- Windows - http://fsharp.org/use/windows

## ติดตั้ง Light Table

- สามารถโหลดไฟล์ [Installer](http://lighttable.com/) จากเว็บไซต์ได้โดยตรง

## ติดตั้ง F# plugin

หลังจากติดตั้ง Light Table สามารถติดตั้ง Plugin ผ่าน Plugin manager ตามขั้นตอนต่อไปนี้

1. เปิดโปรแกรม Light Table
2. เปิดหน้าต่าง Plugin manager โดยการกดปุ่ม `Ctrl + Space` พิมพ์คำว่า `plugin` แล้วคลิกเลือก `Show plugin manager`

    ![](images/open-plugin-manager.png)

3. ในหน้า Plugin manager ให้ Search คำว่า `F#` แล้วคลิกปุ่ม Install ที่มุมด้านขวา

    ![](images/install-plugin.png)

## การใช้งาน

หนังจากติดตั้ง Plugin เราสามารถใช้งานได้ทันที่ ด้วยวิธีง่าย ๆ ดังนี้

1. พิมพ์โคัด
2. Hightlight บรรทัดที่ต้องการ
3. กด `Ctrl + Enter` (Windows) / `Command + Enterl` (Mac)

    ![](images/inline-evaluation.png)

จากตัวอย่าง

- ถ้าบรรทัดที่เลือก return void โปรแกรมจะแสดงเครื่องหมายถูก
- ส่วนบรรทัดที่ return ค่า โปรแกรมจะแสดงผลลัทธ์ที่ท้ายบรรทัด


Links
- Light Table - http://lighttable.com
- F# plugin - https://github.com/enricosada/LightTable-FSharp
- Use F# on Windows - http://fsharp.org/use/windows
- Use F# on Mac - http://fsharp.org/use/mac
