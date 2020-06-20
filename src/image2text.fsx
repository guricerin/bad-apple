#r "../packages/SixLabors.ImageSharp/lib/netstandard2.0/Sixlabors.ImageSharp.dll"

open System
open System.IO
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing

let threshold = float32 127 / float32 255

let isBlackPixel (pixel: Rgba32) =
    pixel.R = 0uy && pixel.G = 0uy && pixel.B = 0uy

let img2Txt (frame: Image<Rgba32>) =
    let res = Text.StringBuilder()
    frame.Mutate(fun x ->
        x.Resize(160, 80).BinaryThreshold(threshold, Color.White, Color.Black)
        |> ignore)
    // Span is not able to deal in computation expression
    // https://qiita.com/cannorin/items/59d79cc9a3b64c761cd4#byref-like-structs
    for y in 0 .. frame.Height - 1 do
        let row = frame.GetPixelRowSpan(y)
        for x in 0 .. row.Length - 1 do
            if isBlackPixel row.[x] then res.Append(" ") |> ignore else res.Append("#") |> ignore
        res.Append("\n") |> ignore
    res.Append("R\n").ToString()

let imageDir, outputDir =
    let args = Environment.GetCommandLineArgs()
    match Array.length args with
    | 4 -> args.[2], args.[3]
    | _ ->
        printfn "  Usage: dotnet fsi .\\src\\image2text.fsx <path-to-images-folder> <path-to-output-folder>"
        exit 1

let printProgress cnt total =
    let msg = sprintf "Converting... %d/ %d" cnt total
    Console.Write msg
    Console.SetCursorPosition(0, Console.CursorTop)

let main () =
    let imageFiles =
        Directory.GetFiles(imageDir, "*", SearchOption.TopDirectoryOnly)

    if Directory.Exists outputDir |> not
    then Directory.CreateDirectory(outputDir) |> ignore

    let outputPath =
        Path.Combine(outputDir, "cui-badapple.txt")

    let mutable cnt = 1

    use writer =
        new StreamWriter(outputPath, false, Text.Encoding.Default)

    Console.CursorVisible <- false
    for file in imageFiles do
        use frame: Image<Rgba32> = Image.Load(file)
        frame |> img2Txt |> writer.Write

        printProgress cnt imageFiles.Length
        cnt <- cnt + 1
    Console.CursorVisible <- true

main ()
