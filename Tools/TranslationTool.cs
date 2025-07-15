using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Threading.Tasks;

namespace GardenaiLocal.Tools
{
    public class TranslationTool
    {
        private readonly IChatCompletionService _chat;

        public TranslationTool(IChatCompletionService chat) { _chat = chat; }

        [KernelFunction]
        static async Task<string> TranslateAsync(IChatCompletionService chat, string text, string targetLanguage)
        {
            var history = new ChatHistory();
            history.AddSystemMessage(
                $"You are a professional translator fluent in {targetLanguage}. " +
                $"Translate the following gardening assistant reasoning steps from English to {targetLanguage}. " +
                $"Preserve ALL HEADINGS (like [Retriever Output], [Main Agent Output], [Seasoned Gardener Reflection], [Data-Driven Gardener Reflection], [Final Answer]) " +
                $"and all formatting, bullet points, and line breaks. " +
                $"Translate idiomatically, as if speaking to a Polish gardener, and use natural, friendly, easy-to-read language. " +
                $"Do NOT translate the section headings themselves. " +
                $"Do NOT summarize, merge, or omit any content—each section should appear with its original heading and translated body. " +
                $"Output ONLY the translated content."
            );
            history.AddUserMessage(text);
            var result = await chat.GetChatMessageContentAsync(history);
            return result.Content?.Trim() ?? string.Empty;
        }

    }
}
