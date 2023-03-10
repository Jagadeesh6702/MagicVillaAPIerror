
using MagicVilla_Villa2.Model;
using MagicVilla_Villa2.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_Villa2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_Villa2.CustomLogging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MagicVilla_Villa2.Repository.IRepository;
using System.Net;

namespace MagicVilla_Villa2.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ILogging _logger;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;
        public VillaAPIController(IMapper mapper, ILogging logging, IVillaRepository dbVilla)
        {
            _mapper = mapper;
            _logger = logging;
            _dbVilla = dbVilla;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.Log("getting all the villas", "");
                IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Log("you are entering the wrong villa id = " + id, "error");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _logger.Log("villa is not present", "warning");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _response;

        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {

            try
            {
                if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("", "villa already exists!");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                Villa villa = _mapper.Map<Villa>(createDTO);

                /* Villa model = new ()
                 {
                     Amenity = createDTO.Amenity,
                     Details = createDTO.Details,
                     ImageUrl = createDTO.ImageUrl,
                     Name = createDTO.Name,
                     Occupancy = createDTO.Occupancy,
                     Rate = createDTO.Rate,
                     Sqft = createDTO.Sqft
                 };*/
                await _dbVilla.CreateAsync(villa);
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                await _dbVilla.RemoveAsync(villa);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                //var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
                //villa.Name = villaDto.Name;
                //villa.Occupancy = villaDto.Occupancy;
                //villa.Sqft = villaDto.Sqft;
                Villa model = _mapper.Map<Villa>(updateDTO);
                await _dbVilla.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> pathcDto)
        {

            if (pathcDto == null || id == 0)
            {
                return BadRequest();
            }
            var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            /*  VillaUpdateDTO villaDto = new()
                   {
                       Amenity = villa.Amenity,
                       Details = villa.Details,
                       Id = villa.Id,
                       ImageUrl = villa.ImageUrl,
                       Name = villa.Name,
                       Occupancy = villa.Occupancy,
                       Rate = villa.Rate,
                       Sqft = villa.Sqft
                   };*/

            if (villa == null)
            {
                return BadRequest();
            }
            pathcDto.ApplyTo(villaDTO, ModelState);
            Villa model = _mapper.Map<Villa>(villaDTO);

            /*  Villa model = new Villa()
              {
                     Amenity = villaDto.Amenity,
                     Details = villaDto.Details,
                     Id = villaDto.Id,
                     ImageUrl = villaDto.ImageUrl,
                     Name = villaDto.Name,
                     Occupancy = villaDto.Occupancy,
                     Rate = villaDto.Rate,
                     Sqft = villaDto.Sqft
               };*/

            await _dbVilla.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }



    }
}


