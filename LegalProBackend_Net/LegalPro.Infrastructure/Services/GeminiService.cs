using LegalPro.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Google.GenAI;
using Google.GenAI.Types;
using Polly;
using Polly.Registry;

namespace LegalPro.Infrastructure.Services;

// ═══════════════════════════════════════════════════════
// SRP: Single Responsibility Principle  
// GeminiService SOLO se encarga de enviar prompts a Gemini.
// Los prompts específicos (template strings) van en cada
// Command Handler, NO aquí.
// OCP: Open/Closed — agregar nuevos prompts NO modifica esta clase.
// ═══════════════════════════════════════════════════════

/// <summary>
/// Low-level Gemini client. Single responsibility: send prompts, return text.
/// The prompt construction is the responsibility of each CQRS Handler (SRP).
/// </summary>
public class GeminiService : IGeminiService
{
    private readonly Client _client;
    private readonly ResiliencePipeline _pipeline;

    public GeminiService(IConfiguration configuration, ResiliencePipelineProvider<string> pipelineProvider)
    {
        // Lee GEMINI_API_KEY (Railway/env var) con fallback a Gemini:ApiKey (appsettings)
        var apiKey = configuration["GEMINI_API_KEY"]
                     ?? configuration["Gemini:ApiKey"]
                     ?? throw new InvalidOperationException("GEMINI_API_KEY no está configurado.");

        // Pasa la API key directamente al constructor — no contamina Environment global.
        // Firma: Client(vertexAI, apiKey, credential, project, location, httpOptions)
        _client = new Client(vertexAI: false, apiKey: apiKey);
        _pipeline = pipelineProvider.GetPipeline("gemini-pipeline");
    }

    /// <summary>Core method: send any prompt to any model.</summary>
    public async Task<string> GenerateAsync(string prompt, string model = "gemini-2.5-flash")
    {
        return await _pipeline.ExecuteAsync(async ct =>
        {
            var config = new GenerateContentConfig { ResponseMimeType = "application/json" };
            var response = await _client.Models.GenerateContentAsync(
                model: model,
                contents: prompt,
                config: config
            );
            return response.Text ?? "{}";
        }, CancellationToken.None);
    }

    // ── Delegated methods (each calls GenerateAsync with its specific prompt) ──

    public async Task<string> GenerateSystemResponseAsync(string userPrompt, string context)
    {
        var prompt = $"RESPONDE ESTRICTAMENTE EN FORMATO JSON.\nCONTEXTO:\n{context}\n\nCONSULTA: {userPrompt}";
        return await GenerateAsync(prompt);
    }

    public async Task<string> AnalyzeLegalDocumentAsync(string documentText)
    {
        var prompt = $"Eres un Analista Criminológico de Expedientes. Analiza el siguiente expediente buscando contradicciones, folios erróneos o hechos que no cuadren.\nRESPONDE ESTRICTAMENTE EN FORMATO JSON CON LA SIGUIENTE ESTRUCTURA:\n{{\"resumenGeneral\": \"string\", \"anotaciones\": [{{\"gravedad\": \"Alta/Media/Baja\", \"titulo\": \"string\", \"descripcion\": \"string\", \"folioReferencia\": \"string\"}}], \"isError\": false}}\n\nEXPEDIENTE:\n{documentText}";
        return await GenerateAsync(prompt);
    }

    public async Task<string> PredictOutcomeAsync(string hechos, string materia, string juzgadoSala, string juez)
    {
        var prompt = $"Eres un Predictor Judicial del Perú. Emite un pronóstico sobre la viabilidad del caso.\nRESPONDE ESTRICTAMENTE EN FORMATO JSON CON LA ESTRUCTURA:\n{{\"probabilidadExito\": 50, \"veredictoGeneral\": \"string\", \"factores\": [{{\"tipo\": \"Favorable/Desfavorable\", \"descripcion\": \"string\"}}]}}\n\nHECHOS: {hechos}\nMATERIA: {materia}\nJUZGADO: {juzgadoSala}\nJUEZ: {juez}";
        return await GenerateAsync(prompt);
    }

    public async Task<string> ChatLegalAsync(string history, string userInput)
    {
        var prompt = $"Eres Asistente LegalPro AI. Ignora el historial si es irrelevante.\nHistorial: {history}\nCONSULTA: {userInput}\nRESPONDE ESTRICTAMENTE JSON TIPO:\n{{\"respuesta\": \"string markdown\", \"funcionesUsadas\": [\"string\"]}}";
        return await GenerateAsync(prompt, "gemini-2.5-flash");
    }

    public async Task<string> DraftDocumentAsync(string promptData)
    {
        var prompt = $"Eres un redactor de escritos legales experto en Derecho Peruano.\nGenera un borrador detallado basado en las directrices.\nRESPONDE ESTRICTAMENTE EN JSON TIPO:\n{{\"borrador\": \"string\", \"leyesCitadas\": [\"string\"]}}\n\nDIRECTRICES: {promptData}";
        return await GenerateAsync(prompt);
    }
}
