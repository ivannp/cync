CMAKE_MINIMUM_REQUIRED(VERSION 3.5)
PROJECT(encoding)
SET(CMAKE_CXX_STANDARD 11)
ADD_LIBRARY(
    encoding
    SHARED
    ../encoding/ComputeHash.cpp
    ../encoding/ComputeHashParams.pb.cc
    ../encoding/EncodeFile.cpp
    ../encoding/EncodingConfig.pb.cc
    ../encoding/EncodingConfig.pb.h
    ../encoding/FileHeader.pb.cc
    ../encoding/FileHeader.pb.h
    ../encoding/GenerateKey.cpp
    ../encoding/DllApi.h)
TARGET_LINK_LIBRARIES(encoding libprotobuf.so)
TARGET_LINK_LIBRARIES(encoding libcryptopp.so)