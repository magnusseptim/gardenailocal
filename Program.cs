using Microsoft.SemanticKernel;
using GardenaiLocal.Agents;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.ChatCompletion;
using GardenaiLocal;

class Program
{
    private static async Task<string> DetectLanguageAsync(IChatCompletionService chat, string input)
    {
        var history = new ChatHistory();
        history.AddSystemMessage(
            "Detect the language of the following text. Respond ONLY with the language's English name (for example: Polish, English, French, German, etc.) and nothing else. Do NOT return sentences or explanations. If the text is Polish, respond with 'Polish'.");
        history.AddUserMessage(input);
        var result = await chat.GetChatMessageContentAsync(history);
        return result.Content?.Trim() ?? string.Empty;
    }

    static async Task<string> TranslateAsync(IChatCompletionService chat, string text, string targetLanguage)
    {
        var history = new ChatHistory();
        history.AddSystemMessage(
            $"Translate the following text to {targetLanguage}. Output ONLY the translation, no explanations or comments.");
        history.AddUserMessage(text);
        var result = await chat.GetChatMessageContentAsync(history);
        return result.Content?.Trim() ?? string.Empty;
    }

    static async Task Main(string[] args)
    {
        bool verboseMode = args.Any(arg => arg.Equals("--verbose", StringComparison.OrdinalIgnoreCase)
                                        || arg.Equals("--debug", StringComparison.OrdinalIgnoreCase));

        var kernelLlama = KernelBuilderFactory.Build("llama3");
        var kernelMistral = KernelBuilderFactory.Build("mistral");

        var llamaChat = kernelLlama.GetRequiredService<IChatCompletionService>();
        var mistralChat = kernelMistral.GetRequiredService<IChatCompletionService>();

        var retriever = new RetrieverAgent(llamaChat);
        var mainAgent = new MainAgent(llamaChat);
        var seasonedGardenerAgent = new ReflectionSeasonedGardenerAgent(llamaChat);
        var dataDrivenGardenerAgent = new ReflectionDataDrivenScepticAgent(mistralChat);
        var orchestrator = new OrchestratorAgent(llamaChat);

        Console.WriteLine("GardenaiLocal is ready! Ask your gardening question (type 'exit' to quit):");
        Console.WriteLine("Add #verbose or #debug for detailed reasoning in English, or #nativeverbose for full reasoning in your language. You can combine flags.");

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("User: ");
            Console.ResetColor();
            var question = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(question) || question.Trim().ToLower() == "exit")
                break;

            // Flags
            bool verboseThisTurn = verboseMode || question.Contains("#verbose", StringComparison.OrdinalIgnoreCase) || question.Contains("#debug", StringComparison.OrdinalIgnoreCase);
            bool nativeVerboseThisTurn = question.Contains("#nativeverbose", StringComparison.OrdinalIgnoreCase);

            // Clean question for processing
            var actualQuestion = question.Replace("#nativeverbose", "", StringComparison.OrdinalIgnoreCase)
                                         .Replace("#verbose", "", StringComparison.OrdinalIgnoreCase)
                                         .Replace("#debug", "", StringComparison.OrdinalIgnoreCase)
                                         .Trim();

            // Detect language
            var userLang = await DetectLanguageAsync(llamaChat, actualQuestion);

            // Pipeline (all reasoning in English)
            var retrieved = await retriever.FetchAsync(actualQuestion);
            if (verboseThisTurn)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n[Retriever Output]");
                Console.WriteLine(retrieved + "\n");
                Console.ResetColor();
            }

            var answer = await mainAgent.AnswerAsync(actualQuestion, retrieved);
            if (verboseThisTurn)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("[Main Agent Output]");
                Console.WriteLine(answer + "\n");
                Console.ResetColor();
            }

            var seasonedGardenerReflection = await seasonedGardenerAgent.ReflectAsync(answer);
            if (verboseThisTurn)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("[Seasoned Gardener Reflection]");
                Console.WriteLine(seasonedGardenerReflection + "\n");
                Console.ResetColor();
            }

            var dataDrivenGardenerReflection = await dataDrivenGardenerAgent.ReflectAsync(answer);
            if (verboseThisTurn)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("[Data-Driven Gardener Reflection]");
                Console.WriteLine(dataDrivenGardenerReflection + "\n");
                Console.ResetColor();
            }

            // Orchestrator handles only English
            var orchestratorOutput = await orchestrator.SummarizeVerboseAsync(
                actualQuestion,
                answer,
                seasonedGardenerReflection,
                dataDrivenGardenerReflection,
                nativeVerboseThisTurn
            );

            // Translate entire output to user's language if requested
            string finalAnswer = orchestratorOutput;
            if (nativeVerboseThisTurn && !userLang.Equals("English", StringComparison.OrdinalIgnoreCase))
            {
                finalAnswer = await TranslateAsync(llamaChat, orchestratorOutput, userLang);
            }

            if (verboseThisTurn && !nativeVerboseThisTurn)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("[Orchestrator Output (final, in English)]");
                Console.WriteLine(orchestratorOutput + "\n");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nFINAL ANSWER:\n{finalAnswer}\n");
            Console.ResetColor();
        }

        Console.WriteLine("\n--- Attribution ---");
        Console.WriteLine("Weather data powered by Open-Meteo.com");
        Console.WriteLine("Plant information sourced from Wikipedia.org (CC BY-SA 4.0).");
    }
}
