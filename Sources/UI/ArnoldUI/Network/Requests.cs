// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Requests.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace GoodAI.Arnold.Net {

  /// <summary>Holder for reflection information generated from Requests.proto</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public static partial class RequestsReflection {

    #region Descriptor
    /// <summary>File descriptor for Requests.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static RequestsReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5SZXF1ZXN0cy5wcm90bxIRR29vZEFJLkFybm9sZC5OZXQifwoOQ29tbWFu",
            "ZFJlcXVlc3QSPgoHQ29tbWFuZBgBIAEoDjItLkdvb2RBSS5Bcm5vbGQuTmV0",
            "LkNvbW1hbmRSZXF1ZXN0LkNvbW1hbmRUeXBlIi0KC0NvbW1hbmRUeXBlEgkK",
            "BVN0YXJ0EAASCQoFUGF1c2UQARIICgRTdG9wEAIiEQoPR2V0U3RhdGVSZXF1",
            "ZXN0YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedCodeInfo(null, new pbr::GeneratedCodeInfo[] {
            new pbr::GeneratedCodeInfo(typeof(global::GoodAI.Arnold.Net.CommandRequest), global::GoodAI.Arnold.Net.CommandRequest.Parser, new[]{ "Command" }, null, new[]{ typeof(global::GoodAI.Arnold.Net.CommandRequest.Types.CommandType) }, null),
            new pbr::GeneratedCodeInfo(typeof(global::GoodAI.Arnold.Net.GetStateRequest), global::GoodAI.Arnold.Net.GetStateRequest.Parser, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public sealed partial class CommandRequest : pb::IMessage<CommandRequest> {
    private static readonly pb::MessageParser<CommandRequest> _parser = new pb::MessageParser<CommandRequest>(() => new CommandRequest());
    public static pb::MessageParser<CommandRequest> Parser { get { return _parser; } }

    public static pbr::MessageDescriptor Descriptor {
      get { return global::GoodAI.Arnold.Net.RequestsReflection.Descriptor.MessageTypes[0]; }
    }

    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    public CommandRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    public CommandRequest(CommandRequest other) : this() {
      command_ = other.command_;
    }

    public CommandRequest Clone() {
      return new CommandRequest(this);
    }

    /// <summary>Field number for the "Command" field.</summary>
    public const int CommandFieldNumber = 1;
    private global::GoodAI.Arnold.Net.CommandRequest.Types.CommandType command_ = global::GoodAI.Arnold.Net.CommandRequest.Types.CommandType.Start;
    public global::GoodAI.Arnold.Net.CommandRequest.Types.CommandType Command {
      get { return command_; }
      set {
        command_ = value;
      }
    }

    public override bool Equals(object other) {
      return Equals(other as CommandRequest);
    }

    public bool Equals(CommandRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Command != other.Command) return false;
      return true;
    }

    public override int GetHashCode() {
      int hash = 1;
      if (Command != global::GoodAI.Arnold.Net.CommandRequest.Types.CommandType.Start) hash ^= Command.GetHashCode();
      return hash;
    }

    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (Command != global::GoodAI.Arnold.Net.CommandRequest.Types.CommandType.Start) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Command);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (Command != global::GoodAI.Arnold.Net.CommandRequest.Types.CommandType.Start) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Command);
      }
      return size;
    }

    public void MergeFrom(CommandRequest other) {
      if (other == null) {
        return;
      }
      if (other.Command != global::GoodAI.Arnold.Net.CommandRequest.Types.CommandType.Start) {
        Command = other.Command;
      }
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            command_ = (global::GoodAI.Arnold.Net.CommandRequest.Types.CommandType) input.ReadEnum();
            break;
          }
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the CommandRequest message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public static partial class Types {
      public enum CommandType {
        Start = 0,
        Pause = 1,
        Stop = 2,
      }

    }
    #endregion

  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public sealed partial class GetStateRequest : pb::IMessage<GetStateRequest> {
    private static readonly pb::MessageParser<GetStateRequest> _parser = new pb::MessageParser<GetStateRequest>(() => new GetStateRequest());
    public static pb::MessageParser<GetStateRequest> Parser { get { return _parser; } }

    public static pbr::MessageDescriptor Descriptor {
      get { return global::GoodAI.Arnold.Net.RequestsReflection.Descriptor.MessageTypes[1]; }
    }

    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    public GetStateRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    public GetStateRequest(GetStateRequest other) : this() {
    }

    public GetStateRequest Clone() {
      return new GetStateRequest(this);
    }

    public override bool Equals(object other) {
      return Equals(other as GetStateRequest);
    }

    public bool Equals(GetStateRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      return true;
    }

    public override int GetHashCode() {
      int hash = 1;
      return hash;
    }

    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    public void WriteTo(pb::CodedOutputStream output) {
    }

    public int CalculateSize() {
      int size = 0;
      return size;
    }

    public void MergeFrom(GetStateRequest other) {
      if (other == null) {
        return;
      }
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
