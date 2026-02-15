# ExcelDataFs

Herramienta CLI escrita en F# que genera archivos Excel (.xlsx) con datos aleatorios. Mantiene un historial de los últimos 10 archivos generados para consulta rápida.

## Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Instalación

```bash
git clone https://github.com/cgonzalezvera/ExcelDataFs.git
cd ExcelDataFs
dotnet build
```

## Uso

### Generar un archivo Excel

```bash
dotnet run -- --generate
```

Genera un archivo `datos_generados.xlsx` en el escritorio (por defecto) con 100 filas de datos aleatorios: un ID secuencial, un número entero, un decimal y 7 columnas con fragmentos de GUID.

### Consultar el último archivo generado

```bash
dotnet run -- --last
```

Muestra la ubicación, nombre y fecha de generación del último archivo creado.

### Ayuda

```bash
dotnet run
```

Sin argumentos muestra los comandos disponibles.

## Configuración

El archivo `appsettings.json` permite personalizar:

| Sección          | Propiedad        | Descripción                          | Valor por defecto        |
|------------------|------------------|--------------------------------------|--------------------------|
| `Excel`          | `FileName`       | Nombre del archivo generado          | `datos_generados.xlsx`   |
| `Excel`          | `OutputDirectory` | Directorio de salida (vacío = Escritorio) | `""`                |
| `Excel`          | `SheetName`      | Nombre de la hoja                    | `Datos`                  |
| `Excel`          | `NumRows`        | Cantidad de filas a generar          | `100`                    |
| `Excel`          | `Headers`        | Encabezados de columnas              | `ID, Entero, Decimal...` |
| `DataGeneration` | `IntMin` / `IntMax` | Rango para enteros aleatorios     | `1` - `100`              |
| `DataGeneration` | `DecimalMin` / `DecimalMax` | Rango para decimales aleatorios | `0.0` - `100.0`    |
| `DataGeneration` | `GuidColumns`    | Cantidad de columnas GUID            | `7`                      |

## Licencia

MIT
