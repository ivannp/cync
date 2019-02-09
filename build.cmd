set root=c:\ttt\projects

cd %root%
git clone https://github.com/Microsoft/vcpkg.git vcpkg
cd vcpkg
git reset --hard 61e2cac730cc8b16c9373c6c463a335a21a1496a
call bootstrap-vcpkg.bat
.\vcpkg integrate install
.\vcpkg install protobuf:x64-windows-static
.\vcpkg install cryptopp:x64-windows-static

cd %root%
git clone https://github.com/ivannp/cync.git cync

cd %root%/cync/src/encoding
%root%/vcpkg/installed\x64-windows-static\tools\protobuf\protoc.exe --cpp_out=. --proto_path ..\..\proto ..\..\proto\ComputeHashParams.proto
%root%/vcpkg/installed\x64-windows-static\tools\protobuf\protoc.exe --cpp_out=. --proto_path ..\..\proto ..\..\proto\ComputeDataHashParams.proto
%root%/vcpkg/installed\x64-windows-static\tools\protobuf\protoc.exe --cpp_out=. --proto_path ..\..\proto ..\..\proto\EncodingConfig.proto
%root%/vcpkg/installed\x64-windows-static\tools\protobuf\protoc.exe --cpp_out=. --proto_path ..\..\proto ..\..\proto\FileHeader.proto
cd %root%/cync/src/cyncore
%root%/vcpkg/installed\x64-windows-static\tools\protobuf\protoc.exe --csharp_out=. --proto_path ..\..\proto ..\..\proto\BaseDir.proto
%root%/vcpkg/installed\x64-windows-static\tools\protobuf\protoc.exe --csharp_out=. --proto_path ..\..\proto ..\..\proto\ComputeHashParams.proto
%root%/vcpkg/installed\x64-windows-static\tools\protobuf\protoc.exe --csharp_out=. --proto_path ..\..\proto ..\..\proto\ComputeDataHashParams.proto
%root%/vcpkg/installed\x64-windows-static\tools\protobuf\protoc.exe --csharp_out=. --proto_path ..\..\proto ..\..\proto\EncodingConfig.proto
%root%/vcpkg/installed\x64-windows-static\tools\protobuf\protoc.exe --csharp_out=. --proto_path ..\..\proto ..\..\proto\FileHeader.proto
cd %root%\cync

nuget restore cync-windows\cync-windows.sln
msbuild cync-windows\cync-windows.sln