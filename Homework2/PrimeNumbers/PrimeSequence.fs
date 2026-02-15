module PrimeSequence

let isPrime n =
    if n < 2 then false
    elif n = 2 then true
    elif n % 2 = 0 then false
    else
        let border = int (sqrt (float n))
        let rec check m =
            if m > border then true
            elif n % m = 0 then false
            else
                check (m + 2)
        check 3

let rec createPrime =
    seq {
        yield 2
        let mutable number = 3
        while true do
            if isPrime number then 
                yield number
            number <- number + 2
    }