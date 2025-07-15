<p align="center">
  <img src="Assets/logo.png" alt="GardenaiLocal Logo" width="300"/>
</p>

# 🌱 GardenaiLocal

**GardenaiLocal** is an AI-powered gardening assistant for your desktop or server, running fully on your local PC.

It helps you with planting, garden planning, care advice, and weather information by combining local LLMs (via Ollama), real-time weather, and plant knowledge from Wikipedia.

- 🌱 **Ask any gardening question**—from “When to plant tomatoes in London?” to “Why are my cucumber leaves yellow?”
- ⚡ **Powered by local open-source models** (Llama3, Mistral) via [Ollama](https://ollama.com/), so your data stays private.
- ☀️ **Live weather integration** for your location.
- 📚 **Plant encyclopedia** built with Wikipedia data.
- 💻 Works on Linux, no API keys needed, easy to set up with an included script.
- 🦾 Designed for extensibility—add more tools, data sources, or agents.

## Features

- Runs locally with no need for OpenAI or external API keys
- Uses two LLM models with agent “reflection” for higher-quality answers
- Weather and plant info tools included
- Multi-language input and output support (automatic translation)
- Console-based, simple install, no Docker required

## Quickstart

1. **Build from source self contained:**
```
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o publish/
```
3. **(Optional) Make executable:**
```
chmod +x run_gardenai.sh
```

3. **Run the setup script:**  
```
./run_gardenai.sh
```

4. **Ask your gardening questions!**

> **Note:** On first launch, Ollama and required models will be installed automatically.  
> For best results, ensure you have at least 10GB free disk space.

## Requirements

- Linux x64 (tested on Ubuntu/Debian, should work on most distros)
- [Ollama](https://ollama.com/) (installed automatically)
- Internet connection (for weather/Wikipedia info and model download)

## Cleaning Up

To free up disk space, remove Ollama models:

```sh
rm -rf ~/.ollama/models
To stop Ollama if running as a service:

```

## Contributing

Contributions, feature requests, and bug reports are welcome!

Open an issue or submit a pull request.

## Attribution

- Plant encyclopedia content is sourced from [Wikipedia](https://www.wikipedia.org/) (CC BY-SA 4.0).
- Weather data provided by [Open-Meteo.com](https://open-meteo.com/).
- Local LLMs provided by [Ollama](https://ollama.com/), using [Llama3](https://llama.meta.com/) and [Mistral](https://mistral.ai/) models.

