
using BMS.BLL.Services;
using BMS.BLL.Services.DbServices;
using BMS.BLL.Services.Events;
using BMS.Models.Models;
using BMS_API.Dto;
using BMS_API.EventHandlers;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BMS_API.Constants.Constants; //custom class to svae constants , key-values

namespace BMS_API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class LibraryController : ControllerBase  //MVC-UI-Controller //API-ControllerBase
{
    //BLL
    private readonly IDbServices<Book> _manageBook; //here already events are declared 

    //BLL.Events
    private readonly IEvents _events;

    //api event handlers
    private readonly LibraryEventHandlers _leh;  //here events actionsa are defined

    //identity
    private readonly UserManager<IdentityUser> _usermanager;

    //configuration
    private readonly IConfiguration _config;


    //--------------------------------------------constructor-start-----------------------------
    public LibraryController(
        
        //BLL injection
        IDbServices<Book> idb, //CRUD operations  //here already events are declared 

        IEvents events,

        //BMS-API. library event handlers
        LibraryEventHandlers leh, //Error handling

        //Identity 
        UserManager<IdentityUser> usermanager, //User credentials from identity tables
  

        //configuration read from appsettings 
        IConfiguration config //Read jwt token
        )


        {
        //BLL
        _manageBook = idb;

        //BMS-API.library event handlers
        _leh = leh;

        //BMS.BLL.Events
        _events = events;

        //Identity
        _usermanager = usermanager;

        //configuration
        _config = config;

        //catch(Exception ex)
        //printex.error

        //register events to methods
        _events.BookAddSucceeded += _leh.HandleBookAdditionSuccess;
        //_manageBook.BookAddSucceeded += _leh.HandleBookOperationSuccess
        _events.BookDeletionSucceeded += _leh.HandleBookDeletionSuccess;
        _events.BookupdationSucceeded += _leh.HandleBookUpdationSuccess;
        _events.ValidationFailed += _leh.HandleValidationFailure;
    }

    ///--------------------------------------------------------constructor-end-----------------------------------


    //API-LOGIN
    //POST:api/Login
    
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) //non procedural programming , //functions procedural programming 
    {

        var user = await _usermanager.FindByNameAsync(loginDto.Username);

        var passcheck = await _usermanager.CheckPasswordAsync(user, loginDto.Password);

        if (user == null || user.UserName ==null || passcheck is false) //tight coupling  //louse coipling -events
        {

            return Unauthorized("Invalid User");

        }
        

        //fetch role
        var roles = await _usermanager.GetRolesAsync(user);

        //debug roles
        Console.WriteLine($"debug roles:{roles}");

        if(roles.Count==0)
        {
            return Unauthorized("Access Denied!");
        }

        //store the username and Id for generating secure token
         var claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)

                //ClaimTypes.
            };
            

        claimsList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        //debug claimsList
        Console.WriteLine($"claimsList: {claimsList}");


        //Jwt key

        var JwtKey = Encoding.UTF8.GetBytes(_config["JWT:Key"]??"");

        if(JwtKey==Encoding.UTF8.GetBytes(""))
        {
            Console.WriteLine("JWT KEY is null");

        }

        //symmetric key generation
        var key = new SymmetricSecurityKey(JwtKey);

        //generating signing credentials using hmac sha256 applied on symmetrickey //digital signature
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //generate token
            var token = new JwtSecurityToken(

                //stored user credentials
                claims: claimsList, //<--user id , username

                //token expiry
                expires: TokenExpiryTime,  //<---expiry time

                //credentials are used for digitally sign token
                signingCredentials: signingCredentials   //<----jwtkey

                );


        //return token
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token),
                        
                        username=loginDto.Username });

    }

    //RFC compliance 
    //eventless functions


    //view all books
    // GET: api/Library
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBooks()
    {
        try
        {
            var books = await _manageBook.ViewAllBooks();
            return Ok(books);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


 
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task <IActionResult> SearchBook([FromQuery] string keywords)
    {
        try
        {
           var result= await _manageBook.SearchBooks(keywords);
            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server error{ex}");
        }
    }


    //view book by id
    // GET: api/Library/{id}
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task <IActionResult> GetBook(int id)
    {
        try
        {
            var result = await _manageBook.ViewBook(id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"internal server error: {ex.Message}");
        }
    }


 
 //event triggered functions

    //add book 
    // POST: api/Library
    [HttpPost]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> AddBook([FromBody] InsertBookDto bookDto)
    {
        try
        {
            if (bookDto == null)
            {
                return  BadRequest("Book data is required");
            }

            //map BookDto to Book
            var book = bookDto.Adapt<Book>();

            //trigger events
            await _manageBook.AddBook(book);

            //event is hidden , moved to other classes include validation 

            //event status return
            var result = _leh.tempData["status"];
            Console.WriteLine($"status during post: {result}");

           return Ok(result);


        }
        catch (Exception ex)
        {
            return StatusCode(500, $"internal server error: {ex.Message}");
        }
    }

    //update book
    // PUT: api/Library/
    [HttpPut]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> UpdateBook([FromBody] BookDto bookDto)
    {
        try
        {
            if (bookDto == null)
            {
                return BadRequest("Book data is required");
            }

            //map BookDto to Book
            var book = bookDto.Adapt<Book>();
            
            //trigger event
            await _manageBook.UpdateBook(book);

            //return status
            var result = _leh.tempData["status"];
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"internal server error: {ex.Message}");
        }
    }
    
    //delete book by id
    // DELETE: api/Library/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        try
        {
            await _manageBook.DeleteBook(id);

            //event returning
            var result = _leh.tempData["status"];
            return Ok(result);

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"internal server error: {ex.Message}");
        }
    }
}
