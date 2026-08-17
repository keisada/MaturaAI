using MaturaAi.Data;
using MaturaAi.DTOs;
using MaturaAi.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;

namespace MaturaAi.Services
{
    public class GeminiAiService : IAiService
    {
        private readonly AppDbContext _dbContext;
        private readonly HttpClient _client;

        private readonly string _apiKey;

        public GeminiAiService(AppDbContext dbContext, HttpClient client, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _client = client;
            _apiKey = configuration["Gemini:ApiKey"] ?? throw new InvalidOperationException("Gemini Api Key Not Found");
        }

        public async Task<AiHintResponse> GetAiHintAsync(AiHintRequest? request)
        {
            if (request == null)
            {
                return new AiHintResponse
                {
                    Hint = "Brak danych"
                };
            }


            var existingHint = await _dbContext.Hints.FirstOrDefaultAsync(h => h.QuestionId == request.questionId);
            
            if(existingHint != null)
            {
                return new AiHintResponse
                {
                    Hint = existingHint.HintText ?? " "
                }; 
            }

            var question = await _dbContext.Questions
                .FirstOrDefaultAsync(q => q.Id == request.questionId);

            var prompt = $"""
            Jesteś korepetytorem przygotowującym ucznia do matury.

            Zadanie:
            {question.Content}

            Wygeneruj JEDNĄ krótką wskazówkę w języku polskim.

            Zasady:
            - nie podawaj poprawnej odpowiedzi,
            - nie rozwiązuj całego zadania,
            - nie pokazuj swojego toku rozumowania,
            - nie wypisuj analizy, planu ani komentarzy,
            - nie używaj nagłówków typu "Task", "Analysis", "Draft",
            - zwróć WYŁĄCZNIE treść wskazówki,
            - maksymalnie 2 zdania.
            """;
            var body = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = prompt
                            }
                        }
                    }
                },

                generationConfig = new
                {
                    thinkingConfig = new
                    {
                        thinkingLevel = "minimal"
                    }
                }
            };

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/models/gemma-4-26b-a4b-it:generateContent"
            );

            httpRequest.Headers.Add("x-goog-api-key", _apiKey);

            httpRequest.Content = JsonContent.Create(body);

            var response = await _client.SendAsync(httpRequest);

            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new AiHintResponse
                {
                    Hint = $"Błąd Gemini: {response.StatusCode} - {responseText}"
                };
            }

            using var json = JsonDocument.Parse(responseText);
            

            var parts = json.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts");

            string? hint = null;

            foreach (var part in parts.EnumerateArray())
            {
                if (part.TryGetProperty("thought", out var thought) &&
                    thought.GetBoolean())
                {
                    continue;
                }

                if (part.TryGetProperty("text", out var text))
                {
                    var value = text.GetString();

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        hint = value;
                        break;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(hint))
            {
                return new AiHintResponse
                {
                    Hint = "Brak odpowiedzi AI"
                };
            }

            var newHint = new AiHint
            {
                HintText = hint, QuestionId = question.Id
            };

            await _dbContext.Hints.AddAsync(newHint);
            await _dbContext.SaveChangesAsync();

            return new AiHintResponse
            {
                Hint = hint ?? "Brak odpowiedzi AI"
            };

            
        }




    }
}
