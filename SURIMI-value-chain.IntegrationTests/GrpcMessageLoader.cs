using Google.Protobuf;
using YamlDotNet.RepresentationModel;

namespace SURIMI_value_chain_IntegrationTests;

/// <summary>
/// Loads a gRPC message from a Postman request YAML file in the postman\collections directory.
/// The message JSON is read from the <c>message.content</c> node; an absent message yields an empty message.
/// </summary>
public static class GrpcMessageLoader
{
    private static readonly string s_collectionsPath =
        Path.Combine(AppContext.BaseDirectory, "postman", "collections");

    public static T Load<T>(string subFolder, string fileName) where T : IMessage<T>, new()
    {
        string filePath = Path.Combine(s_collectionsPath, subFolder, fileName);

        YamlStream yaml = new YamlStream();
        using (StreamReader reader = new StreamReader(filePath))
        {
            yaml.Load(reader);
        }

        YamlMappingNode root = (YamlMappingNode)yaml.Documents[0].RootNode;
        if (!root.Children.TryGetValue(new YamlScalarNode("message"), out YamlNode? messageNode) ||
            messageNode is not YamlMappingNode message ||
            !message.Children.TryGetValue(new YamlScalarNode("content"), out YamlNode? contentNode) ||
            contentNode is not YamlScalarNode { Value: { Length: > 0 } json })
        {
            return new T();
        }

        JsonParser parser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
        return parser.Parse<T>(json);    }
}

