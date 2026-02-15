module DataGeneration

open System
open Configuration

let randomInt (random: Random) min max =
    random.Next(min, max + 1)

let randomDecimal (random: Random) min max =
    min + (random.NextDouble() * (max - min))

let generateRow (config: DataGenerationConfig) (random: Random) rowIndex =
    [|
        yield box rowIndex
        yield box (randomInt random config.IntMin config.IntMax)
        yield box (Math.Round(randomDecimal random config.DecimalMin config.DecimalMax, 2))
        for _ in 1 .. config.GuidColumns do
            yield box ((Guid.NewGuid()).ToString().Substring(0, 8))
    |]

let generateData (config: DataGenerationConfig) (random: Random) numRows =
    [| 1 .. numRows |]
    |> Array.map (generateRow config random)
