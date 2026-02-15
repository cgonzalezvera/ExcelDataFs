# Arquitectura

ExcelDataFs sigue una arquitectura **CQRS** (Command Query Responsibility Segregation), separando las operaciones que modifican estado (generación de archivos) de las que solo consultan datos (historial).

## Estructura de módulos

```
Program.fs          -- Punto de entrada: parsea argv y despacha al comando o query
├── Commands.fs     -- Comando: orquesta generación y registro en historial
│   ├── DataGeneration.fs  -- Genera filas de datos aleatorios
│   ├── ExcelExport.fs     -- Exporta datos a .xlsx usando ClosedXML
│   └── History.fs         -- Persiste el historial en data.txt
├── Queries.fs      -- Query: consulta el último archivo del historial
│   └── History.fs         -- Lee el historial desde data.txt
└── Configuration.fs -- Carga y valida appsettings.json
```

## Flujo de ejecución

### `--generate` (Command)

1. `Program.fs` carga la configuración desde `appsettings.json`
2. `Commands.execute` orquesta el proceso:
   - `DataGeneration.generateData` crea un array de filas con datos aleatorios
   - `ExcelExport.exportToExcel` escribe el archivo .xlsx y retorna la ruta generada
   - `History.addEntry` registra el archivo en `data.txt`

### `--last` (Query)

1. `Queries.execute` lee el historial con `History.getLastEntry`
2. Si existe una entrada, muestra nombre, directorio, ruta completa y fecha
3. Si no hay historial, muestra un mensaje informativo

## Módulos en detalle

### Configuration.fs

Define los tipos `ExcelConfig`, `DataGenerationConfig` y `AppConfig`. Carga `appsettings.json` con valores por defecto para cada propiedad faltante. Resuelve la ruta de salida: si `OutputDirectory` está vacío, usa el escritorio del usuario.

### DataGeneration.fs

Genera filas de datos como `obj array array`. Cada fila contiene:
- Un índice secuencial (ID)
- Un entero aleatorio dentro del rango configurado
- Un decimal aleatorio redondeado a 2 decimales
- N columnas con los primeros 8 caracteres de GUIDs aleatorios

### ExcelExport.fs

Usa [ClosedXML](https://github.com/ClosedXML/ClosedXML) para crear el workbook. Inserta encabezados en la primera fila, datos a partir de la segunda, ajusta el ancho de columnas y guarda el archivo. Retorna la ruta del archivo generado como `string`.

### History.fs

Gestiona el historial persistente en `data.txt`, ubicado junto al ejecutable (`AppContext.BaseDirectory`).

- **Formato**: una línea por entrada, campos separados por `|`
  ```
  datos_generados.xlsx|C:\Users\user\Desktop|14-02-2026 15:30
  ```
- **Tipo**: `HistoryEntry` con campos `FileName`, `Directory` y `GeneratedAt`
- **Límite**: mantiene como máximo 10 entradas (las más recientes)
- **Funciones**: `loadHistory`, `saveHistory`, `addEntry`, `getLastEntry`

### Commands.fs

Módulo de escritura (Command). Genera datos, exporta a Excel y registra la entrada en el historial. Retorna exit code `0`.

### Queries.fs

Módulo de lectura (Query). Consulta el último archivo del historial y lo muestra en consola. No modifica estado. Retorna exit code `0`.

### Program.fs

Punto de entrada. Parsea `argv` con pattern matching sobre la lista de argumentos:
- `["--generate"]` → `Commands.execute`
- `["--last"]` → `Queries.execute`
- Cualquier otro caso → muestra uso y retorna exit code `1`

## Orden de compilación

F# requiere que los archivos se declaren en orden de dependencia en el `.fsproj`:

```
Configuration.fs → DataGeneration.fs → ExcelExport.fs → History.fs → Commands.fs → Queries.fs → Program.fs
```

Cada módulo solo puede referenciar módulos declarados antes que él.

## Dependencias externas

| Paquete    | Versión | Uso                        |
|------------|---------|----------------------------|
| ClosedXML  | 0.105.0 | Generación de archivos Excel |
