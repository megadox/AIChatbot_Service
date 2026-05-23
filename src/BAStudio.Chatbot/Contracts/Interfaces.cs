namespace BAStudio.Chatbot.Contracts;

/// <summary>
/// Coordinates intent detection, retrieval, and answer streaming for a chat request.
/// </summary>
public interface IChatOrchestrator
{
    /// <summary>
    /// Answers a chat request as a stream of status and token events.
    /// </summary>
    IAsyncEnumerable<ChatStreamEvent> AskAsync(ChatRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Converts text into fixed-size vectors for semantic retrieval.
/// </summary>
public interface IEmbeddingService
{
    /// <summary>
    /// Gets the vector dimension produced by this embedding service.
    /// </summary>
    int Dimension { get; }

    /// <summary>
    /// Embeds the supplied text into a normalized vector.
    /// </summary>
    float[] Embed(string text);
}

/// <summary>
/// Searches and loads chunks from the chatbot knowledge base.
/// </summary>
public interface IVectorStore
{
    /// <summary>
    /// Returns ranked chunks for a search request.
    /// </summary>
    Task<IReadOnlyList<RetrievedChunk>> SearchAsync(SearchRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Returns all chunks belonging to a single source document.
    /// </summary>
    Task<IReadOnlyList<RetrievedChunk>> GetChunksBySourceAsync(string source, CancellationToken cancellationToken);
}

/// <summary>
/// Streams text from a language model or fallback answer generator.
/// </summary>
public interface ILlmService
{
    /// <summary>
    /// Generates answer text for the supplied prompt.
    /// </summary>
    IAsyncEnumerable<string> StreamAsync(string prompt, CancellationToken cancellationToken);
}

/// <summary>
/// Provides web search results for out-of-scope or general questions.
/// </summary>
public interface IWebSearchService
{
    /// <summary>
    /// Searches the web and returns a normalized result object.
    /// </summary>
    Task<WebSearchResult> SearchAsync(string query, CancellationToken cancellationToken);
}

/// <summary>
/// Builds grounded LLM prompts from user questions and retrieved chunks.
/// </summary>
public interface IPromptBuilder
{
    /// <summary>
    /// Creates a prompt that constrains the answer to the retrieved manual chunks.
    /// </summary>
    string Build(string question, IReadOnlyList<RetrievedChunk> chunks);
}
