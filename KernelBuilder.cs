using Microsoft.SemanticKernel;
using GardenaiLocal.Tools;
using Microsoft.SemanticKernel.ChatCompletion;

namespace GardenaiLocal
{
    public static class KernelBuilderFactory
    {
        public static Kernel Build(string modelName)
        {
            var builder = Kernel.CreateBuilder();
            builder.AddOllamaChatCompletion(modelName, new Uri("http://localhost:11434"));
            var kernel = builder.Build();

            // Assuming TranslationTool requires a chat completion service, use the one from the kernel
            var chat = kernel.GetRequiredService<IChatCompletionService>();

            kernel.Plugins.Add(KernelPluginFactory.CreateFromObject(new WeatherTool(), "weather"));
            kernel.Plugins.Add(KernelPluginFactory.CreateFromObject(new GardeningNetEncyclopediaTool(), "gardenpedia"));
            kernel.Plugins.Add(KernelPluginFactory.CreateFromObject(new TranslationTool(chat), "translator"));

            return kernel;
        }
    }
}