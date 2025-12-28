using PosterrBackend.Domain.Constants;

namespace PosterrBackend.Infrastructure
{
	public static class Helper
	{
        public static (int, int) GetSkipAndTakeAmount(int page)
        {
            int firstLoadAmount = Constants.RECORDS_IN_FIRST_LOAD;
            int normalLoadAmount = Constants.RECORDS_TO_LOAD;

            int skipAmount = page == 0 ? 0 : firstLoadAmount + ((page - 1) * normalLoadAmount);
            int takeAmount = page == 0 ? firstLoadAmount : normalLoadAmount;

            return (skipAmount, takeAmount);
        }
    }
}

