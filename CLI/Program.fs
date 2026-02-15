open System.IO
open Infrastructure.ConfigLoader

[<EntryPoint>]
let main argv =
    let configPath =
        Path.Combine(System.AppContext.BaseDirectory, "appsettings.json")

    match argv |> Array.toList with
    | ["--generate"] ->
        let config = loadConfig configPath
        Application.Commands.GenerateCommand.execute config
    | ["--last"] ->
        Application.Queries.LastQuery.execute ()
    | _ ->
        printfn "Uso: ExcelDataFs <comando>"
        printfn ""
        printfn "Comandos:"
        printfn "  --generate  Genera un archivo Excel con datos aleatorios"
        printfn "  --last      Muestra la ubicación del último archivo generado"
        1
