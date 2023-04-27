using AutoMapper;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPersonalProject.Data;
using MyPersonalProject.Models;
using MyPersonalProject.Models.Dto;
using MyPersonalProject.Repository.IRepository;
using MyPersonalProject.Services;
using System.Net;
using System.Text.Json;

namespace MyPersonalProject.Controllers
{
    [Route("api")]
    [ApiVersion("1.0")]
    [ApiController]
    
    public class ContactBookAPI : ControllerBase
    {
        protected APIResponse _response;
        private readonly ILogger<ContactBookAPI> _logger;
        private readonly IContactRepository _database;
        private readonly IMapper _mapper;

        public IUploadService _uploadService { get; }

        public ContactBookAPI(ILogger<ContactBookAPI> logger, IContactRepository database, IMapper mapper, IUploadService uploadService)
        {
            _logger = logger;
            _database = database;
            _mapper = mapper;
            _uploadService = uploadService;
            _response = new();
        }



        [HttpGet]
        [ResponseCache(Duration = 30)]
        
        [Route("Contacts")]
        [ProducesDefaultResponseType(typeof(void))]
        public async Task<ActionResult<APIResponse>> GetAllContacts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3)
        {
            try
            {
                _logger.LogInformation($"Getting all contacts with pagination");
                IEnumerable<Contact> contactList = await _database.GetAllContactsAsync();

               

                    // Calculate the number of contacts to skip based on the page number and page size
                int contactsToSkip = (pageNumber - 1) * pageSize;

                // Get the page of contacts based on the page number and page size
                IEnumerable<Contact> pagedContactList = contactList.Skip(contactsToSkip).Take(pageSize);


                
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
                

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<ContactDto>>(pagedContactList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpPost("contacts/add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateContact([FromBody] ContactCreateDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest();
                }

                var existingContact = await _database.GetAsync(c => c.FirstName.ToLower() == createDto.FirstName.ToLower());
                if (existingContact != null)
                {
                    ModelState.AddModelError("ErrorMessage", "Contact already exists");
                    return BadRequest(ModelState);
                }

                var contact = _mapper.Map<Contact>(createDto);



                await _database.CreateAsync(contact);
                var contactDto = _mapper.Map<ContactDto>(contact);

                _response.Result = _mapper.Map<List<ContactDto>>(contact);
                _response.StatusCode = HttpStatusCode.OK;
                // return Ok(_response);
                return CreatedAtRoute(nameof(GetContact), new { id = contactDto.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [ProducesDefaultResponseType(typeof(void))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("contact/delete")]
        public async Task<ActionResult<APIResponse>> DeleteContact(int id)
        {

            if (id == 0) { return NotFound(); }
            var contact = await _database.GetAsync((u => u.Id == id));
            if (contact == null)
            {
                return NotFound();
            }
            await _database.RemoveAsync(contact);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }




        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("update/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateContact(int id, [FromBody] ContactUpdateDto updateDto)
        {
            if (id == 0) { return BadRequest(); }
            if (updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }

            Contact model = _mapper.Map<Contact>(updateDto);

            await _database.UpdateAsync(model);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);



        }



        [HttpGet]
        [Route("contact/{id}")]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(typeof(ContactDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<APIResponse>> GetContact(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _logger.LogInformation("Get Contact Error with Id " + id);
                    return BadRequest(_response);
                }

                var contact = await _database.GetAsync(u => u.Id == id);

                if (contact == null)
                {
                    return NotFound();
                }

                _response.Result = _mapper.Map<ContactDto>(contact);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

                // return Ok(_mapper.Map<ContactDto>(contact));
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [ResponseCache(Duration = 30)]
        
        [Route("Contacts/search")]
        [ProducesDefaultResponseType(typeof(void))]
        public async Task<ActionResult<APIResponse>> SearchContacts([FromQuery] string search = "")
        {
            try
            {
                _logger.LogInformation($"Searching contacts with '{search}' in first name or last name");

                IEnumerable<Contact> contactList = await _database.GetAllContactsAsync();

                if (!string.IsNullOrEmpty(search))
                {
                    contactList = contactList.Where(c => c.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase)
                || c.LastName.Contains(search, StringComparison.OrdinalIgnoreCase)
                || c.Email.Contains(search, StringComparison.OrdinalIgnoreCase)
                || c.PhoneNumber.Contains(search, StringComparison.OrdinalIgnoreCase));

                }

                _response.Result = _mapper.Map<List<ContactDto>>(contactList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPatch]
        [Route("photo/{id}")]
        public async Task<IActionResult> UploadPhoto([FromForm] PhotoUploadDto model, int id)
        {
            
            var uploadResult = await _uploadService.UploadFileAsync(model.File, id);
            if (uploadResult != null)
            {
                return Ok($"PublicId {uploadResult["PublicId"]},Url {uploadResult["Url"]}");
            }
            else { return BadRequest("upload was not successful"); }
        }









    }
}
