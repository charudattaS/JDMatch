using JDMatch.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDMatch.Application.UseCases
{
    public interface IMatchResumeUseCase
    {
        Task<ResumeMatchResult> ExecuteAsync(Guid resumeId, Guid jobDescriptionId);
    }
}
