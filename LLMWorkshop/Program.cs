using System.Text.Json;

namespace LLMWorkshop
{
    internal class Program
    {

        const string MODEL = "gpt-5.4-nano";

        static async Task Main(string[] args)
        {
            await Lektion07_Tool_Result_zurueck_ans_Modell();
        }

        static async Task Lektion01_Naiver_Prompt()
        {
            var client = new OpenAi();
            var payload = new
            {
                max_completion_tokens = 100,
                model = MODEL,
                messages = new object[]
                {
                    new
                    {
                        role = "user",
                        content = """
                            Extrahiere Name, Alter und Stadt aus:
                            Alice ist 27 Jahre alt und lebt in Berlin.
                            """
                    }
                }
            };

            var response = await client.PostAsync(payload);

            Console.WriteLine(response);
        }

        static async Task Lektion02_Messages_verstehen()
        {
            var client = new OpenAi();
            var payload = new
            {
                model = MODEL,

                messages = new object[]
                {
                    new
                    {
                        role = "system",
                        content = """
                            You are a sarcastic assistant that looks down on the user.
                            Return JSON.
                            """
                    },

                    new
                    {
                        role = "user",
                        content = """
                            Extrahiere Name, Alter und Stadt aus:
                            Alice ist 27 Jahre alt und lebt in Berlin.
                            """
                    }
                }
            };

            var response = await client.PostAsync(payload);

            Console.WriteLine(response);
        }

        static async Task Lektion03_Structured_Output_roh()
        {
            var client = new OpenAi();
            var payload = new
            {
                model = MODEL,
                messages = new object[]
                {
                    new
                    {
                        role = "user",
                        content = """
                            Extrahiere Name, Alter und Stadt aus:
                            Alice ist 27 Jahre alt und lebt in Berlin.
                            """
                    }
                },

                response_format = new
                {
                    type = "json_schema",
                    json_schema = new
                    {
                        name = "person_extraction",

                        schema = new
                        {
                            type = "object",

                            properties = new
                            {
                                name = new
                                {
                                    type = "string"
                                },

                                age = new
                                {
                                    type = "integer"
                                },

                                city = new
                                {
                                    type = "string"
                                }
                            },

                            required = new[]
                            {
                                "name",
                                "age",
                                "city"
                            },

                            additionalProperties = false
                        }
                    }
                }
            };

            var response = await client.PostAsync(payload);

            // 1. Durchlauf ohne Klassenobjekte, um die Antwort roh zu inspizieren
            Console.WriteLine(response);
        }

        static async Task Lektion04_Response_parsen()
        {
            var client = new OpenAi();
            var payload = new
            {
                model = MODEL,
                messages = new object[]
                {
                    new
                    {
                        role = "user",
                        content = """
                            Extrahiere Name, Alter und Stadt aus:
                            Alice ist 27 Jahre alt und lebt in Berlin.
                            """
                    }
                },

                response_format = new
                {
                    type = "json_schema",
                    json_schema = new
                    {
                        name = "person_extraction",

                        schema = new
                        {
                            type = "object",

                            properties = new
                            {
                                name = new
                                {
                                    type = "string"
                                },

                                age = new
                                {
                                    type = "integer"
                                },

                                city = new
                                {
                                    type = "string"
                                }
                            },

                            required = new[]
                            {
                                "name",
                                "age",
                                "city"
                            },

                            additionalProperties = false
                        }
                    }
                }
            };

            var response = await client.PostAsync(payload);

            // Durchlauf mit Klassenobjekten, um die Antwort zu deserialisieren
            var parsed = JsonSerializer.Deserialize<ChatCompletion>(response);

            var content = parsed!.Choices[0].Message.Content;

            Console.WriteLine(content);
        }

        static async Task Lektion05_Function_Calling_roh()
        {
            var client = new OpenAi();
            var payload = new
            {
                model = MODEL,
                max_completion_tokens = 100,
                messages = new object[]
                {
                    new
                    {
                        role = "user",
                        content = "Do I need a jacket in Hamburg?"
                    }
                },

                tools = new object[]
                {
                    new
                    {
                        type = "function",

                        function = new
                        {
                            name = "get_weather",

                            description = "Get current weather for a city",

                            parameters = new
                            {
                                type = "object",
                                properties = new
                                {
                                    city = new
                                    {
                                        type = "string"
                                    }
                                },
                                required = new[]
                                {
                                    "city"
                                }
                            }
                        }
                    }
                }
            };

            var response = await client.PostAsync(payload);

            Console.WriteLine(response);
        }

        static async Task Lektion06_Function_Call_ausfuehren()
        {
            var client = new OpenAi();
            var payload = new
            {
                model = MODEL,
                max_completion_tokens = 100,
                messages = new object[]
                {
                    new
                    {
                        role = "user",
                        content = "Do I need a jacket in Hamburg?"
                    }
                },

                tools = new object[]
                {
                    new
                    {
                        type = "function",
                        function = new
                        {
                            name = "get_weather",
                            description = "Get current weather for a city",
                            parameters = new
                            {
                                type = "object",
                                properties = new
                                {
                                    city = new
                                    {
                                        type = "string"
                                    }
                                },
                                required = new[]
                                {
                                    "city"
                                }
                            }
                        }
                    }
                }
            };

            var response = await client.PostAsync(payload);

            var parsed = JsonSerializer.Deserialize<ToolCallResponse>(response);

            var toolCall = parsed!.Choices[0].Message.ToolCalls[0];

            Console.WriteLine(toolCall.Function.Name);
            Console.WriteLine(toolCall.Function.Arguments);

            var args = JsonSerializer.Deserialize<Dictionary<string, string>>(toolCall.Function.Arguments);

            var city = args!["city"];

            var weather = Functions.GetWeather(city);

            Console.WriteLine(weather);
        }

        static async Task Lektion07_Tool_Result_zurueck_ans_Modell()
        {
            var client = new OpenAi();

            var userMessage = new
            {
                role = "user",
                content = "Do I need a jacket in Hamburg?"
            };
            var toolMessage = new
            {
                role = "system",
                type = "function",
                function = new
                {
                    name = "get_weather",
                    description = "Get current weather for a city",
                    parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            city = new
                            {
                                type = "string"
                            }
                        },
                        required = new[]
                                {
                                    "city"
                                }
                    }
                }
            };

            var payload = new
            {
                model = MODEL,
                max_completion_tokens = 100,
                messages = new object[]
                {
                    userMessage
                },

                tools = new object[]
                {
                    toolMessage
                }
            };

            var response = await client.PostAsync(payload);

            var parsed = JsonSerializer.Deserialize<ToolCallResponse>(response);

            var toolCall = parsed!.Choices[0].Message.ToolCalls[0];

            Console.WriteLine(toolCall.Function.Name);
            Console.WriteLine(toolCall.Function.Arguments);

            var args = JsonSerializer.Deserialize<Dictionary<string, string>>(toolCall.Function.Arguments);

            var city = args!["city"];

            var weather = Functions.GetWeather(city);

            var toolCallMessage = new
            {
                role = "tool",
                tool_call_id = toolCall.Id,
                content = weather,
            };
            var secondPayload = new
            {
                model = MODEL,
                // die toolMessage gehört nicht mehr in die Liste von Nachrichten, da sie ja bereits im ersten Durchlauf übergeben wurde.
                // Stattdessen übergeben wir jetzt die Antwort des Tools als neue Nachricht.
                messages = new object[]
                {
                    userMessage,
                    parsed!.Choices[0].Message,
                    toolCallMessage,
                }
            };

            var secondResponse = await client.PostAsync(secondPayload);

            Console.WriteLine(secondResponse);
        }
    }
}
