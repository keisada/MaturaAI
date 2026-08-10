using MaturaAi.DTOs;

namespace MaturaAi.Services
{
    public interface IAiService
    {
       Task<AiHintResponse> GetAiHintAsync(AiHintRequest request);
        
    }
}
