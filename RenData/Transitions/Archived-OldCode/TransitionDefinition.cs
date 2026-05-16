//using System.Numerics;
//using System.Text;

//namespace ObjectsReader;

//public class TransitionDefinition
//{
//    #region Constants
//    public const int TransitionChunkHeader = 0x377DCE2A;

//    public const byte TypeMicroChunkHeader = 0x01;
//    public const byte TypeMicroChunkLength = 0x04;

//    public const byte TriggerZoneMicroChunkHeader = 0x02;
//    public const byte TriggerZoneMicroChunkLength = 0x3C;

//    public const byte TransformMicroChunkHeader = 0x04;
//    public const byte TransformMicroChunkLength = 0x30;

//    public const byte AnimationMicroChunkHeader = 0x03;
//    #endregion

//    public readonly int DataOffset;

//    public readonly int TransitionLength;
//    public readonly TransitionType Type;
//    public readonly OBBoxClass TriggerZone;
//    public readonly string AnimationName;
//    public readonly Matrix3D CharacterTransform;

//    public readonly byte[] RandomBytes;

//    public TransitionDefinition(byte[] transitionData, int offset)
//    {
//        using var memoryStream = new MemoryStream(transitionData[offset..]);
//        using var reader = new BinaryReader(memoryStream);

//        int header = reader.ReadInt32();
//        if (header != TransitionChunkHeader)
//        {
//            throw new InvalidDataException($"Invalid transition data header: {header:X2}");
//        }
//        TransitionLength = reader.ReadUInt16();
//        RandomBytes = reader.ReadBytes(10);

//        byte transitionTypeChunkHeader = reader.ReadByte();
//        if (transitionTypeChunkHeader != TypeMicroChunkHeader)
//        {
//            throw new InvalidDataException($"Invalid transition type chunk header: {transitionTypeChunkHeader:X2}");
//        }
//        byte transitionTypeLength = reader.ReadByte();
//        if (transitionTypeLength != TypeMicroChunkLength)
//        {
//            throw new InvalidDataException($"Invalid transition type length: {transitionTypeLength:X2}");
//        }
//        Type = (TransitionType)reader.ReadInt32();

//        byte triggerZoneChunkHeader = reader.ReadByte();
//        if (triggerZoneChunkHeader != TriggerZoneMicroChunkHeader)
//        {
//            throw new InvalidDataException($"Invalid trigger zone chunk header: {triggerZoneChunkHeader:X2}");
//        }
//        byte triggerZoneLength = reader.ReadByte();
//        if (triggerZoneLength != TriggerZoneMicroChunkLength)
//        {
//            throw new InvalidDataException($"Invalid trigger zone length: {triggerZoneLength:X2}");
//        }

//        TriggerZone = new OBBoxClass()
//        {
//            Basis = new Matrix3()
//            {
//                Row = new FixedArray3<Vector3>(
//                    new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
//                    new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
//                    new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle())
//                )
//            },
//            Center = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
//            Extent = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle())
//        };

//        byte transformChunkHeader = reader.ReadByte();
//        if (transformChunkHeader != TransformMicroChunkHeader)
//        {
//            throw new InvalidDataException($"Invalid transform chunk header: {transformChunkHeader:X2}");
//        }
//        byte transformLength = reader.ReadByte();
//        if (transformLength != TransformMicroChunkLength)
//        {
//            throw new InvalidDataException($"Invalid transform length: {transformLength:X2}");
//        }

//        CharacterTransform = new()
//        {
//            Row = new FixedArray3<Vector4>(
//                new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
//                new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
//                new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle())
//            )
//        };

//        byte animationChunkHeader = reader.ReadByte();
//        if (animationChunkHeader != AnimationMicroChunkHeader)
//        {
//            throw new InvalidDataException($"Invalid animation chunk header: {animationChunkHeader:X2}");
//        }
//        byte animationNameLength = reader.ReadByte();
//        AnimationName = Encoding.ASCII.GetString(reader.ReadBytes(animationNameLength - 1));
//    }

//    public void Write(byte[] data)
//    {
//        using var memoryStream = new MemoryStream(data, DataOffset, TransitionLength);
//        using var writer = new BinaryWriter(memoryStream);
//        writer.Write(TransitionChunkHeader);
//        writer.Write((ushort)TransitionLength);
//        writer.Write(RandomBytes);
//        writer.Write(TypeMicroChunkHeader);
//        writer.Write(TypeMicroChunkLength);
//        writer.Write((int)Type);
//        writer.Write(TriggerZoneMicroChunkHeader);
//        writer.Write(TriggerZoneMicroChunkLength);
//        writer.Write(TriggerZone.Basis.Row[0].X);
//        writer.Write(TriggerZone.Basis.Row[0].Y);
//        writer.Write(TriggerZone.Basis.Row[0].Z);
//        writer.Write(TriggerZone.Basis.Row[1].X);
//        writer.Write(TriggerZone.Basis.Row[1].Y);
//        writer.Write(TriggerZone.Basis.Row[1].Z);
//        writer.Write(TriggerZone.Basis.Row[2].X);
//        writer.Write(TriggerZone.Basis.Row[2].Y);
//        writer.Write(TriggerZone.Basis.Row[2].Z);
//        writer.Write(TriggerZone.Center.X);
//        writer.Write(TriggerZone.Center.Y);
//        writer.Write(TriggerZone.Center.Z);
//        writer.Write(TriggerZone.Extent.X);
//        writer.Write(TriggerZone.Extent.Y);
//        writer.Write(TriggerZone.Extent.Z);
//        writer.Write(TransformMicroChunkHeader);
//        writer.Write(TransformMicroChunkLength);
//        writer.Write(CharacterTransform.Row[0].X);
//        writer.Write(CharacterTransform.Row[0].Y);
//        writer.Write(CharacterTransform.Row[0].Z);
//        writer.Write(CharacterTransform.Row[0].W);
//        writer.Write(CharacterTransform.Row[1].X);
//        writer.Write(CharacterTransform.Row[1].Y);
//        writer.Write(CharacterTransform.Row[1].Z);
//        writer.Write(CharacterTransform.Row[1].W);
//        writer.Write(CharacterTransform.Row[2].X);
//        writer.Write(CharacterTransform.Row[2].Y);
//        writer.Write(CharacterTransform.Row[2].Z);
//        writer.Write(CharacterTransform.Row[2].W);
//        writer.Write(AnimationMicroChunkHeader);
//        writer.Write((byte)(AnimationName.Length+1));
//        writer.Write(Encoding.ASCII.GetBytes(AnimationName));
//    }
//}
