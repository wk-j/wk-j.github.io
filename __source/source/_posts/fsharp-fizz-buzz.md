---
title: Fizz Buzz in F#
date: 2016-01-24 16:46:08
tags: fs
---

## Fuzz Buzz

Fizz Buzz เป็นโจทย์ที่นิยมใช้สำหรับ ทดสอบการเขียนโปรแกรม กฎคือ ให้เขียนโปรแกรมที่รันเลขตั้งแต่ 1 - 100 โดย

1. ถ้าเลขนั้นหาร 3 ลงตัวให้พิมพ์ `Fizz`
2. ถ้าเลขนั้นหาร 5 ลงตัวให้พิมพ์ `Buzz`
3. และถ้าเลขนั้นหารทั้ง 3 และ 5 ลงตัวให้พิมพ์ `FizzBuzz`

ใน F# สามารถแก้ปัญหานี้ได้หลายวิธี วิธีที่ง่ายสุดคือใช้ Pattern matching อีกวิธีคือใช้ Active pattern ซึ่งจะทำให้โค้ดอ่านง่ายขึ้น ส่วนวิธีสุดท้ายที่ยากสุดคือ ใช้ Computation expression

## Pattern matching

Pattern matching แบบแรกคือ ใช้วิธีีคำนวณตัวเลขทีตำแหน่ง matching case โดยใช้ when guard

```fsharp
let fizzBuzz number =
    match number with
    | n when n % 15 = 0 -> "FizzBuzz"
    | n when n % 3 = 0 -> "Fizz"
    | n when n % 5 = 0 -> "Buzz"
    | n -> sprintf "%d" n

[1..100]
|> List.map fizzBuzz
|> List.iter (printfn "%s")
```

หรือคำนวนในประโยค `match ... with`

```fsharp
let fizzBuzz i =
    match i % 3, i % 5 with
    | 0, 0 -> "FizzBuzz"
    | 0, _ -> "Fizz"
    | _, 0 -> "Buzz"
    | _ -> string i
[1..100] |> Seq.map fizzBuzz0 |> Seq.iter (printfn "%s")
```

## Partial active pattern.

ใช้ Partial active pattern สองตัว คือ `P3` และ `P5`

```fsharp
let (|P3|_|) i = if i % 3 = 0 then Some i else None
let (|P5|_|) i = if i % 5 = 0 then Some i else None

let f = function
  | P3 _ & P5 _ -> printfn "FizzBuzz"
  | P3 _        -> printfn "Fizz"
  | P5 _        -> printfn "Buzz"
  | x           -> printfn "%d" x

Seq.iter f {1..100}
//or
for i in 1..100 do f i
```

หรือใช้ Partial case เดียว คือ `DivisibleBy` ซึ่งเป็น Optimize version ของแบบแรก

```fsharp
let (|DivisibleBy|_|) divisor i =
    if i % divisor = 0 then Some () else None
for i in 1..100 do
    match i with
    | DivisibleBy 3 & DivisibleBy 5 -> printfn "FizzBuzz"
    | DivisibleBy 3 -> printfn "Fizz"
    | DivisibleBy 5 -> printfn "Buzz"
    | _ -> printfn "%d" i
```

## Computation expression

วิธีที่ยากสุด คือ เขียนด้วย Computation expression

```fsharp
type M<'T>  = M of 'T
type MonadBuilder() =
    member this.Return x = M x
let m = MonadBuilder()
let fizz = function
    | x when x % 3 = 0  -> m { return x, "Fizz"}
    | x -> m { return x, x.ToString() }
let buzz = function
    | M (x,s) when x % 5 = 0 -> m { return x, s + "Buzz" }
    | M (x,"") -> m { return x, x.ToString() }
    | M (x,s) -> m { return x, s }

[1..100]
|> List.map(fizz >> buzz)
|> List.iter(fun (M (_,s)) -> printfn "%A" s)
```

Links

- http://stackoverflow.com/questions/2422697/how-to-code-fizzbuzz-in-f
- http://zecl.hatenablog.com/entry/20110711/p1
- https://twitter.com/CallMeKohei/status/673677619250913280
