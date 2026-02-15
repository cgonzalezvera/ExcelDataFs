module Queries

open System.IO
open History

let execute () =
    match getLastEntry () with
    | Some entry ->
        let fullPath = Path.Combine(entry.Directory, entry.FileName)
        printfn "Último archivo generado:"
        printfn "  Archivo:   %s" entry.FileName
        printfn "  Directorio: %s" entry.Directory
        printfn "  Ruta completa: %s" fullPath
        printfn "  Generado el: %s" (entry.GeneratedAt.ToString("dd-MM-yyyy HH:mm"))
        0
    | None ->
        printfn "No hay archivos en el historial. Usa --generate para crear uno."
        0
