using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Interfaces;

public interface IApplicationService
{
    Task<ApplicationDto> ApplyToTask(ApplicationCreateDto dto);
    Task<IEnumerable<ApplicationDto>> GetTaskApplications();
    Task<IEnumerable<ApplicationDto>> GetMyApplication();
}