module Domain.HistoryEntry

open System

type HistoryEntry =
    { FileName: string
      Directory: string
      GeneratedAt: DateTime }
