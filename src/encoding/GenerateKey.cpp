#include <cstdint>

#include "cryptopp/osrng.h"

#define DLL_API_EXPORTS
#include "DllApi.h"

using namespace CryptoPP;

extern "C"
{
   DLL_API void GenerateKey(unsigned char * key, unsigned keyLen)
   {
      OS_GenerateRandomBlock(true, key, keyLen);
   }

   DLL_API void GenerateRandomBlock(unsigned char * buf, unsigned bufLen)
   {
       OS_GenerateRandomBlock(true, buf, bufLen);
   }
}