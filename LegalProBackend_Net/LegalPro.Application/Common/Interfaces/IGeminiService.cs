namespace LegalPro.Application.Common.Interfaces;

// ═══════════════════════════════════════════════════════
// ISP: Interface Segregation Principle
// La fat interface IGeminiService se separa en interfaces
// específicas por responsabilidad. Cada consumidor depende
// SOLO de lo que necesita.
// ═══════════════════════════════════════════════════════

/// <summary>Base interface for any Gemini prompt interaction.</summary>
public interface IGeminiClient
{
    Task<string> GenerateAsync(string prompt, string model = "gemini-2.5-flash");
}

/// <summary>ISP: Only document analysis.</summary>
public interface ILegalAnalyzer
{
    Task<string> AnalyzeLegalDocumentAsync(string documentText);
}

/// <summary>ISP: Only outcome prediction.</summary>
public interface ILegalPredictor
{
    Task<string> PredictOutcomeAsync(string hechos, string materia, string juzgadoSala, string juez);
}

/// <summary>ISP: Only chat interaction.</summary>
public interface ILegalChat
{
    Task<string> ChatLegalAsync(string history, string userInput);
}

/// <summary>ISP: Only document drafting.</summary>
public interface ILegalDrafter
{
    Task<string> DraftDocumentAsync(string promptData);
}

/// <summary>ISP: Only simulation universe generation.</summary>
public interface ISimulationAI
{
    Task<string> GenerateSystemResponseAsync(string userPrompt, string context);
}

/// <summary>
/// Aggregate interface for backward compatibility.
/// New code should use the segregated interfaces above.
/// </summary>
public interface IGeminiService : IGeminiClient, ILegalAnalyzer, ILegalPredictor, ILegalChat, ILegalDrafter, ISimulationAI
{
}
