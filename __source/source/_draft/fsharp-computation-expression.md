---
title: Computation Expression
date: 2016-01-31 15:04:28
tags: fs
---

Computation Expression

เป็น syntax พิเศษที่ให้เขียน flow control เป็น sequence

async seq query เป็น build-in computation  มันไม่ใช่ keyword จึงใช้ประกาศ value ได้

```fsharp
let seq = 1;
let async = 2;
let query = 3;
```

คา

```fsharp
type RoundComputationBuilder(digits: int)  =
    let round(x: decimal) = System.Math.Round(x, digits)
    member this.Bind(result, restComputation) =
        restComputation (round result)
    member this.Return x = round x
```
s s

```fsharp
let bankInterest = RoundComputationBuilder 2
bankInterest {
    let! x = 23231.34m * 0.002m
    return x
} |> printfn "%A"
```

| Member | Description        |
| ------------- |-------------|
|member Bind : M<'a> * ('a -> M<'b>) -> M<'b>       |Required member. Used to de-sugar let! and do! within computation expressions.|
|member Return : 'a -> M<'a>	                    |Required member. Used to de-sugar return within computation expressions.|
|member Delay : (unit -> M<'a>) -> M<'a>	        |Required member. Used to ensure side effects within a computation expression are performed when expected.|
|member Yield : 'a -> M<'a>	                   | Optional member. Used to de-sugar yield within computation expressions.|
|member For : seq<'a> * ('a -> M<'b>) -> M<'b>	    |Optional member. Used to de-sugar for ... do ... within computation expressions. M<'b> can optionally be M<unit>|
|member While : (unit -> bool) * M<'a> -> M<'a>	    |Optional member. Used to de-sugar while ... do ... within computation expressions. M<'b> can optionally be M<unit>|
|member Using : 'a * ('a -> M<'b>) -> M<'b> when 'a :> IDisposable	|Optional member. Used to de-sugar use bindings within computation expressions.|
|member Combine : M<'a> -> M<'a> -> M<'a>        |Optional member. Used to de-sugar sequencing within computation expressions. The first M<'a> can optionally be M<unit>|
|member Zero : unit -> M<'a>	                    |Optional member. Used to de-sugar empty else branches of if/then within computation expressions.|
|member TryWith : M<'a> -> M<'a> -> M<'a>	        |Optional member. Used to de-sugar empty try/with bindings within computation expressions.|
|member TryFinally : M<'a> -> M<'a> -> M<'a>	    |Optional member. Used to de-sugar try/finally bindings within computation expressions.|

| Member | Description        |
| ------------- |-------------|
|let pat = expr in cexpr          |let pat = expr in cexpr|
|let! pat = expr in cexpr	        |b.Bind(expr, (fun pat -> cexpr))|
|return expr	                     |b.Return(expr)|
|return! expr	                    |b.ReturnFrom(expr)|
|yield expr	                      |b.Yield(expr)|
|yield! expr	                     |b.YieldFrom(expr)|
|use pat = expr in cexpr	         |b.Using(expr, (fun pat -> cexpr))|
|use! pat = expr in cexpr	        |b.Bind(expr, (fun x -> b.Using(x, fun pat -> cexpr))|
|do! expr in cexpr	           |b.Bind(expr, (fun () -> cexpr))|
|for pat in expr do cexpr	        |b.For(expr, (fun pat -> cexpr))|
|while expr do cexpr	             |b.While((fun () -> expr), b.Delay( fun () -> cexpr))|
|if expr then cexpr1 else cexpr2	|if expr then cexpr1 else cexpr2|
|if expr then cexpr	if expr then cexpr else    | b.Zero()|
|cexpr1 cexpr2	                    |b.Combine(cexpr1, b.Delay(fun () -> cexpr2))|
|try cexpr with patn -> cexprn	    |b.TryWith(expr, fun v -> match v with (patn:ext) -> cexprn | _ raise exn)|
|try cexpr finally expr	              |b.TryFinally(cexpr, (fun () -> expr))|

```bash
46.46M
```
-k

```fsharp
type MaybeBuilder() =
    member this.Bind(x, f) =
        match x with
        | Some(x) when x >= 0 && x <= 100 -> f(x)
        | _ -> None
    member this.Delay(f) = f()
    member this.Return(x) = Some x
```