// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: BaseDir.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace CloudSync.Core {

  /// <summary>Holder for reflection information generated from BaseDir.proto</summary>
  public static partial class BaseDirReflection {

    #region Descriptor
    /// <summary>File descriptor for BaseDir.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BaseDirReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg1CYXNlRGlyLnByb3RvEgdzb3Rjb3JlIqMBCgxWZXJzaW9uRW50cnkSDAoE",
            "dXVpZBgBIAEoCRISCgphdHRyaWJ1dGVzGAIgASgFEhQKDGNyZWF0aW9uVGlt",
            "ZRgDIAEoAxIWCg5sYXN0QWNjZXNzVGltZRgEIAEoAxIVCg1sYXN0V3JpdGVU",
            "aW1lGAUgASgDEg4KBkxlbmd0aBgGIAEoBBIQCghjaGVja3N1bRgHIAEoDBIK",
            "CgJpZBgIIAEoCSJoCghEaXJFbnRyeRIjCgR0eXBlGAEgASgOMhUuc290Y29y",
            "ZS5EaXJFbnRyeVR5cGUSJwoIdmVyc2lvbnMYAiADKAsyFS5zb3Rjb3JlLlZl",
            "cnNpb25FbnRyeRIOCgZsYXRlc3QYAyABKAUikAEKB0Jhc2VEaXISLgoHZW50",
            "cmllcxgBIAMoCzIdLnNvdGNvcmUuQmFzZURpci5FbnRyaWVzRW50cnkSEgoK",
            "ZXllY2F0Y2hlchgCIAEoCRpBCgxFbnRyaWVzRW50cnkSCwoDa2V5GAEgASgJ",
            "EiAKBXZhbHVlGAIgASgLMhEuc290Y29yZS5EaXJFbnRyeToCOAEqIQoMRGly",
            "RW50cnlUeXBlEggKBEZJTEUQABIHCgNESVIQAUIRqgIOQ2xvdWRTeW5jLkNv",
            "cmViBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::CloudSync.Core.DirEntryType), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::CloudSync.Core.VersionEntry), global::CloudSync.Core.VersionEntry.Parser, new[]{ "Uuid", "Attributes", "CreationTime", "LastAccessTime", "LastWriteTime", "Length", "Checksum", "Id" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::CloudSync.Core.DirEntry), global::CloudSync.Core.DirEntry.Parser, new[]{ "Type", "Versions", "Latest" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::CloudSync.Core.BaseDir), global::CloudSync.Core.BaseDir.Parser, new[]{ "Entries", "Eyecatcher" }, null, null, new pbr::GeneratedClrTypeInfo[] { null, })
          }));
    }
    #endregion

  }
  #region Enums
  public enum DirEntryType {
    [pbr::OriginalName("FILE")] File = 0,
    [pbr::OriginalName("DIR")] Dir = 1,
  }

  #endregion

  #region Messages
  public sealed partial class VersionEntry : pb::IMessage<VersionEntry> {
    private static readonly pb::MessageParser<VersionEntry> _parser = new pb::MessageParser<VersionEntry>(() => new VersionEntry());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<VersionEntry> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::CloudSync.Core.BaseDirReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VersionEntry() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VersionEntry(VersionEntry other) : this() {
      uuid_ = other.uuid_;
      attributes_ = other.attributes_;
      creationTime_ = other.creationTime_;
      lastAccessTime_ = other.lastAccessTime_;
      lastWriteTime_ = other.lastWriteTime_;
      length_ = other.length_;
      checksum_ = other.checksum_;
      id_ = other.id_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VersionEntry Clone() {
      return new VersionEntry(this);
    }

    /// <summary>Field number for the "uuid" field.</summary>
    public const int UuidFieldNumber = 1;
    private string uuid_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Uuid {
      get { return uuid_; }
      set {
        uuid_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "attributes" field.</summary>
    public const int AttributesFieldNumber = 2;
    private int attributes_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Attributes {
      get { return attributes_; }
      set {
        attributes_ = value;
      }
    }

    /// <summary>Field number for the "creationTime" field.</summary>
    public const int CreationTimeFieldNumber = 3;
    private long creationTime_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long CreationTime {
      get { return creationTime_; }
      set {
        creationTime_ = value;
      }
    }

    /// <summary>Field number for the "lastAccessTime" field.</summary>
    public const int LastAccessTimeFieldNumber = 4;
    private long lastAccessTime_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long LastAccessTime {
      get { return lastAccessTime_; }
      set {
        lastAccessTime_ = value;
      }
    }

    /// <summary>Field number for the "lastWriteTime" field.</summary>
    public const int LastWriteTimeFieldNumber = 5;
    private long lastWriteTime_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long LastWriteTime {
      get { return lastWriteTime_; }
      set {
        lastWriteTime_ = value;
      }
    }

    /// <summary>Field number for the "Length" field.</summary>
    public const int LengthFieldNumber = 6;
    private ulong length_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong Length {
      get { return length_; }
      set {
        length_ = value;
      }
    }

    /// <summary>Field number for the "checksum" field.</summary>
    public const int ChecksumFieldNumber = 7;
    private pb::ByteString checksum_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Checksum {
      get { return checksum_; }
      set {
        checksum_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 8;
    private string id_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Id {
      get { return id_; }
      set {
        id_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as VersionEntry);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(VersionEntry other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Uuid != other.Uuid) return false;
      if (Attributes != other.Attributes) return false;
      if (CreationTime != other.CreationTime) return false;
      if (LastAccessTime != other.LastAccessTime) return false;
      if (LastWriteTime != other.LastWriteTime) return false;
      if (Length != other.Length) return false;
      if (Checksum != other.Checksum) return false;
      if (Id != other.Id) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Uuid.Length != 0) hash ^= Uuid.GetHashCode();
      if (Attributes != 0) hash ^= Attributes.GetHashCode();
      if (CreationTime != 0L) hash ^= CreationTime.GetHashCode();
      if (LastAccessTime != 0L) hash ^= LastAccessTime.GetHashCode();
      if (LastWriteTime != 0L) hash ^= LastWriteTime.GetHashCode();
      if (Length != 0UL) hash ^= Length.GetHashCode();
      if (Checksum.Length != 0) hash ^= Checksum.GetHashCode();
      if (Id.Length != 0) hash ^= Id.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Uuid.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Uuid);
      }
      if (Attributes != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Attributes);
      }
      if (CreationTime != 0L) {
        output.WriteRawTag(24);
        output.WriteInt64(CreationTime);
      }
      if (LastAccessTime != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(LastAccessTime);
      }
      if (LastWriteTime != 0L) {
        output.WriteRawTag(40);
        output.WriteInt64(LastWriteTime);
      }
      if (Length != 0UL) {
        output.WriteRawTag(48);
        output.WriteUInt64(Length);
      }
      if (Checksum.Length != 0) {
        output.WriteRawTag(58);
        output.WriteBytes(Checksum);
      }
      if (Id.Length != 0) {
        output.WriteRawTag(66);
        output.WriteString(Id);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Uuid.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Uuid);
      }
      if (Attributes != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Attributes);
      }
      if (CreationTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(CreationTime);
      }
      if (LastAccessTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(LastAccessTime);
      }
      if (LastWriteTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(LastWriteTime);
      }
      if (Length != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(Length);
      }
      if (Checksum.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Checksum);
      }
      if (Id.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Id);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(VersionEntry other) {
      if (other == null) {
        return;
      }
      if (other.Uuid.Length != 0) {
        Uuid = other.Uuid;
      }
      if (other.Attributes != 0) {
        Attributes = other.Attributes;
      }
      if (other.CreationTime != 0L) {
        CreationTime = other.CreationTime;
      }
      if (other.LastAccessTime != 0L) {
        LastAccessTime = other.LastAccessTime;
      }
      if (other.LastWriteTime != 0L) {
        LastWriteTime = other.LastWriteTime;
      }
      if (other.Length != 0UL) {
        Length = other.Length;
      }
      if (other.Checksum.Length != 0) {
        Checksum = other.Checksum;
      }
      if (other.Id.Length != 0) {
        Id = other.Id;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Uuid = input.ReadString();
            break;
          }
          case 16: {
            Attributes = input.ReadInt32();
            break;
          }
          case 24: {
            CreationTime = input.ReadInt64();
            break;
          }
          case 32: {
            LastAccessTime = input.ReadInt64();
            break;
          }
          case 40: {
            LastWriteTime = input.ReadInt64();
            break;
          }
          case 48: {
            Length = input.ReadUInt64();
            break;
          }
          case 58: {
            Checksum = input.ReadBytes();
            break;
          }
          case 66: {
            Id = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class DirEntry : pb::IMessage<DirEntry> {
    private static readonly pb::MessageParser<DirEntry> _parser = new pb::MessageParser<DirEntry>(() => new DirEntry());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DirEntry> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::CloudSync.Core.BaseDirReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DirEntry() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DirEntry(DirEntry other) : this() {
      type_ = other.type_;
      versions_ = other.versions_.Clone();
      latest_ = other.latest_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DirEntry Clone() {
      return new DirEntry(this);
    }

    /// <summary>Field number for the "type" field.</summary>
    public const int TypeFieldNumber = 1;
    private global::CloudSync.Core.DirEntryType type_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::CloudSync.Core.DirEntryType Type {
      get { return type_; }
      set {
        type_ = value;
      }
    }

    /// <summary>Field number for the "versions" field.</summary>
    public const int VersionsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::CloudSync.Core.VersionEntry> _repeated_versions_codec
        = pb::FieldCodec.ForMessage(18, global::CloudSync.Core.VersionEntry.Parser);
    private readonly pbc::RepeatedField<global::CloudSync.Core.VersionEntry> versions_ = new pbc::RepeatedField<global::CloudSync.Core.VersionEntry>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::CloudSync.Core.VersionEntry> Versions {
      get { return versions_; }
    }

    /// <summary>Field number for the "latest" field.</summary>
    public const int LatestFieldNumber = 3;
    private int latest_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Latest {
      get { return latest_; }
      set {
        latest_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DirEntry);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DirEntry other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Type != other.Type) return false;
      if(!versions_.Equals(other.versions_)) return false;
      if (Latest != other.Latest) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Type != 0) hash ^= Type.GetHashCode();
      hash ^= versions_.GetHashCode();
      if (Latest != 0) hash ^= Latest.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Type != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Type);
      }
      versions_.WriteTo(output, _repeated_versions_codec);
      if (Latest != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Latest);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Type != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Type);
      }
      size += versions_.CalculateSize(_repeated_versions_codec);
      if (Latest != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Latest);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DirEntry other) {
      if (other == null) {
        return;
      }
      if (other.Type != 0) {
        Type = other.Type;
      }
      versions_.Add(other.versions_);
      if (other.Latest != 0) {
        Latest = other.Latest;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            type_ = (global::CloudSync.Core.DirEntryType) input.ReadEnum();
            break;
          }
          case 18: {
            versions_.AddEntriesFrom(input, _repeated_versions_codec);
            break;
          }
          case 24: {
            Latest = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class BaseDir : pb::IMessage<BaseDir> {
    private static readonly pb::MessageParser<BaseDir> _parser = new pb::MessageParser<BaseDir>(() => new BaseDir());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<BaseDir> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::CloudSync.Core.BaseDirReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public BaseDir() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public BaseDir(BaseDir other) : this() {
      entries_ = other.entries_.Clone();
      eyecatcher_ = other.eyecatcher_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public BaseDir Clone() {
      return new BaseDir(this);
    }

    /// <summary>Field number for the "entries" field.</summary>
    public const int EntriesFieldNumber = 1;
    private static readonly pbc::MapField<string, global::CloudSync.Core.DirEntry>.Codec _map_entries_codec
        = new pbc::MapField<string, global::CloudSync.Core.DirEntry>.Codec(pb::FieldCodec.ForString(10), pb::FieldCodec.ForMessage(18, global::CloudSync.Core.DirEntry.Parser), 10);
    private readonly pbc::MapField<string, global::CloudSync.Core.DirEntry> entries_ = new pbc::MapField<string, global::CloudSync.Core.DirEntry>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::MapField<string, global::CloudSync.Core.DirEntry> Entries {
      get { return entries_; }
    }

    /// <summary>Field number for the "eyecatcher" field.</summary>
    public const int EyecatcherFieldNumber = 2;
    private string eyecatcher_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Eyecatcher {
      get { return eyecatcher_; }
      set {
        eyecatcher_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as BaseDir);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(BaseDir other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!Entries.Equals(other.Entries)) return false;
      if (Eyecatcher != other.Eyecatcher) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= Entries.GetHashCode();
      if (Eyecatcher.Length != 0) hash ^= Eyecatcher.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      entries_.WriteTo(output, _map_entries_codec);
      if (Eyecatcher.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Eyecatcher);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += entries_.CalculateSize(_map_entries_codec);
      if (Eyecatcher.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Eyecatcher);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(BaseDir other) {
      if (other == null) {
        return;
      }
      entries_.Add(other.entries_);
      if (other.Eyecatcher.Length != 0) {
        Eyecatcher = other.Eyecatcher;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            entries_.AddEntriesFrom(input, _map_entries_codec);
            break;
          }
          case 18: {
            Eyecatcher = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
