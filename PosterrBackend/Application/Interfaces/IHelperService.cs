using PosterrBackend.Application.DTOs;
using PosterrBackend.Domain.Enums;

namespace PosterrBackend.Application.Interfaces
{
	public interface IHelperService
	{
        Task<bool> HasUserExceededDailyActions(string userName);
        Task<RepostAvailabilityStatus> IsRepostAvailableForUser(string userName, int idPost);
        Task<List<RecordDTO>> LoadRecords(int page, bool orderByDate = true);
    }
}

