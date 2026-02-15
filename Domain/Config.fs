module Domain.Config

open System
open System.IO

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
