//using RenObjectsDecoderLib;

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

//int firstTransition = Transitions.GetFirstTransition(data);
//int lastTransition = Transitions.GetLastTransition(data);

//if (firstTransition == -1 || lastTransition == -1)
//{
//    Console.WriteLine("No transitions found.");
//    return;
//}

//Console.WriteLine($"First transition found at offset {firstTransition}");
//Console.WriteLine($"Last transition found at offset {lastTransition}");

//var bytes = data[firstTransition..lastTransition];

//File.WriteAllBytes("transitions.bin", bytes);

//Console.WriteLine("Transitions saved to transitions.bin");