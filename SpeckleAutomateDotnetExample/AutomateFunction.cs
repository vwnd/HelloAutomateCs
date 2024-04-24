using Objects;
using Objects.Geometry;
using Speckle.Automate.Sdk;
using Speckle.Core.Logging;
using Speckle.Core.Models.Extensions;

public static class AutomateFunction
{
  public static async Task Run(
    AutomationContext automationContext,
    FunctionInputs functionInputs
  )
  {
    Console.WriteLine("Starting execution");
    _ = typeof(ObjectsKit).Assembly; // INFO: Force objects kit to initialize

    Console.WriteLine("Receiving version");
    var commitObject = await automationContext.ReceiveVersion();

    Console.WriteLine("Received version: " + commitObject);

    var count = commitObject
      .Flatten()
      .Count(b => b.speckle_type == functionInputs.SpeckleTypeToCount);

    // Filter the flattened objects for "Room" types specifically
    var rooms = commitObject
        .Flatten()
        .Where(b => b.speckle_type.Contains("Room")) // Assuming "Room" is the correct speckle_type
        .ToList();

    int roomCount = rooms.Count;

    Console.WriteLine($"Counted {roomCount} rooms");

    // Write the number of rooms to a text file
    File.WriteAllText("report.txt", $"Counted {roomCount} rooms");

    Console.WriteLine($"Counted {count} objects");
    await automationContext.StoreFileResult("report.txt");
    automationContext.MarkRunSuccess($"Counted {count} objects");
  }
}
