#include <cstdint>
#include <fstream>

#include "cryptopp/files.h"
#include "cryptopp/filters.h"
#include "cryptopp/sha.h"

#define DLL_API_EXPORTS
#include "DllApi.h"

#include "ComputeHashParams.pb.h"

using namespace std;
using namespace CryptoPP;

extern "C"
{
    DLL_API void ComputeHash(const char * buf, unsigned bufLen, char * bufOut, unsigned bufOutLen)
    {
        // Deserialize the API blob
        sotcore::ComputeHashParams params;
        params.ParseFromArray(buf, bufLen);

        ArraySink * sink = new ArraySink((byte *)bufOut, bufOutLen);
        HashFilter * hashFilter = nullptr;

        if (params.hash() == "sha256")
        {
            SHA256 sha256;
            hashFilter = new HashFilter(sha256, sink);
        }
        else if (params.hash() == "sha512")
        {
            SHA512 sha512;
            hashFilter = new HashFilter(sha512, sink);
        }

        FileSource ss(params.path().c_str(), true, hashFilter);
    }
}