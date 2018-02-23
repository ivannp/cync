// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: EncodingConfig.proto

#ifndef PROTOBUF_EncodingConfig_2eproto__INCLUDED
#define PROTOBUF_EncodingConfig_2eproto__INCLUDED

#include <string>

#include <google/protobuf/stubs/common.h>

#if GOOGLE_PROTOBUF_VERSION < 3005000
#error This file was generated by a newer version of protoc which is
#error incompatible with your Protocol Buffer headers.  Please update
#error your headers.
#endif
#if 3005000 < GOOGLE_PROTOBUF_MIN_PROTOC_VERSION
#error This file was generated by an older version of protoc which is
#error incompatible with your Protocol Buffer headers.  Please
#error regenerate this file with a newer version of protoc.
#endif

#include <google/protobuf/io/coded_stream.h>
#include <google/protobuf/arena.h>
#include <google/protobuf/arenastring.h>
#include <google/protobuf/generated_message_table_driven.h>
#include <google/protobuf/generated_message_util.h>
#include <google/protobuf/metadata.h>
#include <google/protobuf/message.h>
#include <google/protobuf/repeated_field.h>  // IWYU pragma: export
#include <google/protobuf/extension_set.h>  // IWYU pragma: export
#include <google/protobuf/unknown_field_set.h>
// @@protoc_insertion_point(includes)

namespace protobuf_EncodingConfig_2eproto {
// Internal implementation detail -- do not use these members.
struct TableStruct {
  static const ::google::protobuf::internal::ParseTableField entries[];
  static const ::google::protobuf::internal::AuxillaryParseTableField aux[];
  static const ::google::protobuf::internal::ParseTable schema[1];
  static const ::google::protobuf::internal::FieldMetadata field_metadata[];
  static const ::google::protobuf::internal::SerializationTable serialization_table[];
  static const ::google::protobuf::uint32 offsets[];
};
void AddDescriptors();
void InitDefaultsEncodingConfigImpl();
void InitDefaultsEncodingConfig();
inline void InitDefaults() {
  InitDefaultsEncodingConfig();
}
}  // namespace protobuf_EncodingConfig_2eproto
namespace sotcore {
class EncodingConfig;
class EncodingConfigDefaultTypeInternal;
extern EncodingConfigDefaultTypeInternal _EncodingConfig_default_instance_;
}  // namespace sotcore
namespace sotcore {

// ===================================================================

class EncodingConfig : public ::google::protobuf::Message /* @@protoc_insertion_point(class_definition:sotcore.EncodingConfig) */ {
 public:
  EncodingConfig();
  virtual ~EncodingConfig();

  EncodingConfig(const EncodingConfig& from);

  inline EncodingConfig& operator=(const EncodingConfig& from) {
    CopyFrom(from);
    return *this;
  }
  #if LANG_CXX11
  EncodingConfig(EncodingConfig&& from) noexcept
    : EncodingConfig() {
    *this = ::std::move(from);
  }

  inline EncodingConfig& operator=(EncodingConfig&& from) noexcept {
    if (GetArenaNoVirtual() == from.GetArenaNoVirtual()) {
      if (this != &from) InternalSwap(&from);
    } else {
      CopyFrom(from);
    }
    return *this;
  }
  #endif
  static const ::google::protobuf::Descriptor* descriptor();
  static const EncodingConfig& default_instance();

  static void InitAsDefaultInstance();  // FOR INTERNAL USE ONLY
  static inline const EncodingConfig* internal_default_instance() {
    return reinterpret_cast<const EncodingConfig*>(
               &_EncodingConfig_default_instance_);
  }
  static PROTOBUF_CONSTEXPR int const kIndexInFileMessages =
    0;

  void Swap(EncodingConfig* other);
  friend void swap(EncodingConfig& a, EncodingConfig& b) {
    a.Swap(&b);
  }

  // implements Message ----------------------------------------------

  inline EncodingConfig* New() const PROTOBUF_FINAL { return New(NULL); }

  EncodingConfig* New(::google::protobuf::Arena* arena) const PROTOBUF_FINAL;
  void CopyFrom(const ::google::protobuf::Message& from) PROTOBUF_FINAL;
  void MergeFrom(const ::google::protobuf::Message& from) PROTOBUF_FINAL;
  void CopyFrom(const EncodingConfig& from);
  void MergeFrom(const EncodingConfig& from);
  void Clear() PROTOBUF_FINAL;
  bool IsInitialized() const PROTOBUF_FINAL;

  size_t ByteSizeLong() const PROTOBUF_FINAL;
  bool MergePartialFromCodedStream(
      ::google::protobuf::io::CodedInputStream* input) PROTOBUF_FINAL;
  void SerializeWithCachedSizes(
      ::google::protobuf::io::CodedOutputStream* output) const PROTOBUF_FINAL;
  ::google::protobuf::uint8* InternalSerializeWithCachedSizesToArray(
      bool deterministic, ::google::protobuf::uint8* target) const PROTOBUF_FINAL;
  int GetCachedSize() const PROTOBUF_FINAL { return _cached_size_; }
  private:
  void SharedCtor();
  void SharedDtor();
  void SetCachedSize(int size) const PROTOBUF_FINAL;
  void InternalSwap(EncodingConfig* other);
  private:
  inline ::google::protobuf::Arena* GetArenaNoVirtual() const {
    return NULL;
  }
  inline void* MaybeArenaPtr() const {
    return NULL;
  }
  public:

  ::google::protobuf::Metadata GetMetadata() const PROTOBUF_FINAL;

  // nested types ----------------------------------------------------

  // accessors -------------------------------------------------------

  // repeated string ciphers = 2;
  int ciphers_size() const;
  void clear_ciphers();
  static const int kCiphersFieldNumber = 2;
  const ::std::string& ciphers(int index) const;
  ::std::string* mutable_ciphers(int index);
  void set_ciphers(int index, const ::std::string& value);
  #if LANG_CXX11
  void set_ciphers(int index, ::std::string&& value);
  #endif
  void set_ciphers(int index, const char* value);
  void set_ciphers(int index, const char* value, size_t size);
  ::std::string* add_ciphers();
  void add_ciphers(const ::std::string& value);
  #if LANG_CXX11
  void add_ciphers(::std::string&& value);
  #endif
  void add_ciphers(const char* value);
  void add_ciphers(const char* value, size_t size);
  const ::google::protobuf::RepeatedPtrField< ::std::string>& ciphers() const;
  ::google::protobuf::RepeatedPtrField< ::std::string>* mutable_ciphers();

  // bytes key = 3;
  void clear_key();
  static const int kKeyFieldNumber = 3;
  const ::std::string& key() const;
  void set_key(const ::std::string& value);
  #if LANG_CXX11
  void set_key(::std::string&& value);
  #endif
  void set_key(const char* value);
  void set_key(const void* value, size_t size);
  ::std::string* mutable_key();
  ::std::string* release_key();
  void set_allocated_key(::std::string* key);

  // string src = 4;
  void clear_src();
  static const int kSrcFieldNumber = 4;
  const ::std::string& src() const;
  void set_src(const ::std::string& value);
  #if LANG_CXX11
  void set_src(::std::string&& value);
  #endif
  void set_src(const char* value);
  void set_src(const char* value, size_t size);
  ::std::string* mutable_src();
  ::std::string* release_src();
  void set_allocated_src(::std::string* src);

  // string dest = 5;
  void clear_dest();
  static const int kDestFieldNumber = 5;
  const ::std::string& dest() const;
  void set_dest(const ::std::string& value);
  #if LANG_CXX11
  void set_dest(::std::string&& value);
  #endif
  void set_dest(const char* value);
  void set_dest(const char* value, size_t size);
  ::std::string* mutable_dest();
  ::std::string* release_dest();
  void set_allocated_dest(::std::string* dest);

  // int32 compression_level = 1;
  void clear_compression_level();
  static const int kCompressionLevelFieldNumber = 1;
  ::google::protobuf::int32 compression_level() const;
  void set_compression_level(::google::protobuf::int32 value);

  // @@protoc_insertion_point(class_scope:sotcore.EncodingConfig)
 private:

  ::google::protobuf::internal::InternalMetadataWithArena _internal_metadata_;
  ::google::protobuf::RepeatedPtrField< ::std::string> ciphers_;
  ::google::protobuf::internal::ArenaStringPtr key_;
  ::google::protobuf::internal::ArenaStringPtr src_;
  ::google::protobuf::internal::ArenaStringPtr dest_;
  ::google::protobuf::int32 compression_level_;
  mutable int _cached_size_;
  friend struct ::protobuf_EncodingConfig_2eproto::TableStruct;
  friend void ::protobuf_EncodingConfig_2eproto::InitDefaultsEncodingConfigImpl();
};
// ===================================================================


// ===================================================================

#ifdef __GNUC__
  #pragma GCC diagnostic push
  #pragma GCC diagnostic ignored "-Wstrict-aliasing"
#endif  // __GNUC__
// EncodingConfig

// int32 compression_level = 1;
inline void EncodingConfig::clear_compression_level() {
  compression_level_ = 0;
}
inline ::google::protobuf::int32 EncodingConfig::compression_level() const {
  // @@protoc_insertion_point(field_get:sotcore.EncodingConfig.compression_level)
  return compression_level_;
}
inline void EncodingConfig::set_compression_level(::google::protobuf::int32 value) {
  
  compression_level_ = value;
  // @@protoc_insertion_point(field_set:sotcore.EncodingConfig.compression_level)
}

// repeated string ciphers = 2;
inline int EncodingConfig::ciphers_size() const {
  return ciphers_.size();
}
inline void EncodingConfig::clear_ciphers() {
  ciphers_.Clear();
}
inline const ::std::string& EncodingConfig::ciphers(int index) const {
  // @@protoc_insertion_point(field_get:sotcore.EncodingConfig.ciphers)
  return ciphers_.Get(index);
}
inline ::std::string* EncodingConfig::mutable_ciphers(int index) {
  // @@protoc_insertion_point(field_mutable:sotcore.EncodingConfig.ciphers)
  return ciphers_.Mutable(index);
}
inline void EncodingConfig::set_ciphers(int index, const ::std::string& value) {
  // @@protoc_insertion_point(field_set:sotcore.EncodingConfig.ciphers)
  ciphers_.Mutable(index)->assign(value);
}
#if LANG_CXX11
inline void EncodingConfig::set_ciphers(int index, ::std::string&& value) {
  // @@protoc_insertion_point(field_set:sotcore.EncodingConfig.ciphers)
  ciphers_.Mutable(index)->assign(std::move(value));
}
#endif
inline void EncodingConfig::set_ciphers(int index, const char* value) {
  GOOGLE_DCHECK(value != NULL);
  ciphers_.Mutable(index)->assign(value);
  // @@protoc_insertion_point(field_set_char:sotcore.EncodingConfig.ciphers)
}
inline void EncodingConfig::set_ciphers(int index, const char* value, size_t size) {
  ciphers_.Mutable(index)->assign(
    reinterpret_cast<const char*>(value), size);
  // @@protoc_insertion_point(field_set_pointer:sotcore.EncodingConfig.ciphers)
}
inline ::std::string* EncodingConfig::add_ciphers() {
  // @@protoc_insertion_point(field_add_mutable:sotcore.EncodingConfig.ciphers)
  return ciphers_.Add();
}
inline void EncodingConfig::add_ciphers(const ::std::string& value) {
  ciphers_.Add()->assign(value);
  // @@protoc_insertion_point(field_add:sotcore.EncodingConfig.ciphers)
}
#if LANG_CXX11
inline void EncodingConfig::add_ciphers(::std::string&& value) {
  ciphers_.Add(std::move(value));
  // @@protoc_insertion_point(field_add:sotcore.EncodingConfig.ciphers)
}
#endif
inline void EncodingConfig::add_ciphers(const char* value) {
  GOOGLE_DCHECK(value != NULL);
  ciphers_.Add()->assign(value);
  // @@protoc_insertion_point(field_add_char:sotcore.EncodingConfig.ciphers)
}
inline void EncodingConfig::add_ciphers(const char* value, size_t size) {
  ciphers_.Add()->assign(reinterpret_cast<const char*>(value), size);
  // @@protoc_insertion_point(field_add_pointer:sotcore.EncodingConfig.ciphers)
}
inline const ::google::protobuf::RepeatedPtrField< ::std::string>&
EncodingConfig::ciphers() const {
  // @@protoc_insertion_point(field_list:sotcore.EncodingConfig.ciphers)
  return ciphers_;
}
inline ::google::protobuf::RepeatedPtrField< ::std::string>*
EncodingConfig::mutable_ciphers() {
  // @@protoc_insertion_point(field_mutable_list:sotcore.EncodingConfig.ciphers)
  return &ciphers_;
}

// bytes key = 3;
inline void EncodingConfig::clear_key() {
  key_.ClearToEmptyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline const ::std::string& EncodingConfig::key() const {
  // @@protoc_insertion_point(field_get:sotcore.EncodingConfig.key)
  return key_.GetNoArena();
}
inline void EncodingConfig::set_key(const ::std::string& value) {
  
  key_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), value);
  // @@protoc_insertion_point(field_set:sotcore.EncodingConfig.key)
}
#if LANG_CXX11
inline void EncodingConfig::set_key(::std::string&& value) {
  
  key_.SetNoArena(
    &::google::protobuf::internal::GetEmptyStringAlreadyInited(), ::std::move(value));
  // @@protoc_insertion_point(field_set_rvalue:sotcore.EncodingConfig.key)
}
#endif
inline void EncodingConfig::set_key(const char* value) {
  GOOGLE_DCHECK(value != NULL);
  
  key_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), ::std::string(value));
  // @@protoc_insertion_point(field_set_char:sotcore.EncodingConfig.key)
}
inline void EncodingConfig::set_key(const void* value, size_t size) {
  
  key_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(),
      ::std::string(reinterpret_cast<const char*>(value), size));
  // @@protoc_insertion_point(field_set_pointer:sotcore.EncodingConfig.key)
}
inline ::std::string* EncodingConfig::mutable_key() {
  
  // @@protoc_insertion_point(field_mutable:sotcore.EncodingConfig.key)
  return key_.MutableNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline ::std::string* EncodingConfig::release_key() {
  // @@protoc_insertion_point(field_release:sotcore.EncodingConfig.key)
  
  return key_.ReleaseNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline void EncodingConfig::set_allocated_key(::std::string* key) {
  if (key != NULL) {
    
  } else {
    
  }
  key_.SetAllocatedNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), key);
  // @@protoc_insertion_point(field_set_allocated:sotcore.EncodingConfig.key)
}

// string src = 4;
inline void EncodingConfig::clear_src() {
  src_.ClearToEmptyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline const ::std::string& EncodingConfig::src() const {
  // @@protoc_insertion_point(field_get:sotcore.EncodingConfig.src)
  return src_.GetNoArena();
}
inline void EncodingConfig::set_src(const ::std::string& value) {
  
  src_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), value);
  // @@protoc_insertion_point(field_set:sotcore.EncodingConfig.src)
}
#if LANG_CXX11
inline void EncodingConfig::set_src(::std::string&& value) {
  
  src_.SetNoArena(
    &::google::protobuf::internal::GetEmptyStringAlreadyInited(), ::std::move(value));
  // @@protoc_insertion_point(field_set_rvalue:sotcore.EncodingConfig.src)
}
#endif
inline void EncodingConfig::set_src(const char* value) {
  GOOGLE_DCHECK(value != NULL);
  
  src_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), ::std::string(value));
  // @@protoc_insertion_point(field_set_char:sotcore.EncodingConfig.src)
}
inline void EncodingConfig::set_src(const char* value, size_t size) {
  
  src_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(),
      ::std::string(reinterpret_cast<const char*>(value), size));
  // @@protoc_insertion_point(field_set_pointer:sotcore.EncodingConfig.src)
}
inline ::std::string* EncodingConfig::mutable_src() {
  
  // @@protoc_insertion_point(field_mutable:sotcore.EncodingConfig.src)
  return src_.MutableNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline ::std::string* EncodingConfig::release_src() {
  // @@protoc_insertion_point(field_release:sotcore.EncodingConfig.src)
  
  return src_.ReleaseNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline void EncodingConfig::set_allocated_src(::std::string* src) {
  if (src != NULL) {
    
  } else {
    
  }
  src_.SetAllocatedNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), src);
  // @@protoc_insertion_point(field_set_allocated:sotcore.EncodingConfig.src)
}

// string dest = 5;
inline void EncodingConfig::clear_dest() {
  dest_.ClearToEmptyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline const ::std::string& EncodingConfig::dest() const {
  // @@protoc_insertion_point(field_get:sotcore.EncodingConfig.dest)
  return dest_.GetNoArena();
}
inline void EncodingConfig::set_dest(const ::std::string& value) {
  
  dest_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), value);
  // @@protoc_insertion_point(field_set:sotcore.EncodingConfig.dest)
}
#if LANG_CXX11
inline void EncodingConfig::set_dest(::std::string&& value) {
  
  dest_.SetNoArena(
    &::google::protobuf::internal::GetEmptyStringAlreadyInited(), ::std::move(value));
  // @@protoc_insertion_point(field_set_rvalue:sotcore.EncodingConfig.dest)
}
#endif
inline void EncodingConfig::set_dest(const char* value) {
  GOOGLE_DCHECK(value != NULL);
  
  dest_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), ::std::string(value));
  // @@protoc_insertion_point(field_set_char:sotcore.EncodingConfig.dest)
}
inline void EncodingConfig::set_dest(const char* value, size_t size) {
  
  dest_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(),
      ::std::string(reinterpret_cast<const char*>(value), size));
  // @@protoc_insertion_point(field_set_pointer:sotcore.EncodingConfig.dest)
}
inline ::std::string* EncodingConfig::mutable_dest() {
  
  // @@protoc_insertion_point(field_mutable:sotcore.EncodingConfig.dest)
  return dest_.MutableNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline ::std::string* EncodingConfig::release_dest() {
  // @@protoc_insertion_point(field_release:sotcore.EncodingConfig.dest)
  
  return dest_.ReleaseNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline void EncodingConfig::set_allocated_dest(::std::string* dest) {
  if (dest != NULL) {
    
  } else {
    
  }
  dest_.SetAllocatedNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), dest);
  // @@protoc_insertion_point(field_set_allocated:sotcore.EncodingConfig.dest)
}

#ifdef __GNUC__
  #pragma GCC diagnostic pop
#endif  // __GNUC__

// @@protoc_insertion_point(namespace_scope)

}  // namespace sotcore

// @@protoc_insertion_point(global_scope)

#endif  // PROTOBUF_EncodingConfig_2eproto__INCLUDED