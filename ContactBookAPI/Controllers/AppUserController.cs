using AutoMapper;
using ContactBookAPI.DTOs;
using ContactBookAPI.Lib.Core;
using ContactBookAPI.Lib.Core.Services;
using ContactBookAPI.Lib.Model;
using ContactBookAPI.Lib.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactBookAPI.Controllers
{
    [Route("User/")]
    [ApiController]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class AppUserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPhoneNumberRepository _phnRepository;
        private readonly ISocialRepository _socialRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AppUserController(UserManager<AppUser> userManager, IContactRepository contactRepository,
            IPhoneNumberRepository phnRepository, ISocialRepository socialRepository, IMapper mapper,
            IConfiguration config)
        {
            _userManager = userManager;
            _phnRepository = phnRepository;
            _socialRepository = socialRepository;
            _contactRepository = contactRepository;
            _mapper = mapper;
            _config = config;
        }
        // GET: api/<AppUserController>
        [HttpGet]
        [Route("all-users")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Get([FromQuery]PagingParameter paging)
        {
            var users = await  _contactRepository.GetContact(paging);
            var result = _mapper.Map<List<AppUserDto>>(users);
            return Ok(result);
        }

        // GET api/<AppUserController>/5
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Regular")]
        public async Task<IActionResult> Get(string id)
        {
            var res = await _userManager.FindByIdAsync(id);
            if (!string.IsNullOrEmpty(res.Id))
            {
                // handle errors when the id does not exist
                var userPhones = await _phnRepository.ReadPhoneNumbers(res.Id);
                var userSocial = await _socialRepository.ReadSocialsAsync(res.Id);
                var PhoneDto = new List<PhoneNumberDto>();
                var SocialDt = new List<SocialDto>();
                var UserDto = new AppUserDto();
                foreach (var userphone in userPhones)
                    PhoneDto.Add(new PhoneNumberDto { Number = userphone.Number });
                foreach (var social in userSocial)
                    SocialDt.Add(new SocialDto { Name = social.Name, Link = social.Link });
                UserDto.Id = res.Id;
                UserDto.Name = $"{res.FirstName} {res.LastName}";
                UserDto.PhotoUrl = res.PhotoUrl;
                UserDto.Address = $"{res.City}, {res.State}, {res.Country}";
                UserDto.PhoneNumbers = PhoneDto;
                UserDto.Socials = SocialDt;
                return Ok(UserDto);
            }
            return BadRequest();
            // 
        }

        // POST api/<AppUserController>
        [HttpPost]
        [Route("add-new")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostUSer([FromBody] AppUserCreationDto contactCreation)
        {
            // check if user already exist 
            var user = await _userManager.FindByEmailAsync(contactCreation.Email);
            if(user == null)
            {
                var NewUser = new AppUser
                {
                    FirstName = contactCreation.FirstName,
                    LastName = contactCreation.LastName,
                    Email = contactCreation.Email,
                    UserName = contactCreation.Email,
                    City = contactCreation.City,
                    State = contactCreation.State,
                    Country = contactCreation.Country
                   
                };
                var res = await _userManager.CreateAsync(NewUser,contactCreation.Password);

                if (res.Succeeded)
                {
                    await _userManager.AddToRoleAsync(NewUser, "Regular");
                    foreach (var social in contactCreation.Socials)
                    {
                        var Usersoc = new Social { Name = social.Name, Link = social.Link, AppUserId = NewUser.Id };
                        await _socialRepository.CreateAsync(Usersoc);
                    }
                    foreach (var numbers in contactCreation.PhoneNumbers)
                    {
                        var UserPh = new PhoneNumber { Number = numbers.Number, AppUserId = NewUser.Id };
                        await _phnRepository.CreateAsync(UserPh);
                    }
                }
                else
                    return BadRequest();
                return Ok(contactCreation);
            }
            return BadRequest();

           
        }

        // PUT api/<AppUserController>/5
        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Put(string id, [FromBody] AppUserUpdateDto updateDto)
        {
            // check if id is null or empty
            if (string.IsNullOrEmpty(id))
                return BadRequest("id cannot be null");
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                user.FirstName = updateDto.FirstName;
                user.LastName = updateDto.LastName;
                user.Email = updateDto.Email;
                user.City = updateDto.City;
                user.State = updateDto.State;
                user.Country = updateDto.Country;
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, updateDto.Password);
                await _userManager.UpdateAsync(user);
                List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();
                foreach (var phone in updateDto.PhoneNumbers)
                {
                    phoneNumbers.Add(new PhoneNumber { Id = phone.Id, AppUserId = user.Id, Number = phone.Number });
                }
                await _phnRepository.UpdateAsync(phoneNumbers);
                List<Social> socials = new List<Social>();
                foreach (var social in updateDto.Socials)
                {
                    socials.Add(new Social{ Id = social.Id, AppUserId = user.Id, Name=social.Name, Link=social.Link });
                }
                await _socialRepository.UpdateAsync(socials);
                return Ok(updateDto);

            }
            return NotFound();
        }

        // DELETE api/<AppUserController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                var res = await _userManager.DeleteAsync(user);
                if (res.Succeeded)
                    return NoContent();
            }
            return NotFound();
        }

        [HttpGet()]
        [Route("email")]
        [Authorize(Roles = "Admin,Regular")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var res = await _userManager.Users
                .Include(user => user.PhoneNumbers)
                .Include(u => u.Socials)
                .FirstOrDefaultAsync(u => u.Email == email);
            
            if (!string.IsNullOrEmpty(res.Email))
            {
                var result = _mapper.Map<AppUserDto>(res);
                return Ok(result);
            }
            return BadRequest();
        }


        [HttpPatch("photo/{id}")]
        [Authorize(Roles ="Admin,Regular")]
        public async Task<IActionResult> PhotoUpdate(string id, [FromForm] PhotoUpdateDto photoUpdate)
        {
            if (photoUpdate.Photo.Length <= 0)
                return BadRequest("Upload a photo");
            if (string.IsNullOrEmpty(id))
                return BadRequest("User id is empty");
            var user = await _userManager.FindByIdAsync(id);
            if (user.Id == id)
            {
                CloudinarySetup setup = new CloudinarySetup(_config);
                var photoListPar = setup.UploadMyPic(photoUpdate.Photo);
                user.PhotoUrl = photoListPar[0];
                var res = await _userManager.UpdateAsync(user);
                if (res.Succeeded)
                {
                    var result = _mapper.Map<AfterPhotoUpdateDto>(user);
                    return Ok(result);
                }
            }
            return BadRequest();
        }
        [HttpGet("search")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrEmpty(term))
                return BadRequest("Search term cannot be Empty");
            var users = await _userManager.Users
                                      .Include(u=>u.PhoneNumbers)
                                      .Include(u=>u.Socials)
                                      .Where(u => u.Email.Contains(term)
                                      || u.FirstName.Contains(term)
                                      || u.LastName.Contains(term)
                                      || u.City.Contains(term)
                                      || u.State.Contains(term)
                                      || u.Country.Contains(term)
                                      ).ToListAsync();
            if (users.Count<1)
            {
                return NotFound("No search result found");
            }

            var result = _mapper.Map<List<AppUserDto>>(users);
            return Ok(result);
            

        }
    }
}
