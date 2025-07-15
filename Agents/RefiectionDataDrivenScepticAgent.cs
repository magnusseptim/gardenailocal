using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace GardenaiLocal.Agents
{
    public class ReflectionDataDrivenScepticAgent
    {
        private readonly IChatCompletionService _chat;
        public ReflectionDataDrivenScepticAgent(IChatCompletionService chat) { _chat = chat; }

        public async Task<string> ReflectAsync(string answer)
        {
            var history = new ChatHistory();
            history.AddSystemMessage(
                "You are a data-driven, tool-focused gardening expert. Critique and improve the answer for scientific accuracy, data support, and correct usage of gardening tools or methods.");
            history.AddUserMessage(answer);
            var result = await _chat.GetChatMessageContentAsync(history);
            return result?.Content ?? string.Empty;
        }
    }
}
