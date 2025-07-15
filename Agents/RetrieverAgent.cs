using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace GardenaiLocal.Agents
{
    public class RetrieverAgent
    {
        private readonly IChatCompletionService _chat;
        public RetrieverAgent(IChatCompletionService chat) { _chat = chat; }

        public async Task<string> FetchAsync(string question)
        {
            var history = new ChatHistory();
            history.AddSystemMessage(
                "You are a Retriever agent for a gardening assistant. Use your tools (weather, gardenpedia) to collect factual information relevant to the user's gardening question. Respond concisely with the key facts.");
            history.AddUserMessage(question);
            var result = await _chat.GetChatMessageContentAsync(history);
            return result?.Content ?? string.Empty;
        }
    }
}
