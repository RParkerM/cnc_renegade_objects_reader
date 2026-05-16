//using System.Data;
//using System.IO;
//using System.Numerics;
//using ObjectsDdbDecoding;
//using ObjectsReader;
//using ReneGamesData.Model;

//string filename = "C:\\Westwood\\tools\\leveledit\\Renegade\\presets\\objects.ddb";

//byte[] data;

//try
//{
//    data = File.ReadAllBytes(filename);
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"Error reading file: {ex.Message}");
//    return;
//}

//Dictionary<string, List<int>> ObjectTransitions = Objects.GetObjectTransitions(data);

//Console.WriteLine($"Transitions found for {ObjectTransitions.First().Key}: {ObjectTransitions.First().Value.Count}");
//foreach (var transitionLocation in ObjectTransitions.First().Value)
//{
//    var transition = new TransitionDefinition(data, transitionLocation);
//    Console.WriteLine($"Transition Type: {transition.TransitionType}");
//    Console.WriteLine($"Zone Basis:\n {transition.TriggerZone.Basis.Row[0]}\n {transition.TriggerZone.Basis.Row[1]}\n {transition.TriggerZone.Basis.Row[2]}");
//    Console.WriteLine($"Zone Center: {transition.TriggerZone.Center}");
//    Console.WriteLine($"Zone Extent: {transition.TriggerZone.Extent}");
//    Console.WriteLine($"Character Transform: {transition.CharacterTransform.Row[0]} {transition.CharacterTransform.Row[1]} {transition.CharacterTransform.Row[2]}");
//    Console.WriteLine($"Animation Name: {transition.AnimationName}\n");
//}
////int offset = 0;

////var renObject = Objects.GetNextObject(data, offset);

////while (true)
////{
////    if (renObject == -1)
////    {
////        break;
////    }

////    var objectNameOffset = Objects.GetObjectName(data, renObject, out string objectName);

////    ByteSequence.SequenceType sequenceType = ByteSequence.FindNextRenSequence(data, objectNameOffset, out int nextOffset);
////    while (true)
////    {
////        if (sequenceType == ByteSequence.SequenceType.Transition)
////        {
////            if (!ObjectTransitions.TryGetValue(objectName, out List<int>? value))
////            {
////                value = ([]);
////                ObjectTransitions[objectName] = value;
////            }

////            value.Add(nextOffset);
////            sequenceType = ByteSequence.FindNextRenSequence(data, nextOffset + 1, out nextOffset);
////        }
////        else if (sequenceType == ByteSequence.SequenceType.ObjectHeader)
////        {
////            renObject = nextOffset;
////            break;
////        }
////        else if (sequenceType == ByteSequence.SequenceType.Unknown)
////        {
////            renObject = -1;
////            break;
////        }
////    }
////    if (ObjectTransitions.TryGetValue(objectName, out List<int>? transitions))
////    {
////        Console.WriteLine($"Transitions found for {objectName}: {transitions.Count}");
////    }
////}



////var transitionLocation = ObjectTransitions.First().Value.First();

////var filename = "C:\\Development\\Ren\\ObjectsEditing\\transitions.ddb";

////var filestream = new FileStream(filename, FileMode.Open, FileAccess.Read);

////var file = new BinaryReader(filestream);

////for (int i = 0; i < 8; i++)
////{

////    var transitionType = (TransitionType)file.ReadUInt32();

////    Console.WriteLine($"Transition Type: {transitionType}");

////    var randomBytes = file.ReadBytes(2); // unknown
////    Console.WriteLine($"Unknown: {BitConverter.ToString(randomBytes)}");

////    var zone = new OBBoxClass
////    {
////        Basis = new Matrix3()
////        {
////            Row = new FixedArray3<Vector3>(

////                new Vector3(file.ReadSingle(), file.ReadSingle(), file.ReadSingle()),
////                new Vector3(file.ReadSingle(), file.ReadSingle(), file.ReadSingle()),
////                new Vector3(file.ReadSingle(), file.ReadSingle(), file.ReadSingle())
////            )
////        },
////        Center = new Vector3(file.ReadSingle(), file.ReadSingle(), file.ReadSingle()),
////        Extent = new Vector3(file.ReadSingle(), file.ReadSingle(), file.ReadSingle())
////    };

////    Console.WriteLine($"Zone Basis:\n {zone.Basis.Row[0]}\n {zone.Basis.Row[1]}\n {zone.Basis.Row[2]}");
////    Console.WriteLine($"Zone Center: {zone.Center}");
////    Console.WriteLine($"Zone Extent: {zone.Extent}");

////    randomBytes = file.ReadBytes(2);
////    Console.WriteLine($"Unknown: {BitConverter.ToString(randomBytes)}");

////    var CharacterTransform = new Matrix3D
////    {
////        Row = new FixedArray3<Vector4>(

////                new Vector4(file.ReadSingle(), file.ReadSingle(), file.ReadSingle(), file.ReadSingle()),
////                new Vector4(file.ReadSingle(), file.ReadSingle(), file.ReadSingle(), file.ReadSingle()),
////                new Vector4(file.ReadSingle(), file.ReadSingle(), file.ReadSingle(), file.ReadSingle())
////            )
////    };

////    Console.WriteLine($"Character Transform: {CharacterTransform.Row[0]} {CharacterTransform.Row[1]} {CharacterTransform.Row[2]}");

////    randomBytes = file.ReadBytes(1);

////    Console.WriteLine($"Unknown: {BitConverter.ToString(randomBytes)}");
////    var animationNameLength = (uint)file.ReadByte();
////    Console.WriteLine($"Length of Animation Name: {animationNameLength}");
////    var animationName = new string(file.ReadChars((int)animationNameLength));



////    Console.WriteLine($"Animation Name: {animationName}");

////    int randomByteCount = 18;
////    randomBytes = file.ReadBytes(randomByteCount);

////    Console.WriteLine($"Random Bytes: {BitConverter.ToString(randomBytes)}");
////}

