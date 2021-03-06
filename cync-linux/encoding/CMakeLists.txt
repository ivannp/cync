CMAKE_MINIMUM_REQUIRED(VERSION 3.5)
PROJECT(encoding)
SET(CMAKE_CXX_STANDARD 11)
ADD_LIBRARY(
    encoding
    SHARED
    ../../src/encoding/ComputeHash.cpp
    ../../src/encoding/ComputeHashParams.pb.cc
    ../../src/encoding/EncodeFile.cpp
    ../../src/encoding/EncodingConfig.pb.cc
    ../../src/encoding/EncodingConfig.pb.h
    ../../src/encoding/FileHeader.pb.cc
    ../../src/encoding/FileHeader.pb.h
    ../../src/encoding/GenerateKey.cpp
    ../../src/encoding/DllApi.h)
TARGET_LINK_LIBRARIES(encoding libprotobuf.so)
TARGET_LINK_LIBRARIES(encoding libcryptopp.so)

SET(CYNC_PATH ${CMAKE_CURRENT_BINARY_DIR}/../cync)
ADD_CUSTOM_COMMAND(
    TARGET encoding POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
        ${CMAKE_CURRENT_BINARY_DIR}/libencoding.so
        ${CYNC_PATH}/encoding.so
    COMMENT "Copying libencoding.so to '${CYNC_PATH}'")