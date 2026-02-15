module Infrastructure.ConfigLoader

open System.IO
open System.Text.Json
open Domain.Config

let loadConfig (path: string) =
    if not (File.Exists(path)) then
        defaultConfig
    else
        let json = File.ReadAllText(path)
        let doc = JsonDocument.Parse(json)
        let root = doc.RootElement

        let excelConfig =
            match root.TryGetProperty("Excel") with
            | true, el ->
                { FileName =
                    match el.TryGetProperty("FileName") with
                    | true, v -> v.GetString()
                    | _ -> defaultExcelConfig.FileName
                  OutputDirectory =
                    match el.TryGetProperty("OutputDirectory") with
                    | true, v -> v.GetString()
                    | _ -> defaultExcelConfig.OutputDirectory
                  SheetName =
                    match el.TryGetProperty("SheetName") with
                    | true, v -> v.GetString()
                    | _ -> defaultExcelConfig.SheetName
                  NumRows =
                    match el.TryGetProperty("NumRows") with
                    | true, v -> v.GetInt32()
                    | _ -> defaultExcelConfig.NumRows
                  Headers =
                    match el.TryGetProperty("Headers") with
                    | true, v ->
                        [| for i in 0 .. v.GetArrayLength() - 1 -> v.[i].GetString() |]
                    | _ -> defaultExcelConfig.Headers }
            | _ -> defaultExcelConfig

        let dataGenConfig =
            match root.TryGetProperty("DataGeneration") with
            | true, el ->
                { IntMin =
                    match el.TryGetProperty("IntMin") with
                    | true, v -> v.GetInt32()
                    | _ -> defaultDataGenerationConfig.IntMin
                  IntMax =
                    match el.TryGetProperty("IntMax") with
                    | true, v -> v.GetInt32()
                    | _ -> defaultDataGenerationConfig.IntMax
                  DecimalMin =
                    match el.TryGetProperty("DecimalMin") with
                    | true, v -> v.GetDouble()
                    | _ -> defaultDataGenerationConfig.DecimalMin
                  DecimalMax =
                    match el.TryGetProperty("DecimalMax") with
                    | true, v -> v.GetDouble()
                    | _ -> defaultDataGenerationConfig.DecimalMax
                  GuidColumns =
                    match el.TryGetProperty("GuidColumns") with
                    | true, v -> v.GetInt32()
                    | _ -> defaultDataGenerationConfig.GuidColumns }
            | _ -> defaultDataGenerationConfig

        { Excel = excelConfig
          DataGeneration = dataGenConfig }
