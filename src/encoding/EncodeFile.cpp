#include <codecvt>
#include <cstdint>
#include <fstream>

#include "cryptopp/aes.h"
#include "cryptopp/blowfish.h"
#include "cryptopp/channels.h"
#include "cryptopp/cryptlib.h"
#include "cryptopp/files.h"
#include "cryptopp/filters.h"
#include "cryptopp/gcm.h"
#include "cryptopp/osrng.h"
#include "cryptopp/serpent.h"
#include "cryptopp/twofish.h"
#include "cryptopp/zdeflate.h"
#include "cryptopp/zinflate.h"

#include "EncodingConfig.pb.h"
#include "FileHeader.pb.h"

using namespace std;
using namespace CryptoPP;
using namespace sotcore;

template <typename T>
void Destructor(void * pp)
{
    delete (T *)pp;
}

class DestructorList
{
public:

    DestructorList()
    {}

    ~DestructorList()
    {
        for (const auto & pp : mDestructors)
        {
            pp.first(pp.second);
        }
    }

    void Add(const function<void(void *)> & func, void * pp)
    {
        mDestructors.emplace_back(func, pp);
    }

protected:
    vector<pair<function<void(void *)>, void *>> mDestructors;
};

static sotcore::Cipher CipherFromString(const std::string & str)
{
    if (str == "aes")
    {
        return sotcore::Cipher::AES;
    }
    else if (str == "twofish")
    {
        return sotcore::Cipher::TWOFISH;
    }
    else if (str == "serpent")
    {
        return sotcore::Cipher::SERPENT;
    }

    return sotcore::Cipher::UNDEFINED;
}

static void OpenInputFileStream(const std::string & path, ios_base::openmode mode, ifstream & result)
{
#ifdef _MSC_VER
	wstring_convert<codecvt_utf8_utf16<wchar_t>, wchar_t> convert;
	auto u16path = convert.from_bytes(path);
	result.open((wchar_t *)&u16path[0], mode);
#else // _MSC_VER
	result.open(path, mode);
#endif // _MSC_VER
}

static void OpenOutputFileStream(const std::string & path, ios_base::openmode mode, ofstream & result)
{
#ifdef _MSC_VER
	wstring_convert<codecvt_utf8_utf16<wchar_t>, wchar_t> convert;
	auto u16path = convert.from_bytes(path);
	result.open((wchar_t *)&u16path[0], mode);
#else // _MSC_VER
	result.open(path, mode);
#endif // _MSC_VER
}

extern "C"
{
    __declspec(dllexport) void EncodeFile(const char * buf, unsigned bufLen, char * hash, unsigned hashLen)
    {
        DestructorList encryptionDestructors;

        // Deserialize the API blob
        sotcore::EncodingConfig cfg;
        cfg.ParseFromArray(buf, bufLen);

        // The file header structure
        sotcore::FileHeader fileHeader;
        fileHeader.set_compression_type(sotcore::CompressionType::ZLIB);
        fileHeader.set_compression_level(cfg.compression_level());
        for (int ii = 0; ii < cfg.ciphers_size(); ++ii)
        {
            auto cc = CipherFromString(cfg.ciphers(ii));
            if (cc != sotcore::Cipher::UNDEFINED)
            {
                fileHeader.add_ciphers(cc);
            }
        }

        // We use an output stream so that we can write the header first.
        ofstream ofs(cfg.dest().c_str(), ios::binary | ios::trunc);
        FileSink * fileSink = new FileSink(ofs);

        BufferedTransformation * lastTransform = fileSink;

        bool generateIvs = true;

        // Build the encryption chain
        size_t keyOffset = 0;
        for (int ii = 0; ii < fileHeader.ciphers_size(); ++ii)
        {
            StreamTransformationFilter * stf;

            if (fileHeader.ciphers(ii) == sotcore::Cipher::AES)
            {
                size_t blockSize = AES::BLOCKSIZE;
                size_t keySize = AES::MAX_KEYLENGTH;

                string * iv;
                if (generateIvs)
                {
                    iv = fileHeader.add_ivs();
                    iv->resize(blockSize);
                    OS_GenerateRandomBlock(true, (uint8_t *)&(*iv)[0], iv->size());
                }
                else
                {
                    iv = fileHeader.mutable_ivs(ii);
                }

                CTR_Mode<CryptoPP::AES>::Encryption * encryption = new CTR_Mode<CryptoPP::AES>::Encryption((uint8_t *)&cfg.key()[keyOffset], CryptoPP::AES::MAX_KEYLENGTH, (uint8_t *)iv->c_str(), iv->size());
                encryptionDestructors.Add(Destructor<CTR_Mode<CryptoPP::AES>::Encryption>, encryption);
                stf = new StreamTransformationFilter(*encryption, lastTransform);
                lastTransform = stf;

                keyOffset += keySize;
            }
            else if (fileHeader.ciphers(ii) == sotcore::Cipher::TWOFISH)
            {
                size_t blockSize = Twofish::BLOCKSIZE;
                size_t keySize = Twofish::MAX_KEYLENGTH;

                string * iv;
                if (generateIvs)
                {
                    iv = fileHeader.add_ivs();
                    iv->resize(blockSize);
                    OS_GenerateRandomBlock(true, (uint8_t *)&(*iv)[0], iv->size());
                }
                else
                {
                    iv = fileHeader.mutable_ivs(ii);
                }

                CTR_Mode<CryptoPP::Twofish>::Encryption * encryption = new CTR_Mode<CryptoPP::Twofish>::Encryption((uint8_t *)&cfg.key()[keyOffset], CryptoPP::Twofish::MAX_KEYLENGTH, (uint8_t *)iv->c_str(), iv->size());
                encryptionDestructors.Add(Destructor<CTR_Mode<Twofish>::Encryption>, encryption);
                stf = new StreamTransformationFilter(*encryption, lastTransform);
                lastTransform = stf;

                keyOffset += keySize;
            }
            else if (fileHeader.ciphers(ii) == sotcore::Cipher::SERPENT)
            {
                size_t blockSize = Serpent::BLOCKSIZE;
                size_t keySize = Serpent::MAX_KEYLENGTH;

                string * iv;
                if (generateIvs)
                {
                    iv = fileHeader.add_ivs();
                    iv->resize(blockSize);
                    OS_GenerateRandomBlock(true, (uint8_t *)&(*iv)[0], iv->size());
                }
                else
                {
                    iv = fileHeader.mutable_ivs(ii);
                }

                CTR_Mode<CryptoPP::Serpent>::Encryption * encryption = new CTR_Mode<CryptoPP::Serpent>::Encryption((uint8_t *)&cfg.key()[keyOffset], CryptoPP::Serpent::MAX_KEYLENGTH, (uint8_t *)iv->c_str(), iv->size());
                encryptionDestructors.Add(Destructor<CTR_Mode<Serpent>::Encryption>, encryption);
                stf = new StreamTransformationFilter(*encryption, lastTransform);
                lastTransform = stf;

                keyOffset += keySize;
            }
        }

        // Write the file header, its size first
        string bb;
        fileHeader.SerializeToString(&bb);
        uint32_t size = (uint32_t)bb.size();
        ofs.write((char *)&size, 4);
        ofs.write(&bb[0], bb.size());

        Deflator compression(lastTransform, cfg.compression_level());
        SHA256 sha256;
        HashFilter hf(sha256, new ArraySink((byte *)hash, hashLen));
        ChannelSwitch * cs = new ChannelSwitch(compression);
        cs->AddDefaultRoute(hf);
		ifstream ifs;
		OpenInputFileStream(cfg.src().c_str(), ios::in | ios::binary, ifs);
        FileSource ss(ifs, true, cs);
    }

    __declspec(dllexport) void DecodeFile(const char * buf, unsigned bufLen, char * hash, unsigned hashLen)
    {
        DestructorList encryptionDestructors;

        // Deserialize the API blob
        sotcore::EncodingConfig cfg;
        cfg.ParseFromArray(buf, bufLen);

        // The input stream
        ifstream ifs;
		OpenInputFileStream(cfg.src().c_str(), ios::binary | ios::in, ifs);

        // Deserialize the file header structure
        uint32_t size;
        ifs.read((char *)&size, 4);
        string bb;
        bb.resize(size);
        ifs.read(&bb[0], size);
        sotcore::FileHeader fileHeader;
        fileHeader.ParseFromString(bb);

		ofstream ofs;
		OpenOutputFileStream(cfg.dest().c_str(), ios::binary | ios::out | ios::trunc, ofs);

        FileSink fileSink(ofs);
        ArraySink shaSink((byte *)hash, hashLen);

        ChannelSwitch * cs = new ChannelSwitch(fileSink);
        cs->AddDefaultRoute(shaSink);

        Inflator decompression(cs);

        BufferedTransformation * lastTransform = new Redirector(decompression);

        // Build the decryption chain
        size_t keyOffset = 0;
        for (int ii = 0; ii < fileHeader.ciphers_size(); ++ii)
        {
            if (fileHeader.ciphers(ii) == sotcore::Cipher::AES)
            {
                CTR_Mode<CryptoPP::AES>::Decryption * decryption = new CTR_Mode<CryptoPP::AES>::Decryption((uint8_t *)&cfg.key()[keyOffset], CryptoPP::AES::MAX_KEYLENGTH, (uint8_t *)fileHeader.ivs(ii).c_str(), fileHeader.ivs(ii).size());
                encryptionDestructors.Add(Destructor<CTR_Mode<CryptoPP::AES>::Decryption>, decryption);
                StreamTransformationFilter * stf = new StreamTransformationFilter(*decryption, lastTransform);

                lastTransform = stf;
                keyOffset += CryptoPP::AES::MAX_KEYLENGTH;
            }
            else if (fileHeader.ciphers(ii) == sotcore::Cipher::TWOFISH)
            {
                CTR_Mode<CryptoPP::Twofish>::Decryption * decryption = new CTR_Mode<CryptoPP::Twofish>::Decryption((uint8_t *)&cfg.key()[keyOffset], CryptoPP::Twofish::MAX_KEYLENGTH, (uint8_t *)fileHeader.ivs(ii).c_str(), fileHeader.ivs(ii).size());
                encryptionDestructors.Add(Destructor<CTR_Mode<CryptoPP::Twofish>::Decryption>, decryption);
                StreamTransformationFilter * stf = new StreamTransformationFilter(*decryption, lastTransform);

                lastTransform = stf;
                keyOffset += CryptoPP::Twofish::MAX_KEYLENGTH;
            }
            else if (fileHeader.ciphers(ii) == sotcore::Cipher::SERPENT)
            {
                CTR_Mode<CryptoPP::Serpent>::Decryption * decryption = new CTR_Mode<CryptoPP::Serpent>::Decryption((uint8_t *)&cfg.key()[keyOffset], CryptoPP::Serpent::MAX_KEYLENGTH, (uint8_t *)fileHeader.ivs(ii).c_str(), fileHeader.ivs(ii).size());
                encryptionDestructors.Add(Destructor<CTR_Mode<CryptoPP::Serpent>::Decryption>, decryption);
                StreamTransformationFilter * stf = new StreamTransformationFilter(*decryption, lastTransform);

                lastTransform = stf;
                keyOffset += CryptoPP::Serpent::MAX_KEYLENGTH;
            }
        }

        FileSource ss(ifs, true, lastTransform);
    }
}