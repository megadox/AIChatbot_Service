namespace BAStudio.Chatbot.Contracts;

public interface IChatOrchestrator
{
    IAsyncEnumerable<ChatStreamEvent> AskAsync(ChatRequest request, CancellationToken cancellationToken);
}

public interface IEmbeddingService
{
    int Dimension { get; }
    float[] Embed(string text);
}

public interface IVectorStore
{
    Task<IReadOnlyList<RetrievedChunk>> SearchAsync(SearchRequest request, CancellationToken cancellationToken);
}

public interface ILlmService
{
    IAsyncEnumerable<string> StreamAsync(string prompt, CancellationToken cancellationToken);
}

public interface IPromptBuilder
{
    string Build(string question, IReadOnlyList<RetrievedChunk> chunks);
}
