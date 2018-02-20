#include <cstdint>

#include "cryptopp/osrng.h"

using namespace CryptoPP;

extern "C"
{
   __declspec(dllexport) void GenerateKey(unsigned char * key, unsigned keyLen)
   {
      OS_GenerateRandomBlock(true, key, keyLen);
   }

   __declspec(dllexport) void GenerateRandomBlock(unsigned char * buf, unsigned bufLen)
   {
       OS_GenerateRandomBlock(true, buf, bufLen);
   }
}