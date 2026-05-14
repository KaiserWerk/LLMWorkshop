using System.Text.Json.Serialization;

namespace LLMWorkshop;

public class ChatCompletion
{
    [JsonPropertyName("choices")]
    public Choice[] Choices { get; set; } = [];
}

public class Choice
{
    [JsonPropertyName("message")]
    public Message Message { get; set; } = new();
}

public class Message
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = "";
}


public class ToolCallResponse
{
    [JsonPropertyName("choices")]
    public ToolChoice[] Choices { get; set; } = [];
}

public class ToolChoice
{
    [JsonPropertyName("message")]
    public ToolMessage Message { get; set; } = new();
}

public class ToolMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("tool_calls")]
    public ToolCall[] ToolCalls { get; set; } = [];
}

public class ToolCall
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("function")]
    public FunctionCall Function { get; set; } = new();
}

public class FunctionCall
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("arguments")]
    public string Arguments { get; set; } = "";
}