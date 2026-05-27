using Google.Protobuf;

namespace SURIMI_value_chain_IntegrationTests;

/// <summary>
/// Loads a gRPC message from a JSON fixture file in the GrpcMessages directory.
/// </summary>
public static class GrpcMessageLoader
{
    private static readonly string s_grpcMessagesPath =
        Path.Combine(AppContext.BaseDirectory, "GrpcMessages");

    public static T Load<T>(string subFolder, string fileName) where T : IMessage<T>, new()
    {
        string filePath = Path.Combine(s_grpcMessagesPath, subFolder, fileName);
        string json = File.ReadAllText(filePath);
        JsonParser parser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
        return parser.Parse<T>(json);
    }
}
