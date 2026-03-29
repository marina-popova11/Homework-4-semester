module LazyLib

type ILazy<'a> =
    abstract member Get: unit -> 'a