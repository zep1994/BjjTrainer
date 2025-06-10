using BjjTrainer_API.Models.DTO;
using BjjTrainer_API.Models.Moves;
using BjjTrainer_API.Services_API.Moves;
using Microsoft.AspNetCore.Mvc;

namespace BjjTrainer_API.Controllers.Moves
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovesController : ControllerBase
    {
        private readonly MoveService _moveService;

        public MovesController(MoveService moveService)
        {
            _moveService = moveService;
        }

        // ******************************** GET ALL MOVES ********************************
        [HttpGet]
        public async Task<ActionResult<List<MoveDto>>> GetAllMoves()
        {
            var moves = await _moveService.GetAllMovesAsync();
            return Ok(moves);
        }

        // ******************************** GET MOVE BY ID ********************************
        [HttpGet("{id}")]
        public async Task<ActionResult<MoveDto>> GetMoveById(int id)
        {
            var move = await _moveService.GetMoveByIdAsync(id);
            return move == null ? (ActionResult<MoveDto>)NotFound("Move not found.") : (ActionResult<MoveDto>)Ok(move);
        }

        // ******************************** GET MOVES BY IDS ********************************
        [HttpPost("byIds")]
        public async Task<ActionResult<List<MoveDto>>> GetMovesByIdsAsync([FromBody] List<int> moveIds)
        {
            try
            {
                var moves = await _moveService.GetMovesByIdsAsync(moveIds);
                return Ok(moves);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ******************************** CREATE MOVE ********************************
        [HttpPost]
        public async Task<ActionResult<Move>> CreateMove([FromBody] Move move)
        {
            var createdMove = await _moveService.CreateMoveAsync(move);
            return CreatedAtAction(nameof(GetMoveById), new { id = createdMove.Id }, createdMove);
        }

        // ******************************** UPDATE MOVE ********************************
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMove(int id, [FromBody] MoveDto moveDto)
        {
            if (id != moveDto.Id)
            {
                return BadRequest("Move ID mismatch.");
            }

            var success = await _moveService.UpdateMoveAsync(moveDto);
            return !success ? NotFound("Move not found.") : NoContent();
        }

        // ******************************** DELETE MOVE ********************************
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMove(int id)
        {
            var success = await _moveService.DeleteMoveAsync(id);
            return !success ? NotFound("Move not found.") : NoContent();
        }
    }
}
