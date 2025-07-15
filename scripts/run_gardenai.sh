#!/bin/bash
set -e

echo "==== GardenaiLocal Setup ===="

cd "$(dirname "$0")"

if ! command -v ollama >/dev/null 2>&1; then
    echo "Ollama not found. Installing Ollama..."
    curl -fsSL https://ollama.com/install.sh | sh
fi

if ! pgrep -x "ollama" >/dev/null; then
    echo "Starting Ollama server in background..."
    ollama serve &
    sleep 5
fi

echo "Pulling required models (llama3, mistral)..."
ollama pull llama3
ollama pull mistral

APP_BINARY="./GardenaiLocal"
if [ ! -f "$APP_BINARY" ]; then
    echo "ERROR: App binary not found at $APP_BINARY"
    exit 1
fi
chmod +x "$APP_BINARY"

echo "Launching GardenaiLocal self-contained binary..."
"$APP_BINARY"

echo
echo "GardenaiLocal demo completed."
echo
echo "---- Optional Manual Ollama Model Cleanup ----"
echo "If you want to free disk space used by Ollama models, run:"
echo "    rm -rf ~/.ollama/models"
echo
echo "To remove only a specific model (example):"
echo "    ollama rm llama3"
echo "    ollama rm mistral"
echo
echo "To stop the Ollama service (not usually needed, and may require sudo):"
echo "    sudo systemctl stop ollama"
echo
echo "That's it! Thank you for using GardenaiLocal."

echo
read -p "Press Enter to close this window..."