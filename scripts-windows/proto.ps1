$location = Get-Location

$soluctionDir = Split-Path $PSScriptRoot

$path = Join-Path $soluctionDir "\encoding"
Set-Location $path

$protoPath = Join-Path "$env:PROTOC_DIR" "protoc.exe"

Start-Process -FilePath $protoPath -NoNewWindow -Wait " --cpp_out=. --proto_path ..\proto ..\proto\ComputeHashParams.proto"
Start-Process -FilePath $protoPath -NoNewWindow -Wait " --cpp_out=. --proto_path ..\proto ..\proto\EncodingConfig.proto"
Start-Process -FilePath $protoPath -NoNewWindow -Wait " --cpp_out=. --proto_path ..\proto ..\proto\FileHeader.proto"

$path = Join-Path $soluctionDir "\cyncore"
Set-Location $path

Start-Process -FilePath $protoPath -NoNewWindow -Wait " --csharp_out=. --proto_path ..\proto ..\proto\BaseDir.proto"
Start-Process -FilePath $protoPath -NoNewWindow -Wait " --csharp_out=. --proto_path ..\proto ..\proto\ComputeHashParams.proto"
Start-Process -FilePath $protoPath -NoNewWindow -Wait " --csharp_out=. --proto_path ..\proto ..\proto\EncodingConfig.proto"
Start-Process -FilePath $protoPath -NoNewWindow -Wait " --csharp_out=. --proto_path ..\proto ..\proto\FileHeader.proto"

Set-Location $location