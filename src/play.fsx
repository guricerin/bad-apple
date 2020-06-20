open System
open System.IO
open System.Diagnostics

let srcPath, fps =
    let args = Environment.GetCommandLineArgs()
    match Array.length args with
    | 4 -> args.[2], int args.[3]
    | _ ->
        printfn "  Usage: dotnet fsi .\\src\\play.fsx <binarized-txt-file> <fps>"
        exit 1

let play () =
    use reader = new StreamReader(srcPath)

    use writer =
        new StreamWriter(new BufferedStream(Console.OpenStandardOutput()))

    let mutable framecnt = 0
    let start = DateTime.Now

    let wait nextFrame =
        let rec go () =
            let now = DateTime.Now
            let delta = (now - start).TotalMilliseconds
            if float nextFrame / float fps * 1000.0 <= delta
            then ()
            else go ()

        go ()

    let write () =
        let line = reader.ReadLine()
        match line.[0] with
        | 'R' ->
            writer.Flush()
            Console.SetCursorPosition(0, 0)
            wait framecnt
            framecnt <- framecnt + 1
        | _ -> writer.WriteLine(line)

    let rec loop () =
        match reader.Peek() > -1 with
        | true ->
            write ()
            loop ()
        | _ -> ()

    loop ()

let main () =
    Console.Clear()
    Console.CursorVisible <- false
    play ()
    Console.Clear()
    Console.CursorVisible <- true

main ()
