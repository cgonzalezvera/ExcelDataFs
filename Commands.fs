module Commands

open System
open System.IO
open Configuration
open DataGeneration
open ExcelExport
open History

let execute (config: AppConfig) =
    let random = Random()
    let data = generateData config.DataGeneration random config.Excel.NumRows
    let outputPath = exportToExcel config.Excel data

    let entry =
        { FileName = Path.GetFileName(outputPath)
          Directory = Path.GetDirectoryName(outputPath)
          GeneratedAt = DateTime.Now }

    addEntry entry
    0
