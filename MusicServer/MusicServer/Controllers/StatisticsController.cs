using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicServer.Models.Database.Commands;

namespace MusicServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        //// GET: api/Statistics
        [HttpGet]
        public int Get()
        {
            var hex = "0x9c3c30";
            var dec = Convert.ToInt32(hex, 16); //2016
            return dec;
        }

        [HttpGet("{id}", Name = "Dec2Hex")]
        public IActionResult Dec2Hex(int id)
        {
            // Get the integral value of the character.
            int value = Convert.ToInt32(id);
            // Convert the decimal value to a hexadecimal value in string form.
            string hexOutput = String.Format("{0:X}", value);
            return Ok(new { hex = hexOutput });
        }

        //// POST: api/Statistics
        [HttpPost]
        public IActionResult Post([FromBody] CreateConvertCommand command)
        {
            if (!String.IsNullOrEmpty(command.BlockNumber) && command.BlockNumber.Length > 2)
            {
                command.BlockNumber = command.BlockNumber.Substring(2);
                var dec = Int64.Parse(command.BlockNumber, System.Globalization.NumberStyles.HexNumber);
                return Ok(new { blockHeight = dec });
            }
            else
            {
                return Ok(new { blockHeight = 0 });
            }
            
        }
    }
}