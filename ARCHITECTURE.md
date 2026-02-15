# Arquitectura

ExcelDataFs sigue una arquitectura **Clean Architecture** con separación **CQRS** (Command Query Responsibility Segregation), organizada en cuatro capas con carpetas dedicadas.

## Capas

### Domain (tipos puros, sin IO)

Contiene los tipos del dominio sin dependencias de IO ni librerías externas.

- **`Domain/Config.fs`** — Tipos `ExcelConfig`, `DataGenerationConfig`, `AppConfig`, valores por defecto y `resolveOutputPath`
- **`Domain/HistoryEntry.fs`** — Record `HistoryEntry` con `FileName`, `Directory` y `GeneratedAt`

### Infrastructure (IO y librerías externas)

Implementaciones concretas que interactúan con el sistema de archivos y librerías.

- **`Infrastructure/ConfigLoader.fs`** — Carga y parsea `appsettings.json` con valores por defecto para propiedades faltantes
- **`Infrastructure/DataGeneration.fs`** — Genera filas de datos aleatorios como `obj array array`
- **`Infrastructure/ExcelExport.fs`** — Crea archivos `.xlsx` con ClosedXML
- **`Infrastructure/HistoryRepository.fs`** — Lectura/escritura del historial en `data.txt`

### Application (casos de uso)

Orquesta la lógica de negocio combinando Domain e Infrastructure.

- **`Application/Commands/GenerateCommand.fs`** — Comando: genera datos, exporta a Excel y registra en historial
- **`Application/Queries/LastQuery.fs`** — Query: consulta el último archivo del historial

### CLI (presentación)

Punto de entrada de la aplicación.

- **`CLI/Program.fs`** — Parsea `argv` y despacha al comando o query correspondiente

## Estructura de carpetas

```
ExcelDataFs/
├── Domain/
│   ├── Config.fs
│   └── HistoryEntry.fs
├── Application/
│   ├── Commands/
│   │   └── GenerateCommand.fs
│   └── Queries/
│       └── LastQuery.fs
├── Infrastructure/
│   ├── ConfigLoader.fs
│   ├── DataGeneration.fs
│   ├── ExcelExport.fs
│   └── HistoryRepository.fs
└── CLI/
    └── Program.fs
```

## Flujo de ejecución

### `--generate` (Command)

1. `CLI/Program.fs` carga la configuración via `Infrastructure.ConfigLoader`
2. `Application.Commands.GenerateCommand.execute` orquesta el proceso:
   - `Infrastructure.DataGeneration.generateData` crea un array de filas con datos aleatorios
   - `Infrastructure.ExcelExport.exportToExcel` escribe el archivo .xlsx y retorna la ruta generada
   - `Infrastructure.HistoryRepository.addEntry` registra el archivo en `data.txt`

### `--last` (Query)

1. `Application.Queries.LastQuery.execute` lee el historial con `Infrastructure.HistoryRepository.getLastEntry`
2. Si existe una entrada, muestra nombre, directorio, ruta completa y fecha
3. Si no hay historial, muestra un mensaje informativo

## Orden de compilación

F# requiere que los archivos se declaren en orden de dependencia en el `.fsproj`:

```
Domain/Config.fs → Domain/HistoryEntry.fs → Infrastructure/ConfigLoader.fs → Infrastructure/DataGeneration.fs → Infrastructure/ExcelExport.fs → Infrastructure/HistoryRepository.fs → Application/Commands/GenerateCommand.fs → Application/Queries/LastQuery.fs → CLI/Program.fs
```

Cada módulo solo puede referenciar módulos declarados antes que él.

## Principios

- **Domain**: tipos puros sin dependencias de IO ni librerías externas
- **Application**: casos de uso (commands/queries) que orquestan la lógica
- **Infrastructure**: implementaciones concretas de IO (archivos, Excel, JSON)
- **CLI**: capa de presentación, punto de entrada

## Dependencias externas

| Paquete    | Versión | Uso                        |
|------------|---------|----------------------------|
| ClosedXML  | 0.105.0 | Generación de archivos Excel |
