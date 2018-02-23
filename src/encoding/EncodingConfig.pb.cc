// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: EncodingConfig.proto

#include "EncodingConfig.pb.h"

#include <algorithm>

#include <google/protobuf/stubs/common.h>
#include <google/protobuf/stubs/port.h>
#include <google/protobuf/stubs/once.h>
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
class EncodingConfigDefaultTypeInternal {
 public:
  ::google::protobuf::internal::ExplicitlyConstructed<EncodingConfig>
      _instance;
} _EncodingConfig_default_instance_;
}  // namespace sotcore
namespace protobuf_EncodingConfig_2eproto {
void InitDefaultsEncodingConfigImpl() {
  GOOGLE_PROTOBUF_VERIFY_VERSION;

#ifdef GOOGLE_PROTOBUF_ENFORCE_UNIQUENESS
  ::google::protobuf::internal::InitProtobufDefaultsForceUnique();
#else
  ::google::protobuf::internal::InitProtobufDefaults();
#endif  // GOOGLE_PROTOBUF_ENFORCE_UNIQUENESS
  {
    void* ptr = &::sotcore::_EncodingConfig_default_instance_;
    new (ptr) ::sotcore::EncodingConfig();
    ::google::protobuf::internal::OnShutdownDestroyMessage(ptr);
  }
  ::sotcore::EncodingConfig::InitAsDefaultInstance();
}

void InitDefaultsEncodingConfig() {
  static GOOGLE_PROTOBUF_DECLARE_ONCE(once);
  ::google::protobuf::GoogleOnceInit(&once, &InitDefaultsEncodingConfigImpl);
}

::google::protobuf::Metadata file_level_metadata[1];

const ::google::protobuf::uint32 TableStruct::offsets[] GOOGLE_PROTOBUF_ATTRIBUTE_SECTION_VARIABLE(protodesc_cold) = {
  ~0u,  // no _has_bits_
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::sotcore::EncodingConfig, _internal_metadata_),
  ~0u,  // no _extensions_
  ~0u,  // no _oneof_case_
  ~0u,  // no _weak_field_map_
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::sotcore::EncodingConfig, compression_level_),
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::sotcore::EncodingConfig, ciphers_),
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::sotcore::EncodingConfig, key_),
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::sotcore::EncodingConfig, src_),
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::sotcore::EncodingConfig, dest_),
};
static const ::google::protobuf::internal::MigrationSchema schemas[] GOOGLE_PROTOBUF_ATTRIBUTE_SECTION_VARIABLE(protodesc_cold) = {
  { 0, -1, sizeof(::sotcore::EncodingConfig)},
};

static ::google::protobuf::Message const * const file_default_instances[] = {
  reinterpret_cast<const ::google::protobuf::Message*>(&::sotcore::_EncodingConfig_default_instance_),
};

void protobuf_AssignDescriptors() {
  AddDescriptors();
  ::google::protobuf::MessageFactory* factory = NULL;
  AssignDescriptors(
      "EncodingConfig.proto", schemas, file_default_instances, TableStruct::offsets, factory,
      file_level_metadata, NULL, NULL);
}

void protobuf_AssignDescriptorsOnce() {
  static GOOGLE_PROTOBUF_DECLARE_ONCE(once);
  ::google::protobuf::GoogleOnceInit(&once, &protobuf_AssignDescriptors);
}

void protobuf_RegisterTypes(const ::std::string&) GOOGLE_PROTOBUF_ATTRIBUTE_COLD;
void protobuf_RegisterTypes(const ::std::string&) {
  protobuf_AssignDescriptorsOnce();
  ::google::protobuf::internal::RegisterAllTypes(file_level_metadata, 1);
}

void AddDescriptorsImpl() {
  InitDefaults();
  static const char descriptor[] GOOGLE_PROTOBUF_ATTRIBUTE_SECTION_VARIABLE(protodesc_cold) = {
      "\n\024EncodingConfig.proto\022\007sotcore\"d\n\016Encod"
      "ingConfig\022\031\n\021compression_level\030\001 \001(\005\022\017\n\007"
      "ciphers\030\002 \003(\t\022\013\n\003key\030\003 \001(\014\022\013\n\003src\030\004 \001(\t\022"
      "\014\n\004dest\030\005 \001(\tB\021\252\002\016CloudSync.Coreb\006proto3"
  };
  ::google::protobuf::DescriptorPool::InternalAddGeneratedFile(
      descriptor, 160);
  ::google::protobuf::MessageFactory::InternalRegisterGeneratedFile(
    "EncodingConfig.proto", &protobuf_RegisterTypes);
}

void AddDescriptors() {
  static GOOGLE_PROTOBUF_DECLARE_ONCE(once);
  ::google::protobuf::GoogleOnceInit(&once, &AddDescriptorsImpl);
}
// Force AddDescriptors() to be called at dynamic initialization time.
struct StaticDescriptorInitializer {
  StaticDescriptorInitializer() {
    AddDescriptors();
  }
} static_descriptor_initializer;
}  // namespace protobuf_EncodingConfig_2eproto
namespace sotcore {

// ===================================================================

void EncodingConfig::InitAsDefaultInstance() {
}
#if !defined(_MSC_VER) || _MSC_VER >= 1900
const int EncodingConfig::kCompressionLevelFieldNumber;
const int EncodingConfig::kCiphersFieldNumber;
const int EncodingConfig::kKeyFieldNumber;
const int EncodingConfig::kSrcFieldNumber;
const int EncodingConfig::kDestFieldNumber;
#endif  // !defined(_MSC_VER) || _MSC_VER >= 1900

EncodingConfig::EncodingConfig()
  : ::google::protobuf::Message(), _internal_metadata_(NULL) {
  if (GOOGLE_PREDICT_TRUE(this != internal_default_instance())) {
    ::protobuf_EncodingConfig_2eproto::InitDefaultsEncodingConfig();
  }
  SharedCtor();
  // @@protoc_insertion_point(constructor:sotcore.EncodingConfig)
}
EncodingConfig::EncodingConfig(const EncodingConfig& from)
  : ::google::protobuf::Message(),
      _internal_metadata_(NULL),
      ciphers_(from.ciphers_),
      _cached_size_(0) {
  _internal_metadata_.MergeFrom(from._internal_metadata_);
  key_.UnsafeSetDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  if (from.key().size() > 0) {
    key_.AssignWithDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), from.key_);
  }
  src_.UnsafeSetDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  if (from.src().size() > 0) {
    src_.AssignWithDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), from.src_);
  }
  dest_.UnsafeSetDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  if (from.dest().size() > 0) {
    dest_.AssignWithDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), from.dest_);
  }
  compression_level_ = from.compression_level_;
  // @@protoc_insertion_point(copy_constructor:sotcore.EncodingConfig)
}

void EncodingConfig::SharedCtor() {
  key_.UnsafeSetDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  src_.UnsafeSetDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  dest_.UnsafeSetDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  compression_level_ = 0;
  _cached_size_ = 0;
}

EncodingConfig::~EncodingConfig() {
  // @@protoc_insertion_point(destructor:sotcore.EncodingConfig)
  SharedDtor();
}

void EncodingConfig::SharedDtor() {
  key_.DestroyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  src_.DestroyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  dest_.DestroyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}

void EncodingConfig::SetCachedSize(int size) const {
  GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
  _cached_size_ = size;
  GOOGLE_SAFE_CONCURRENT_WRITES_END();
}
const ::google::protobuf::Descriptor* EncodingConfig::descriptor() {
  ::protobuf_EncodingConfig_2eproto::protobuf_AssignDescriptorsOnce();
  return ::protobuf_EncodingConfig_2eproto::file_level_metadata[kIndexInFileMessages].descriptor;
}

const EncodingConfig& EncodingConfig::default_instance() {
  ::protobuf_EncodingConfig_2eproto::InitDefaultsEncodingConfig();
  return *internal_default_instance();
}

EncodingConfig* EncodingConfig::New(::google::protobuf::Arena* arena) const {
  EncodingConfig* n = new EncodingConfig;
  if (arena != NULL) {
    arena->Own(n);
  }
  return n;
}

void EncodingConfig::Clear() {
// @@protoc_insertion_point(message_clear_start:sotcore.EncodingConfig)
  ::google::protobuf::uint32 cached_has_bits = 0;
  // Prevent compiler warnings about cached_has_bits being unused
  (void) cached_has_bits;

  ciphers_.Clear();
  key_.ClearToEmptyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  src_.ClearToEmptyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  dest_.ClearToEmptyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  compression_level_ = 0;
  _internal_metadata_.Clear();
}

bool EncodingConfig::MergePartialFromCodedStream(
    ::google::protobuf::io::CodedInputStream* input) {
#define DO_(EXPRESSION) if (!GOOGLE_PREDICT_TRUE(EXPRESSION)) goto failure
  ::google::protobuf::uint32 tag;
  // @@protoc_insertion_point(parse_start:sotcore.EncodingConfig)
  for (;;) {
    ::std::pair< ::google::protobuf::uint32, bool> p = input->ReadTagWithCutoffNoLastTag(127u);
    tag = p.first;
    if (!p.second) goto handle_unusual;
    switch (::google::protobuf::internal::WireFormatLite::GetTagFieldNumber(tag)) {
      // int32 compression_level = 1;
      case 1: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(8u /* 8 & 0xFF */)) {

          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   ::google::protobuf::int32, ::google::protobuf::internal::WireFormatLite::TYPE_INT32>(
                 input, &compression_level_)));
        } else {
          goto handle_unusual;
        }
        break;
      }

      // repeated string ciphers = 2;
      case 2: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(18u /* 18 & 0xFF */)) {
          DO_(::google::protobuf::internal::WireFormatLite::ReadString(
                input, this->add_ciphers()));
          DO_(::google::protobuf::internal::WireFormatLite::VerifyUtf8String(
            this->ciphers(this->ciphers_size() - 1).data(),
            static_cast<int>(this->ciphers(this->ciphers_size() - 1).length()),
            ::google::protobuf::internal::WireFormatLite::PARSE,
            "sotcore.EncodingConfig.ciphers"));
        } else {
          goto handle_unusual;
        }
        break;
      }

      // bytes key = 3;
      case 3: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(26u /* 26 & 0xFF */)) {
          DO_(::google::protobuf::internal::WireFormatLite::ReadBytes(
                input, this->mutable_key()));
        } else {
          goto handle_unusual;
        }
        break;
      }

      // string src = 4;
      case 4: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(34u /* 34 & 0xFF */)) {
          DO_(::google::protobuf::internal::WireFormatLite::ReadString(
                input, this->mutable_src()));
          DO_(::google::protobuf::internal::WireFormatLite::VerifyUtf8String(
            this->src().data(), static_cast<int>(this->src().length()),
            ::google::protobuf::internal::WireFormatLite::PARSE,
            "sotcore.EncodingConfig.src"));
        } else {
          goto handle_unusual;
        }
        break;
      }

      // string dest = 5;
      case 5: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(42u /* 42 & 0xFF */)) {
          DO_(::google::protobuf::internal::WireFormatLite::ReadString(
                input, this->mutable_dest()));
          DO_(::google::protobuf::internal::WireFormatLite::VerifyUtf8String(
            this->dest().data(), static_cast<int>(this->dest().length()),
            ::google::protobuf::internal::WireFormatLite::PARSE,
            "sotcore.EncodingConfig.dest"));
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
  // @@protoc_insertion_point(parse_success:sotcore.EncodingConfig)
  return true;
failure:
  // @@protoc_insertion_point(parse_failure:sotcore.EncodingConfig)
  return false;
#undef DO_
}

void EncodingConfig::SerializeWithCachedSizes(
    ::google::protobuf::io::CodedOutputStream* output) const {
  // @@protoc_insertion_point(serialize_start:sotcore.EncodingConfig)
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  // int32 compression_level = 1;
  if (this->compression_level() != 0) {
    ::google::protobuf::internal::WireFormatLite::WriteInt32(1, this->compression_level(), output);
  }

  // repeated string ciphers = 2;
  for (int i = 0, n = this->ciphers_size(); i < n; i++) {
    ::google::protobuf::internal::WireFormatLite::VerifyUtf8String(
      this->ciphers(i).data(), static_cast<int>(this->ciphers(i).length()),
      ::google::protobuf::internal::WireFormatLite::SERIALIZE,
      "sotcore.EncodingConfig.ciphers");
    ::google::protobuf::internal::WireFormatLite::WriteString(
      2, this->ciphers(i), output);
  }

  // bytes key = 3;
  if (this->key().size() > 0) {
    ::google::protobuf::internal::WireFormatLite::WriteBytesMaybeAliased(
      3, this->key(), output);
  }

  // string src = 4;
  if (this->src().size() > 0) {
    ::google::protobuf::internal::WireFormatLite::VerifyUtf8String(
      this->src().data(), static_cast<int>(this->src().length()),
      ::google::protobuf::internal::WireFormatLite::SERIALIZE,
      "sotcore.EncodingConfig.src");
    ::google::protobuf::internal::WireFormatLite::WriteStringMaybeAliased(
      4, this->src(), output);
  }

  // string dest = 5;
  if (this->dest().size() > 0) {
    ::google::protobuf::internal::WireFormatLite::VerifyUtf8String(
      this->dest().data(), static_cast<int>(this->dest().length()),
      ::google::protobuf::internal::WireFormatLite::SERIALIZE,
      "sotcore.EncodingConfig.dest");
    ::google::protobuf::internal::WireFormatLite::WriteStringMaybeAliased(
      5, this->dest(), output);
  }

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    ::google::protobuf::internal::WireFormat::SerializeUnknownFields(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()), output);
  }
  // @@protoc_insertion_point(serialize_end:sotcore.EncodingConfig)
}

::google::protobuf::uint8* EncodingConfig::InternalSerializeWithCachedSizesToArray(
    bool deterministic, ::google::protobuf::uint8* target) const {
  (void)deterministic; // Unused
  // @@protoc_insertion_point(serialize_to_array_start:sotcore.EncodingConfig)
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  // int32 compression_level = 1;
  if (this->compression_level() != 0) {
    target = ::google::protobuf::internal::WireFormatLite::WriteInt32ToArray(1, this->compression_level(), target);
  }

  // repeated string ciphers = 2;
  for (int i = 0, n = this->ciphers_size(); i < n; i++) {
    ::google::protobuf::internal::WireFormatLite::VerifyUtf8String(
      this->ciphers(i).data(), static_cast<int>(this->ciphers(i).length()),
      ::google::protobuf::internal::WireFormatLite::SERIALIZE,
      "sotcore.EncodingConfig.ciphers");
    target = ::google::protobuf::internal::WireFormatLite::
      WriteStringToArray(2, this->ciphers(i), target);
  }

  // bytes key = 3;
  if (this->key().size() > 0) {
    target =
      ::google::protobuf::internal::WireFormatLite::WriteBytesToArray(
        3, this->key(), target);
  }

  // string src = 4;
  if (this->src().size() > 0) {
    ::google::protobuf::internal::WireFormatLite::VerifyUtf8String(
      this->src().data(), static_cast<int>(this->src().length()),
      ::google::protobuf::internal::WireFormatLite::SERIALIZE,
      "sotcore.EncodingConfig.src");
    target =
      ::google::protobuf::internal::WireFormatLite::WriteStringToArray(
        4, this->src(), target);
  }

  // string dest = 5;
  if (this->dest().size() > 0) {
    ::google::protobuf::internal::WireFormatLite::VerifyUtf8String(
      this->dest().data(), static_cast<int>(this->dest().length()),
      ::google::protobuf::internal::WireFormatLite::SERIALIZE,
      "sotcore.EncodingConfig.dest");
    target =
      ::google::protobuf::internal::WireFormatLite::WriteStringToArray(
        5, this->dest(), target);
  }

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    target = ::google::protobuf::internal::WireFormat::SerializeUnknownFieldsToArray(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()), target);
  }
  // @@protoc_insertion_point(serialize_to_array_end:sotcore.EncodingConfig)
  return target;
}

size_t EncodingConfig::ByteSizeLong() const {
// @@protoc_insertion_point(message_byte_size_start:sotcore.EncodingConfig)
  size_t total_size = 0;

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    total_size +=
      ::google::protobuf::internal::WireFormat::ComputeUnknownFieldsSize(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()));
  }
  // repeated string ciphers = 2;
  total_size += 1 *
      ::google::protobuf::internal::FromIntSize(this->ciphers_size());
  for (int i = 0, n = this->ciphers_size(); i < n; i++) {
    total_size += ::google::protobuf::internal::WireFormatLite::StringSize(
      this->ciphers(i));
  }

  // bytes key = 3;
  if (this->key().size() > 0) {
    total_size += 1 +
      ::google::protobuf::internal::WireFormatLite::BytesSize(
        this->key());
  }

  // string src = 4;
  if (this->src().size() > 0) {
    total_size += 1 +
      ::google::protobuf::internal::WireFormatLite::StringSize(
        this->src());
  }

  // string dest = 5;
  if (this->dest().size() > 0) {
    total_size += 1 +
      ::google::protobuf::internal::WireFormatLite::StringSize(
        this->dest());
  }

  // int32 compression_level = 1;
  if (this->compression_level() != 0) {
    total_size += 1 +
      ::google::protobuf::internal::WireFormatLite::Int32Size(
        this->compression_level());
  }

  int cached_size = ::google::protobuf::internal::ToCachedSize(total_size);
  GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
  _cached_size_ = cached_size;
  GOOGLE_SAFE_CONCURRENT_WRITES_END();
  return total_size;
}

void EncodingConfig::MergeFrom(const ::google::protobuf::Message& from) {
// @@protoc_insertion_point(generalized_merge_from_start:sotcore.EncodingConfig)
  GOOGLE_DCHECK_NE(&from, this);
  const EncodingConfig* source =
      ::google::protobuf::internal::DynamicCastToGenerated<const EncodingConfig>(
          &from);
  if (source == NULL) {
  // @@protoc_insertion_point(generalized_merge_from_cast_fail:sotcore.EncodingConfig)
    ::google::protobuf::internal::ReflectionOps::Merge(from, this);
  } else {
  // @@protoc_insertion_point(generalized_merge_from_cast_success:sotcore.EncodingConfig)
    MergeFrom(*source);
  }
}

void EncodingConfig::MergeFrom(const EncodingConfig& from) {
// @@protoc_insertion_point(class_specific_merge_from_start:sotcore.EncodingConfig)
  GOOGLE_DCHECK_NE(&from, this);
  _internal_metadata_.MergeFrom(from._internal_metadata_);
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  ciphers_.MergeFrom(from.ciphers_);
  if (from.key().size() > 0) {

    key_.AssignWithDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), from.key_);
  }
  if (from.src().size() > 0) {

    src_.AssignWithDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), from.src_);
  }
  if (from.dest().size() > 0) {

    dest_.AssignWithDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), from.dest_);
  }
  if (from.compression_level() != 0) {
    set_compression_level(from.compression_level());
  }
}

void EncodingConfig::CopyFrom(const ::google::protobuf::Message& from) {
// @@protoc_insertion_point(generalized_copy_from_start:sotcore.EncodingConfig)
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

void EncodingConfig::CopyFrom(const EncodingConfig& from) {
// @@protoc_insertion_point(class_specific_copy_from_start:sotcore.EncodingConfig)
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

bool EncodingConfig::IsInitialized() const {
  return true;
}

void EncodingConfig::Swap(EncodingConfig* other) {
  if (other == this) return;
  InternalSwap(other);
}
void EncodingConfig::InternalSwap(EncodingConfig* other) {
  using std::swap;
  ciphers_.InternalSwap(&other->ciphers_);
  key_.Swap(&other->key_);
  src_.Swap(&other->src_);
  dest_.Swap(&other->dest_);
  swap(compression_level_, other->compression_level_);
  _internal_metadata_.Swap(&other->_internal_metadata_);
  swap(_cached_size_, other->_cached_size_);
}

::google::protobuf::Metadata EncodingConfig::GetMetadata() const {
  protobuf_EncodingConfig_2eproto::protobuf_AssignDescriptorsOnce();
  return ::protobuf_EncodingConfig_2eproto::file_level_metadata[kIndexInFileMessages];
}


// @@protoc_insertion_point(namespace_scope)
}  // namespace sotcore

// @@protoc_insertion_point(global_scope)