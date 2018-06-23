#include <cstdint>
#include <fstream>

#include <cryptopp/pwdbased.h>
#include "cryptopp/files.h"
#include "cryptopp/filters.h"
#include "cryptopp/sha.h"

#define DLL_API_EXPORTS
#include "DllApi.h"

#include "ComputeDataHashParams.pb.h"
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

	DLL_API void ComputeDataHash(const char * buf, unsigned bufLen, char * bufOut, unsigned bufOutLen)
	{
		SecByteBlock derived(bufOutLen);

		// Deserialize the API blob
		sotcore::ComputeDataHashParams params;
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

		// KDF function
		PKCS5_PBKDF2_HMAC<SHA256> kdf;
		kdf.DeriveKey((byte *)bufOut, bufOutLen, 0, (byte *)buf, bufLen, NULL, 0, params.iterations());
	}
}