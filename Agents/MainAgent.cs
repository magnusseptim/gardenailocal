using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace GardenaiLocal.Agents
{
    public class MainAgent
    {
        private readonly IChatCompletionService _chat;
        public MainAgent(IChatCompletionService chat) { _chat = chat; }

        public async Task<string> AnswerAsync(string question, string retrievedFacts)
        {
            var history = new ChatHistory();
            history.AddSystemMessage(
                "You are the main gardening assistant. Use the retrieved facts (from weather or encyclopedia tools) and your gardening expertise to answer the user's question clearly and thoroughly. If the user asked about growing, planting, or care, be sure to address those aspects.");
            history.AddUserMessage($"Question: {question}\n\nRetrieved Facts: {retrievedFacts}");
            var result = await _chat.GetChatMessageContentAsync(history);
            return result?.Content ?? string.Empty;
        }
    }
}
