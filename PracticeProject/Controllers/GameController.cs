using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private const string sessionGameKey = "game";
        public record Coordinates([Required] int x, [Required] int y, [Required] bool prediction);

        [HttpPost("Generate")]
        public IActionResult GenerateBoard(Game game)
        {
            if (!this.ModelState.IsValid) return BadRequest(this.ModelState);
            if (game.X < 1 || game.Y < 1) return BadRequest("Impossible grid");
            game.GenerateGameBoard();
            HttpContext.Session.SetObject(sessionGameKey, game);
            var result = game.GenerateTips();
            return Ok(result);
        }

        [HttpPost("Value")]
        public IActionResult CheckValue(Coordinates coordinates)
        {
            if (!this.ModelState.IsValid) return BadRequest(this.ModelState);
            var game = HttpContext.Session.GetObject<Game>(sessionGameKey);
            if (game == default(Game)) return BadRequest("No game has been started");
            if (coordinates.x < 0 
                || game.X < coordinates.x 
                || coordinates.x < 0 
                || game.Y < coordinates.y) return BadRequest("Wrong coordinates");

            var result = game.CheckValue(coordinates.x, coordinates.y, coordinates.prediction);
            HttpContext.Session.SetObject(sessionGameKey, game);
            if (result == null) return BadRequest();

            return Ok(new { result, game.Lifes});   
        }
    }
}
