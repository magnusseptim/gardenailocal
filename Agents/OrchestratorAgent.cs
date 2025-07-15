using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace GardenaiLocal.Agents
{
    public class OrchestratorAgent
    {
        private readonly IChatCompletionService _chat;

        public OrchestratorAgent(IChatCompletionService chat)
        {
            _chat = chat;
        }

        public async Task<string> SummarizeVerboseAsync(
            string question,
            string mainAnswer,
            string seasonedGardenerReflection,
            string dataDrivenGardenerReflection,
            bool nativeVerbose)
        {
            var history = new ChatHistory();
            history.AddSystemMessage(
                $"You are the Orchestrator for a gardening assistant. " +
                $"If the native verbose flag below is TRUE, your output must be clearly structured as follows (all in ENGLISH):\n\n" +
                $"[Retriever Output]\n<retriever step>\n\n" +
                $"[Main Agent Output]\n<main agent step>\n\n" +
                $"[Seasoned Gardener Reflection]\n<seasoned gardener reflection>\n\n" +
                $"[Data-Driven Gardener Reflection]\n<data-driven gardener reflection>\n\n" +
                $"[Final Answer]\nWrite a clear, friendly, full-sentence answer for the user, summarizing all findings and advice above. " +
                $"Address the user directly. Use formatting (like paragraphs or bullet points) for readability. End with an encouraging gardening wish or closing remark.\n\n" +
                $"If the flag is FALSE, only output the final answer. " +
                $"Always reason in English. Native verbose flag: {nativeVerbose}. Attribute weather and encyclopedia sources as needed."
            );

            history.AddUserMessage(
                $"Question: {question}\nMain Answer: {mainAnswer}\nSeasoned Gardener Reflection: {seasonedGardenerReflection}\nData-Driven Gardener Reflection: {dataDrivenGardenerReflection}\nNative verbose flag: {nativeVerbose}"
            );
            var result = await _chat.GetChatMessageContentAsync(history);
            return result?.Content ?? string.Empty;
        }
    }
}
