using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using BAStudio.Chatbot.Contracts;

namespace BAStudio.Chatbot.Infra.Embedding;

/// <summary>
/// Produces deterministic lightweight hash embeddings for local retrieval.
/// </summary>
public sealed class HashEmbeddingService : IEmbeddingService
{
    private static readonly Regex TokenRegex = new(@"[A-Za-z0-9_()]+|[가-힣]+", RegexOptions.Compiled);

    /// <summary>
    /// Creates a hash embedding service with the requested vector dimension.
    /// </summary>
    public HashEmbeddingService(int dimension = 384)
    {
        Dimension = dimension;
    }

    /// <summary>
    /// Gets the vector dimension used by the embedding service.
    /// </summary>
    public int Dimension { get; }

    /// <summary>
    /// Embeds text by hashing normalized tokens into a fixed-size vector.
    /// </summary>
    public float[] Embed(string text)
    {
        var vector = new float[Dimension];
        foreach (Match match in TokenRegex.Matches(text.ToLowerInvariant()))
        {
            var token = match.Value;
            AddToken(vector, token, 1.0f);

            if (IsHangul(token) && token.Length > 2)
            {
                for (var i = 0; i <= token.Length - 2; i++)
                {
                    AddToken(vector, token.Substring(i, 2), 0.35f);
                }
            }
        }

        Normalize(vector);
        return vector;
    }

    private static bool IsHangul(string token) => token.Any(c => c >= '가' && c <= '힣');

    private void AddToken(float[] vector, string token, float weight)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        var index = BitConverter.ToUInt32(hash, 0) % (uint)Dimension;
        var sign = (hash[4] & 1) == 0 ? 1f : -1f;
        vector[index] += sign * weight;
    }

    private static void Normalize(float[] vector)
    {
        double sum = 0;
        foreach (var v in vector)
        {
            sum += v * v;
        }

        var norm = Math.Sqrt(sum);
        if (norm <= 0)
        {
            return;
        }

        for (var i = 0; i < vector.Length; i++)
        {
            vector[i] = (float)(vector[i] / norm);
        }
    }
}
