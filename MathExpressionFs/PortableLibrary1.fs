namespace MathExpressionFs
open System
type Token = 
| Number of double
| Symbol of string
| LB
| RB
| EOF

type Tokenizer(expression:string) =
    member this.Expression = expression
    member this.Tokenize() =
        let number exptail = 
            let rec numberMatcher num exptail = 
                match exptail with
                | c::tail when Char.IsDigit(c) -> numberMatcher (c::num) tail
                | _ -> (exptail, num |> List.rev |> Array.ofList |> String |> Double.Parse |> Token.Number)
            numberMatcher [] exptail
        let symbol exptail = 
            let rec symbolMatcher sym exptail = 
                match exptail with
                | c::tail when Char.IsLetterOrDigit(c) -> symbolMatcher (c::sym) tail
                | _ -> (exptail, sym |> List.rev |> Array.ofList |> String |> Token.Symbol)
            symbolMatcher [] exptail

        let mutable exp = List.ofSeq <| expression.ToCharArray()
        let rec matcher exptail = 
            match exptail with
            | c::tail when Char.IsLetter(c) -> symbol exptail
            | c::tail when Char.IsDigit(c) -> number exptail
            | '('::tail -> (tail, Token.LB)
            | ')'::tail -> (tail, Token.RB)
            | ' '::tail -> matcher tail
            | _ -> ([], Token.EOF)
        seq{
            while not exp.IsEmpty do
                let (ex, token) = matcher exp
                exp <- ex
                yield token
        }