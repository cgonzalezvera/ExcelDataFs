module History

open System
open System.IO

type HistoryEntry =
    { FileName: string
      Directory: string
      GeneratedAt: DateTime }

let dataFilePath =
    Path.Combine(AppContext.BaseDirectory, "data.txt")

let private parseEntry (line: string) =
    let parts = line.Split('|')
    if parts.Length = 3 then
        match DateTime.TryParseExact(parts.[2], "dd-MM-yyyy HH:mm", null, Globalization.DateTimeStyles.None) with
        | true, dt ->
            Some { FileName = parts.[0]; Directory = parts.[1]; GeneratedAt = dt }
        | _ -> None
    else
        None

let private formatEntry (entry: HistoryEntry) =
    sprintf "%s|%s|%s" entry.FileName entry.Directory (entry.GeneratedAt.ToString("dd-MM-yyyy HH:mm"))

let loadHistory () =
    if File.Exists(dataFilePath) then
        File.ReadAllLines(dataFilePath)
        |> Array.choose parseEntry
        |> Array.toList
    else
        []

let saveHistory (entries: HistoryEntry list) =
    let lines =
        entries
        |> List.truncate 10
        |> List.map formatEntry
        |> List.toArray
    File.WriteAllLines(dataFilePath, lines)

let addEntry (entry: HistoryEntry) =
    let history = loadHistory ()
    let updated = entry :: history
    saveHistory updated

let getLastEntry () =
    loadHistory () |> List.tryHead
