// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: FileHeader.proto

#include "FileHeader.pb.h"

#include <algorithm>

#include <google/protobuf/stubs/common.h>
#include <google/protobuf/stubs/port.h>
#include <google/protobuf/io/coded_stream.h>
#include <google/protobuf/wire_format_lite_inl.h>
#include <google/protobuf/descriptor.h>
#include <google/protobuf/generated_message_reflection.h>
#include <google/protobuf/reflection_ops.h>
#include <google/protobuf/wire_format.h>
// This is a temporary google only hack
#ifdef GOOGLE_PROTOBUF_ENFORCE_UNIQUENESS
#include "third_party/protobuf/version.h"
#endif
// @@protoc_insertion_point(includes)

namespace sotcore {
class FileHeaderDefaultTypeInternal {
 public:
  ::google::protobuf::internal::ExplicitlyConstructed<FileHeader>
      _instance;
} _FileHeader_default_instance_;
}  // namespace sotcore
namespace protobuf_FileHeader_2eproto {
static void InitDefaultsFileHeader() {
  GOOGLE_PROTOBUF_VERIFY_VERSION;

  {
    void* ptr = &::sotcore::_FileHeader_default_instance_;
    new (ptr) ::sotcore::FileHeader();
    ::google::protobuf::internal::OnShutdownDestroyMessage(ptr);
  }
  ::sotcore::FileHeader::InitAsDefaultInstance();
}

::google::protobuf::internal::SCCInfo<0> scc_info_FileHeader =
    {{ATOMIC_VAR_INIT(::google::protobuf::internal::SCCInfoBase::kUninitialized), 0, InitDefaultsFileHeader}, {}};

void InitDefaults() {
  ::google::protobuf::internal::InitSCC(&scc_info_FileHeader.base);
}

::google::protobuf::Metadata file_level_metadata[1];
const ::google::protobuf::EnumDescriptor* file_level_enum_descriptors[2];

const ::google::protobuf::uint32 TableStruct::offsets[] GOOGLE_PROTOBUF_ATTRIBUTE_SECTION_VARIABLE(protodesc_cold) = {
  ~0u,  // no _has_bits_
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::sotcore::FileHeader, _internal_metadata_),
  ~0u,  // no _extensions_
  ~0u,  // no _oneof_case_
  ~0u,  // no _weak_field_map_
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::sotcore::FileHeader, compression_type_),
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::sotcore::FileHeader, compression_level_),
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::sotcore::FileHeader, ciphers_),
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::sotcore::FileHeader, ivs_),
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::sotcore::FileHeader, checksum_),
};
static const ::google::protobuf::internal::MigrationSchema schemas[] GOOGLE_PROTOBUF_ATTRIBUTE_SECTION_VARIABLE(protodesc_cold) = {
  { 0, -1, sizeof(::sotcore::FileHeader)},
};

static ::google::protobuf::Message const * const file_default_instances[] = {
  reinterpret_cast<const ::google::protobuf::Message*>(&::sotcore::_FileHeader_default_instance_),
};

void protobuf_AssignDescriptors() {
  AddDescriptors();
  AssignDescriptors(
      "FileHeader.proto", schemas, file_default_instances, TableStruct::offsets,
      file_level_metadata, file_level_enum_descriptors, NULL);
}

void protobuf_AssignDescriptorsOnce() {
  static ::google::protobuf::internal::once_flag once;
  ::google::protobuf::internal::call_once(once, protobuf_AssignDescriptors);
}

void protobuf_RegisterTypes(const ::std::string&) GOOGLE_PROTOBUF_ATTRIBUTE_COLD;
void protobuf_RegisterTypes(const ::std::string&) {
  protobuf_AssignDescriptorsOnce();
  ::google::protobuf::internal::RegisterAllTypes(file_level_metadata, 1);
}

void AddDescriptorsImpl() {
  InitDefaults();
  static const char descriptor[] GOOGLE_PROTOBUF_ATTRIBUTE_SECTION_VARIABLE(protodesc_cold) = {
      "\n\020FileHeader.proto\022\007sotcore\"\234\001\n\nFileHead"
      "er\0222\n\020compression_type\030\001 \001(\0162\030.sotcore.C"
      "ompressionType\022\031\n\021compression_level\030\002 \001("
      "\005\022 \n\007ciphers\030\003 \003(\0162\017.sotcore.Cipher\022\013\n\003i"
      "vs\030\004 \003(\014\022\020\n\010checksum\030\005 \001(\014*\033\n\017Compressio"
      "nType\022\010\n\004ZLIB\020\000*:\n\006Cipher\022\r\n\tUNDEFINED\020\000"
      "\022\007\n\003AES\020\001\022\013\n\007TWOFISH\020\002\022\013\n\007SERPENT\020\003B\021\252\002\016"
      "CloudSync.Coreb\006proto3"
  };
  ::google::protobuf::DescriptorPool::InternalAddGeneratedFile(
      descriptor, 302);
  ::google::protobuf::MessageFactory::InternalRegisterGeneratedFile(
    "FileHeader.proto", &protobuf_RegisterTypes);
}

void AddDescriptors() {
  static ::google::protobuf::internal::once_flag once;
  ::google::protobuf::internal::call_once(once, AddDescriptorsImpl);
}
// Force AddDescriptors() to be called at dynamic initialization time.
struct StaticDescriptorInitializer {
  StaticDescriptorInitializer() {
    AddDescriptors();
  }
} static_descriptor_initializer;
}  // namespace protobuf_FileHeader_2eproto
namespace sotcore {
const ::google::protobuf::EnumDescriptor* CompressionType_descriptor() {
  protobuf_FileHeader_2eproto::protobuf_AssignDescriptorsOnce();
  return protobuf_FileHeader_2eproto::file_level_enum_descriptors[0];
}
bool CompressionType_IsValid(int value) {
  switch (value) {
    case 0:
      return true;
    default:
      return false;
  }
}

const ::google::protobuf::EnumDescriptor* Cipher_descriptor() {
  protobuf_FileHeader_2eproto::protobuf_AssignDescriptorsOnce();
  return protobuf_FileHeader_2eproto::file_level_enum_descriptors[1];
}
bool Cipher_IsValid(int value) {
  switch (value) {
    case 0:
    case 1:
    case 2:
    case 3:
      return true;
    default:
      return false;
  }
}


// ===================================================================

void FileHeader::InitAsDefaultInstance() {
}
#if !defined(_MSC_VER) || _MSC_VER >= 1900
const int FileHeader::kCompressionTypeFieldNumber;
const int FileHeader::kCompressionLevelFieldNumber;
const int FileHeader::kCiphersFieldNumber;
const int FileHeader::kIvsFieldNumber;
const int FileHeader::kChecksumFieldNumber;
#endif  // !defined(_MSC_VER) || _MSC_VER >= 1900

FileHeader::FileHeader()
  : ::google::protobuf::Message(), _internal_metadata_(NULL) {
  ::google::protobuf::internal::InitSCC(
      &protobuf_FileHeader_2eproto::scc_info_FileHeader.base);
  SharedCtor();
  // @@protoc_insertion_point(constructor:sotcore.FileHeader)
}
FileHeader::FileHeader(const FileHeader& from)
  : ::google::protobuf::Message(),
      _internal_metadata_(NULL),
      ciphers_(from.ciphers_),
      ivs_(from.ivs_) {
  _internal_metadata_.MergeFrom(from._internal_metadata_);
  checksum_.UnsafeSetDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  if (from.checksum().size() > 0) {
    checksum_.AssignWithDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), from.checksum_);
  }
  ::memcpy(&compression_type_, &from.compression_type_,
    static_cast<size_t>(reinterpret_cast<char*>(&compression_level_) -
    reinterpret_cast<char*>(&compression_type_)) + sizeof(compression_level_));
  // @@protoc_insertion_point(copy_constructor:sotcore.FileHeader)
}

void FileHeader::SharedCtor() {
  checksum_.UnsafeSetDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  ::memset(&compression_type_, 0, static_cast<size_t>(
      reinterpret_cast<char*>(&compression_level_) -
      reinterpret_cast<char*>(&compression_type_)) + sizeof(compression_level_));
}

FileHeader::~FileHeader() {
  // @@protoc_insertion_point(destructor:sotcore.FileHeader)
  SharedDtor();
}

void FileHeader::SharedDtor() {
  checksum_.DestroyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}

void FileHeader::SetCachedSize(int size) const {
  _cached_size_.Set(size);
}
const ::google::protobuf::Descriptor* FileHeader::descriptor() {
  ::protobuf_FileHeader_2eproto::protobuf_AssignDescriptorsOnce();
  return ::protobuf_FileHeader_2eproto::file_level_metadata[kIndexInFileMessages].descriptor;
}

const FileHeader& FileHeader::default_instance() {
  ::google::protobuf::internal::InitSCC(&protobuf_FileHeader_2eproto::scc_info_FileHeader.base);
  return *internal_default_instance();
}


void FileHeader::Clear() {
// @@protoc_insertion_point(message_clear_start:sotcore.FileHeader)
  ::google::protobuf::uint32 cached_has_bits = 0;
  // Prevent compiler warnings about cached_has_bits being unused
  (void) cached_has_bits;

  ciphers_.Clear();
  ivs_.Clear();
  checksum_.ClearToEmptyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  ::memset(&compression_type_, 0, static_cast<size_t>(
      reinterpret_cast<char*>(&compression_level_) -
      reinterpret_cast<char*>(&compression_type_)) + sizeof(compression_level_));
  _internal_metadata_.Clear();
}

bool FileHeader::MergePartialFromCodedStream(
    ::google::protobuf::io::CodedInputStream* input) {
#define DO_(EXPRESSION) if (!GOOGLE_PREDICT_TRUE(EXPRESSION)) goto failure
  ::google::protobuf::uint32 tag;
  // @@protoc_insertion_point(parse_start:sotcore.FileHeader)
  for (;;) {
    ::std::pair<::google::protobuf::uint32, bool> p = input->ReadTagWithCutoffNoLastTag(127u);
    tag = p.first;
    if (!p.second) goto handle_unusual;
    switch (::google::protobuf::internal::WireFormatLite::GetTagFieldNumber(tag)) {
      // .sotcore.CompressionType compression_type = 1;
      case 1: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(8u /* 8 & 0xFF */)) {
          int value;
          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   int, ::google::protobuf::internal::WireFormatLite::TYPE_ENUM>(
                 input, &value)));
          set_compression_type(static_cast< ::sotcore::CompressionType >(value));
        } else {
          goto handle_unusual;
        }
        break;
      }

      // int32 compression_level = 2;
      case 2: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(16u /* 16 & 0xFF */)) {

          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   ::google::protobuf::int32, ::google::protobuf::internal::WireFormatLite::TYPE_INT32>(
                 input, &compression_level_)));
        } else {
          goto handle_unusual;
        }
        break;
      }

      // repeated .sotcore.Cipher ciphers = 3;
      case 3: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(26u /* 26 & 0xFF */)) {
          ::google::protobuf::uint32 length;
          DO_(input->ReadVarint32(&length));
          ::google::protobuf::io::CodedInputStream::Limit limit = input->PushLimit(static_cast<int>(length));
          while (input->BytesUntilLimit() > 0) {
            int value;
            DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   int, ::google::protobuf::internal::WireFormatLite::TYPE_ENUM>(
                 input, &value)));
            add_ciphers(static_cast< ::sotcore::Cipher >(value));
          }
          input->PopLimit(limit);
        } else if (
            static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(24u /* 24 & 0xFF */)) {
          int value;
          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   int, ::google::protobuf::internal::WireFormatLite::TYPE_ENUM>(
                 input, &value)));
          add_ciphers(static_cast< ::sotcore::Cipher >(value));
        } else {
          goto handle_unusual;
        }
        break;
      }

      // repeated bytes ivs = 4;
      case 4: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(34u /* 34 & 0xFF */)) {
          DO_(::google::protobuf::internal::WireFormatLite::ReadBytes(
                input, this->add_ivs()));
        } else {
          goto handle_unusual;
        }
        break;
      }

      // bytes checksum = 5;
      case 5: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(42u /* 42 & 0xFF */)) {
          DO_(::google::protobuf::internal::WireFormatLite::ReadBytes(
                input, this->mutable_checksum()));
        } else {
          goto handle_unusual;
        }
        break;
      }

      default: {
      handle_unusual:
        if (tag == 0) {
          goto success;
        }
        DO_(::google::protobuf::internal::WireFormat::SkipField(
              input, tag, _internal_metadata_.mutable_unknown_fields()));
        break;
      }
    }
  }
success:
  // @@protoc_insertion_point(parse_success:sotcore.FileHeader)
  return true;
failure:
  // @@protoc_insertion_point(parse_failure:sotcore.FileHeader)
  return false;
#undef DO_
}

void FileHeader::SerializeWithCachedSizes(
    ::google::protobuf::io::CodedOutputStream* output) const {
  // @@protoc_insertion_point(serialize_start:sotcore.FileHeader)
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  // .sotcore.CompressionType compression_type = 1;
  if (this->compression_type() != 0) {
    ::google::protobuf::internal::WireFormatLite::WriteEnum(
      1, this->compression_type(), output);
  }

  // int32 compression_level = 2;
  if (this->compression_level() != 0) {
    ::google::protobuf::internal::WireFormatLite::WriteInt32(2, this->compression_level(), output);
  }

  // repeated .sotcore.Cipher ciphers = 3;
  if (this->ciphers_size() > 0) {
    ::google::protobuf::internal::WireFormatLite::WriteTag(
      3,
      ::google::protobuf::internal::WireFormatLite::WIRETYPE_LENGTH_DELIMITED,
      output);
    output->WriteVarint32(
        static_cast< ::google::protobuf::uint32>(_ciphers_cached_byte_size_));
  }
  for (int i = 0, n = this->ciphers_size(); i < n; i++) {
    ::google::protobuf::internal::WireFormatLite::WriteEnumNoTag(
      this->ciphers(i), output);
  }

  // repeated bytes ivs = 4;
  for (int i = 0, n = this->ivs_size(); i < n; i++) {
    ::google::protobuf::internal::WireFormatLite::WriteBytes(
      4, this->ivs(i), output);
  }

  // bytes checksum = 5;
  if (this->checksum().size() > 0) {
    ::google::protobuf::internal::WireFormatLite::WriteBytesMaybeAliased(
      5, this->checksum(), output);
  }

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    ::google::protobuf::internal::WireFormat::SerializeUnknownFields(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()), output);
  }
  // @@protoc_insertion_point(serialize_end:sotcore.FileHeader)
}

::google::protobuf::uint8* FileHeader::InternalSerializeWithCachedSizesToArray(
    bool deterministic, ::google::protobuf::uint8* target) const {
  (void)deterministic; // Unused
  // @@protoc_insertion_point(serialize_to_array_start:sotcore.FileHeader)
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  // .sotcore.CompressionType compression_type = 1;
  if (this->compression_type() != 0) {
    target = ::google::protobuf::internal::WireFormatLite::WriteEnumToArray(
      1, this->compression_type(), target);
  }

  // int32 compression_level = 2;
  if (this->compression_level() != 0) {
    target = ::google::protobuf::internal::WireFormatLite::WriteInt32ToArray(2, this->compression_level(), target);
  }

  // repeated .sotcore.Cipher ciphers = 3;
  if (this->ciphers_size() > 0) {
    target = ::google::protobuf::internal::WireFormatLite::WriteTagToArray(
      3,
      ::google::protobuf::internal::WireFormatLite::WIRETYPE_LENGTH_DELIMITED,
      target);
    target = ::google::protobuf::io::CodedOutputStream::WriteVarint32ToArray(      static_cast< ::google::protobuf::uint32>(
            _ciphers_cached_byte_size_), target);
    target = ::google::protobuf::internal::WireFormatLite::WriteEnumNoTagToArray(
      this->ciphers_, target);
  }

  // repeated bytes ivs = 4;
  for (int i = 0, n = this->ivs_size(); i < n; i++) {
    target = ::google::protobuf::internal::WireFormatLite::
      WriteBytesToArray(4, this->ivs(i), target);
  }

  // bytes checksum = 5;
  if (this->checksum().size() > 0) {
    target =
      ::google::protobuf::internal::WireFormatLite::WriteBytesToArray(
        5, this->checksum(), target);
  }

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    target = ::google::protobuf::internal::WireFormat::SerializeUnknownFieldsToArray(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()), target);
  }
  // @@protoc_insertion_point(serialize_to_array_end:sotcore.FileHeader)
  return target;
}

size_t FileHeader::ByteSizeLong() const {
// @@protoc_insertion_point(message_byte_size_start:sotcore.FileHeader)
  size_t total_size = 0;

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    total_size +=
      ::google::protobuf::internal::WireFormat::ComputeUnknownFieldsSize(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()));
  }
  // repeated .sotcore.Cipher ciphers = 3;
  {
    size_t data_size = 0;
    unsigned int count = static_cast<unsigned int>(this->ciphers_size());for (unsigned int i = 0; i < count; i++) {
      data_size += ::google::protobuf::internal::WireFormatLite::EnumSize(
        this->ciphers(static_cast<int>(i)));
    }
    if (data_size > 0) {
      total_size += 1 +
        ::google::protobuf::internal::WireFormatLite::Int32Size(
            static_cast< ::google::protobuf::int32>(data_size));
    }
    int cached_size = ::google::protobuf::internal::ToCachedSize(data_size);
    GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
    _ciphers_cached_byte_size_ = cached_size;
    GOOGLE_SAFE_CONCURRENT_WRITES_END();
    total_size += data_size;
  }

  // repeated bytes ivs = 4;
  total_size += 1 *
      ::google::protobuf::internal::FromIntSize(this->ivs_size());
  for (int i = 0, n = this->ivs_size(); i < n; i++) {
    total_size += ::google::protobuf::internal::WireFormatLite::BytesSize(
      this->ivs(i));
  }

  // bytes checksum = 5;
  if (this->checksum().size() > 0) {
    total_size += 1 +
      ::google::protobuf::internal::WireFormatLite::BytesSize(
        this->checksum());
  }

  // .sotcore.CompressionType compression_type = 1;
  if (this->compression_type() != 0) {
    total_size += 1 +
      ::google::protobuf::internal::WireFormatLite::EnumSize(this->compression_type());
  }

  // int32 compression_level = 2;
  if (this->compression_level() != 0) {
    total_size += 1 +
      ::google::protobuf::internal::WireFormatLite::Int32Size(
        this->compression_level());
  }

  int cached_size = ::google::protobuf::internal::ToCachedSize(total_size);
  SetCachedSize(cached_size);
  return total_size;
}

void FileHeader::MergeFrom(const ::google::protobuf::Message& from) {
// @@protoc_insertion_point(generalized_merge_from_start:sotcore.FileHeader)
  GOOGLE_DCHECK_NE(&from, this);
  const FileHeader* source =
      ::google::protobuf::internal::DynamicCastToGenerated<const FileHeader>(
          &from);
  if (source == NULL) {
  // @@protoc_insertion_point(generalized_merge_from_cast_fail:sotcore.FileHeader)
    ::google::protobuf::internal::ReflectionOps::Merge(from, this);
  } else {
  // @@protoc_insertion_point(generalized_merge_from_cast_success:sotcore.FileHeader)
    MergeFrom(*source);
  }
}

void FileHeader::MergeFrom(const FileHeader& from) {
// @@protoc_insertion_point(class_specific_merge_from_start:sotcore.FileHeader)
  GOOGLE_DCHECK_NE(&from, this);
  _internal_metadata_.MergeFrom(from._internal_metadata_);
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  ciphers_.MergeFrom(from.ciphers_);
  ivs_.MergeFrom(from.ivs_);
  if (from.checksum().size() > 0) {

    checksum_.AssignWithDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), from.checksum_);
  }
  if (from.compression_type() != 0) {
    set_compression_type(from.compression_type());
  }
  if (from.compression_level() != 0) {
    set_compression_level(from.compression_level());
  }
}

void FileHeader::CopyFrom(const ::google::protobuf::Message& from) {
// @@protoc_insertion_point(generalized_copy_from_start:sotcore.FileHeader)
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

void FileHeader::CopyFrom(const FileHeader& from) {
// @@protoc_insertion_point(class_specific_copy_from_start:sotcore.FileHeader)
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

bool FileHeader::IsInitialized() const {
  return true;
}

void FileHeader::Swap(FileHeader* other) {
  if (other == this) return;
  InternalSwap(other);
}
void FileHeader::InternalSwap(FileHeader* other) {
  using std::swap;
  ciphers_.InternalSwap(&other->ciphers_);
  ivs_.InternalSwap(CastToBase(&other->ivs_));
  checksum_.Swap(&other->checksum_, &::google::protobuf::internal::GetEmptyStringAlreadyInited(),
    GetArenaNoVirtual());
  swap(compression_type_, other->compression_type_);
  swap(compression_level_, other->compression_level_);
  _internal_metadata_.Swap(&other->_internal_metadata_);
}

::google::protobuf::Metadata FileHeader::GetMetadata() const {
  protobuf_FileHeader_2eproto::protobuf_AssignDescriptorsOnce();
  return ::protobuf_FileHeader_2eproto::file_level_metadata[kIndexInFileMessages];
}


// @@protoc_insertion_point(namespace_scope)
}  // namespace sotcore
namespace google {
namespace protobuf {
template<> GOOGLE_PROTOBUF_ATTRIBUTE_NOINLINE ::sotcore::FileHeader* Arena::CreateMaybeMessage< ::sotcore::FileHeader >(Arena* arena) {
  return Arena::CreateInternal< ::sotcore::FileHeader >(arena);
}
}  // namespace protobuf
}  // namespace google

// @@protoc_insertion_point(global_scope)
