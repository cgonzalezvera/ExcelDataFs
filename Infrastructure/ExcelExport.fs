module Infrastructure.ExcelExport

open ClosedXML.Excel
open Domain.Config

let exportToExcel (config: ExcelConfig) (data: obj array array) =
    let outputPath = resolveOutputPath config

    use workbook = new XLWorkbook()
    let worksheet = workbook.Worksheets.Add(config.SheetName)

    worksheet.Cell(1, 1).InsertData([| config.Headers |]) |> ignore

    worksheet.Cell(2, 1).InsertData(data) |> ignore

    worksheet.Columns().AdjustToContents() |> ignore

    workbook.SaveAs(outputPath)
    printfn "Archivo Excel generado: %s" outputPath
    outputPath
