using BjjTrainer_API.Models.DTO.UserDtos;
using BjjTrainer_API.Services_API.Users;
using Microsoft.AspNetCore.Mvc;

namespace BjjTrainer_API.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProgressController : ControllerBase
    {
        private readonly UserProgressService _userProgressService;

        public UserProgressController(UserProgressService userProgressService)
        {
            _userProgressService = userProgressService;
        }

        [HttpGet("{userId}/progress")]
        public async Task<ActionResult<UserProgressDto>> GetUserProgress(string userId)
        {
            try
            {
                var progress = await _userProgressService.GetUserProgressAsync(userId);
                return Ok(progress);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching user progress.", error = ex.Message });
            }
        }

        // --- Aggregated Daily Training Time ---
        [HttpGet("{userId}/training-time/daily/last7days")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalTrainingTimeLast7Days(string userId)
            => Ok(await _userProgressService.GetDailyTotalTrainingTimeLast7DaysAsync(userId));

        [HttpGet("{userId}/training-time/daily/lastmonth")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalTrainingTimeLastMonth(string userId)
            => Ok(await _userProgressService.GetDailyTotalTrainingTimeLastMonthAsync(userId));

        [HttpGet("{userId}/training-time/daily/last6months")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalTrainingTimeLast6Months(string userId)
            => Ok(await _userProgressService.GetDailyTotalTrainingTimeLast6MonthsAsync(userId));

        [HttpGet("{userId}/training-time/daily/lastyear")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalTrainingTimeLastYear(string userId)
            => Ok(await _userProgressService.GetDailyTotalTrainingTimeLastYearAsync(userId));

        // --- Aggregated Daily Rounds Rolled ---
        [HttpGet("{userId}/rounds-rolled/daily/last7days")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalRoundsRolledLast7Days(string userId)
            => Ok(await _userProgressService.GetDailyTotalRoundsRolledLast7DaysAsync(userId));

        [HttpGet("{userId}/rounds-rolled/daily/lastmonth")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalRoundsRolledLastMonth(string userId)
            => Ok(await _userProgressService.GetDailyTotalRoundsRolledLastMonthAsync(userId));

        [HttpGet("{userId}/rounds-rolled/daily/last6months")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalRoundsRolledLast6Months(string userId)
            => Ok(await _userProgressService.GetDailyTotalRoundsRolledLast6MonthsAsync(userId));

        [HttpGet("{userId}/rounds-rolled/daily/lastyear")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalRoundsRolledLastYear(string userId)
            => Ok(await _userProgressService.GetDailyTotalRoundsRolledLastYearAsync(userId));

        // --- Aggregated Daily Submissions ---
        [HttpGet("{userId}/submissions/daily/last7days")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalSubmissionsLast7Days(string userId)
            => Ok(await _userProgressService.GetDailyTotalSubmissionsLast7DaysAsync(userId));

        [HttpGet("{userId}/submissions/daily/lastmonth")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalSubmissionsLastMonth(string userId)
            => Ok(await _userProgressService.GetDailyTotalSubmissionsLastMonthAsync(userId));

        [HttpGet("{userId}/submissions/daily/last6months")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalSubmissionsLast6Months(string userId)
            => Ok(await _userProgressService.GetDailyTotalSubmissionsLast6MonthsAsync(userId));

        [HttpGet("{userId}/submissions/daily/lastyear")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalSubmissionsLastYear(string userId)
            => Ok(await _userProgressService.GetDailyTotalSubmissionsLastYearAsync(userId));

        // --- Aggregated Daily Taps ---
        [HttpGet("{userId}/taps/daily/last7days")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalTapsLast7Days(string userId)
            => Ok(await _userProgressService.GetDailyTotalTapsLast7DaysAsync(userId));

        [HttpGet("{userId}/taps/daily/lastmonth")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalTapsLastMonth(string userId)
            => Ok(await _userProgressService.GetDailyTotalTapsLastMonthAsync(userId));

        [HttpGet("{userId}/taps/daily/last6months")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalTapsLast6Months(string userId)
            => Ok(await _userProgressService.GetDailyTotalTapsLast6MonthsAsync(userId));

        [HttpGet("{userId}/taps/daily/lastyear")]
        public async Task<ActionResult<List<DailyUserProgressDto>>> GetDailyTotalTapsLastYear(string userId)
            => Ok(await _userProgressService.GetDailyTotalTapsLastYearAsync(userId));
    }
}
