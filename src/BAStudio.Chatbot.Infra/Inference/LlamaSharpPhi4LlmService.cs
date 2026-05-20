using System.Runtime.CompilerServices;
using BAStudio.Chatbot.Contracts;
using BAStudio.Chatbot.Infra.Configuration;
using LLama;
using LLama.Common;
using LLama.Sampling;

namespace BAStudio.Chatbot.Infra.Inference;

public sealed class LlamaSharpPhi4LlmService : ILlmService, IDisposable
{
    private readonly ChatbotOptions _options;
    private readonly Lazy<ModelHandle> _model;

    public LlamaSharpPhi4LlmService(ChatbotOptions options)
    {
        _options = options;
        _model = new Lazy<ModelHandle>(LoadModel);
    }

    public async IAsyncEnumerable<string> StreamAsync(
        string prompt,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var handle = _model.Value;
        var inference = new InferenceParams
        {
            MaxTokens = _options.MaxTokens,
            SamplingPipeline = new DefaultSamplingPipeline
            {
                Temperature = (float)_options.Temperature,
                TopP = 0.85f,
                TopK = 40
            },
            AntiPrompts = ["<|user|>", "<|system|>", "<|end|>"]
        };

        await foreach (var token in handle.Executor.InferAsync(prompt, inference, cancellationToken))
        {
            yield return token;
        }
    }

    public void Dispose()
    {
        if (_model.IsValueCreated)
        {
            _model.Value.Dispose();
        }
    }

    private ModelHandle LoadModel()
    {
        if (!File.Exists(_options.ModelPath))
        {
            throw new FileNotFoundException("Phi-4 GGUF 모델 파일을 찾을 수 없습니다.", _options.ModelPath);
        }

        var parameters = new ModelParams(_options.ModelPath)
        {
            ContextSize = (uint)_options.ContextSize,
            GpuLayerCount = 0
        };

        var weights = LLamaWeights.LoadFromFile(parameters);
        var context = weights.CreateContext(parameters);
        var executor = new InteractiveExecutor(context);
        return new ModelHandle(weights, context, executor);
    }

    private sealed class ModelHandle : IDisposable
    {
        private readonly LLamaWeights _weights;
        private readonly LLamaContext _context;

        public ModelHandle(LLamaWeights weights, LLamaContext context, InteractiveExecutor executor)
        {
            _weights = weights;
            _context = context;
            Executor = executor;
        }

        public InteractiveExecutor Executor { get; }

        public void Dispose()
        {
            _context.Dispose();
            _weights.Dispose();
        }
    }
}
