using JDMatch.Application.DTOs;

namespace JDMatch.Application.UseCases
{
    public interface IUploadResumeUseCase
    {
        Task<UploadResumeResponse> ExecuteAsync(
            Guid userId,
            Stream fileStream,
            string originalFileName);
    }

}
