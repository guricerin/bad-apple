# bad-apple

Bad Apple!! PV 影絵 on terminal

## 必要なツールなど
* [.NET Core 3.1](https://dotnet.microsoft.com/download)
* nicozon
* ffmpeg

## 元ネタ様
https://github.com/tem6/badapple

## 実行手順

* 準備

```posh
> git clone https://github.com/guricerin/bad-apple.git
> cd bad-apple
> dotnet tool restore
> dotnet paket install
```

* 元動画の幅・高さが大きい場合はリサイズ

```posh
> ffmpeg \path\to\org.mp4\ --vf scale=<width you like>:-1 \path\to\resize.mp4
```

* 動画をフレームごとに連番でpng画像化

```posh
> ffmpeg -i \path\to\mp4 -vcodec png -r 60 \path\to\output\folder\%06d.png # -rオプションはfpsの指定
```

* ffmpegが出力した画像ファイルが配置されているフォルダーと出力先フォルダーを指定し、``image2text.fsx``を実行、``cui-badapple.txt``が生成される

```posh
> dotnet fsi .\src\image2text.fsx \path\to\frames-folder \path\to\output-folder
```

* ``cui-badapple.txt``のパスを指定し、``play.fsx``を実行

```posh
> dotnet fsi .\src\image2text.fsx \path\to\frames-folder
```

## 音声は？
がんばってください  
