open System.IO
open Configuration

[<EntryPoint>]
let main argv =
    let configPath =
        Path.Combine(System.AppContext.BaseDirectory, "appsettings.json")

    match argv |> Array.toList with
    | ["--generate"] ->
        let config = loadConfig configPath
        Commands.execute config
    | ["--last"] ->
        Queries.execute ()
    | _ ->
        printfn "Uso: ExcelDataFs <comando>"
        printfn ""
        printfn "Comandos:"
        printfn "  --generate  Genera un archivo Excel con datos aleatorios"
        printfn "  --last      Muestra la ubicación del último archivo generado"
        1
