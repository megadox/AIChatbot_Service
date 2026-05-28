namespace BAStudio.Chatbot.Infra.Configuration;

/// <summary>
/// Holds runtime configuration for the local model, knowledge base, and retrieval limits.
/// </summary>
public sealed class ChatbotOptions
{
    public string Mode { get; init; } = "Local";
    public string ModelPath { get; init; } = "model/microsoft_Phi-4-mini-instruct-Q4_K_M.gguf";
    public string KbPath { get; init; } = "ChatBot/ba_manual_vector.db";
    public string ApiBaseUrl { get; init; } = "";
    public string ApiToken { get; init; } = "";
    public int ContextSize { get; init; } = 4096;
    public int MaxTokens { get; init; } = 700;
    public double Temperature { get; init; } = 0.2;
    public int TopK { get; init; } = 6;
    public int CandidateLimit { get; init; } = 80;
    public double MinScore { get; init; } = 0.0;
}
