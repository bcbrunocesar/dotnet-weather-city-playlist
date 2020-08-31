using Ingaia.Challenge.WebApi.Context;
using Ingaia.Challenge.WebApi.Interfaces;
using Ingaia.Challenge.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Controllers
{    
    public class PlaylistsController : ControllerBase
    {
        private readonly IAppService _appService;
        private readonly ApplicationDbContext _context;

        public PlaylistsController(IAppService appService, ApplicationDbContext context)
        {
            _appService = appService;
            _context = context;
        }

        [HttpGet]
        [Route("api/v1/weather-playlist")]
        public async Task<IEnumerable<CityStatisticModel>> Get()
        {
            var citiesStatistics = await _context.CitiesStatistics.ToListAsync();

            // implementar lógica para listar cidades mais acessadas

            return Ok(citiesStatistics);
        }

        // Colocar o cache dentro do serviço/ ou verificar se existe cache, salvar no bd, e depois retornar o cache
        // [ResponseCache(Duration = 900, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "cityName" })]
        [HttpGet]
        [Route("api/v1/weather-playlist/{cityName}")]
        public async Task<IActionResult> Get(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                return BadRequest();
            }

            var playlist = await _appService.GetWeatherPlaylist(cityName);
            return Ok(playlist);
        }
    }
}