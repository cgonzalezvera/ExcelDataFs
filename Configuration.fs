module Configuration

open System
open System.IO
open System.Text.Json

type ExcelConfig =
    { FileName: string
      OutputDirectory: string
      SheetName: string
      NumRows: int
      Headers: string array }

type DataGenerationConfig =
    { IntMin: int
      IntMax: int
      DecimalMin: float
      DecimalMax: float
      GuidColumns: int }

type AppConfig =
    { Excel: ExcelConfig
      DataGeneration: DataGenerationConfig }

let defaultExcelConfig =
    { FileName = "datos_generados.xlsx"
      OutputDirectory = ""
      SheetName = "Datos"
      NumRows = 100
      Headers = [| "ID"; "Entero"; "Decimal"; "Col4"; "Col5"; "Col6"; "Col7"; "Col8"; "Col9"; "Col10" |] }

let defaultDataGenerationConfig =
    { IntMin = 1
      IntMax = 100
      DecimalMin = 0.0
      DecimalMax = 100.0
      GuidColumns = 7 }

let defaultConfig =
    { Excel = defaultExcelConfig
      DataGeneration = defaultDataGenerationConfig }

let resolveOutputPath (config: ExcelConfig) =
    let dir =
        if String.IsNullOrWhiteSpace(config.OutputDirectory) then
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        else
            config.OutputDirectory

    Path.Combine(dir, config.FileName)

let loadConfig (path: string) =
    if not (File.Exists(path)) then
        defaultConfig
    else
        let json = File.ReadAllText(path)
        let options = JsonSerializerOptions(PropertyNameCaseInsensitive = true)
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
