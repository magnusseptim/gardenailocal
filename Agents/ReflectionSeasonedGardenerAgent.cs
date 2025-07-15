using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace GardenaiLocal.Agents
{
    public class ReflectionSeasonedGardenerAgent
    {
        private readonly IChatCompletionService _chat;
        public ReflectionSeasonedGardenerAgent(IChatCompletionService chat) { _chat = chat; }

        public async Task<string> ReflectAsync(string answer)
        {
            var history = new ChatHistory();
            history.AddSystemMessage(
                "You are a seasoned gardener with decades of experience. Critique and improve the answer with practical, hands-on advice, correcting any gardening mistakes or misconceptions.");
            history.AddUserMessage(answer);
            var result = await _chat.GetChatMessageContentAsync(history);
            return result?.Content ?? string.Empty;
        }
    }
}
