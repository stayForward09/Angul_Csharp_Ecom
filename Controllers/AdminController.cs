using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using StackApi.Common;
using StackApi.Core.IConfiguration;
using StackApi.Data;
using StackApi.Dtos;
using StackApi.Models;
using StackApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StackApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
public class AdminController : ControllerBase
{
    private readonly ILogger<AdminController> _logger;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMailService mailService;
    private readonly ICacheService cacheService;
    private readonly IMapper mapper;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly PartDbContext context;
    private readonly IJwtService jwtService;

    public AdminController(ILogger<AdminController> logger, IUnitOfWork _unitofwork,
    IMailService _mailservice, ICacheService _cacheService, IMapper _mapper,
    IWebHostEnvironment _webHostEnvironment, PartDbContext _context, IJwtService _IJwtService)
    {
        _logger = logger;
        unitOfWork = _unitofwork;
        mailService = _mailservice;
        cacheService = _cacheService;
        mapper = _mapper;
        webHostEnvironment = _webHostEnvironment;
        context = _context;
        jwtService = _IJwtService;
    }

    [HttpPost]
    [Route("[controller]/AddUser")]
    public async Task<IActionResult> AddUser(User user)
    {
        try
        {
            var extuser = await unitOfWork.Users.CheckEmailExists(user.EmailID);
            if (extuser is null)
            {
                user.EmailConfirmed = false;
                user.IsAdmin = false;
                user.Password = jwtService.HashPassword(user.Password);
                await unitOfWork.Users.Add(user);
                await unitOfWork.CompleteAsync();
                string OTP = mailService.GenerateOTP();
                await cacheService.SetCacheValueAsync(user.EmailID, OTP);
                string content = await mailService.CreateContent(user.Fname + " " + Convert.ToString(user.Mname) + " " + user.Lname, OTP);
                bool result = await mailService.SendEmail(user.EmailID, "Confirm Mail ID", content);
                var token = jwtService.GenerateJwt(user);
                if (result)
                {
                    return new OkObjectResult(new Response<object>() { Message = "Confirm Mail ID", Succeeded = true, Data = token });
                }
                else
                {
                    return new OkObjectResult(new Response<object> { Message = "Created..", Succeeded = true, Data = token });
                }
            }
            else
            {
                return new OkObjectResult(new Response<object> { Message = "Account Already Exists, Please Login", Succeeded = false });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<object> { Errors = new string[] { ex.Message }, Succeeded = false });
        }
    }

    [HttpPost]
    [Route("[controller]/Login")]
    [AllowAnonymous]
    // sasiKumar12
    // Mathi@124
    // mathi.root@gmail.com
    public async Task<IActionResult> Login(LoginDtos login)
    {
        try
        {
            var users = await unitOfWork.Users.CheckEmailExists(login.emailID);
            string hashpass = jwtService.HashPassword(login.Password);
            if (users == null)
            {
                return BadRequest(new Response<object>("Invalid Username", false));
            }
            if (users.Password != hashpass)
            {
                return BadRequest(new Response<object>($"Invalid Username or Password", false));
            }
            var token = jwtService.GenerateJwt(users);
            return Ok(new Response<object>() { Message = "Login successfully", Data = token, Succeeded = true });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<object>() { Errors = new string[] { ex.Message.ToString() } });
        }
    }

    [Authorize(Policy = "Admin")]
    [HttpGet]
    [Route("[controller]/GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var data = await unitOfWork.Users.All();
        return Ok(new Response<object>(data));
    }

    [Authorize(Policy = "Admin")]
    [HttpGet]
    [Route("[controller]/GetDetails")]
    public async Task<IActionResult> GetDetails()
    {
        var data = await unitOfWork.userDetailsRepository.All();
        return Ok(new Response<object>(data));
    }

    [HttpPost]
    [Route("[controller]/SaveUserDetails")]
    [Authorize]
    public async Task<IActionResult> SaveUserDetails(UserDetails userDetails)
    {
        await unitOfWork.userDetailsRepository.Add(userDetails);
        await unitOfWork.CompleteAsync();
        return Ok(new Response<object>("User Details Updated", true));
    }

    [HttpPost, Authorize]
    [Route("[controller]/VerifyMailOTP")]
    public async Task<IActionResult> VerifyMailOTP(VerifyOTP verifyOTP)
    {
        try
        {
            string OTP = await cacheService.GetCacheValueAsync(verifyOTP.EmailID.Trim());
            if (OTP == verifyOTP.OTP)
            {
                var user = await unitOfWork.Users.CheckEmailExists(verifyOTP.EmailID);
                if (user is not null)
                {
                    user.EmailConfirmed = true;
                }
                return Ok(new Response<object>("Account Verified Successfully..", true));
            }
            else
            {
                return BadRequest(new Response<object>("Invalid OTP", false));
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<object>() { Errors = new string[] { ex.Message.ToString() } });
        }
    }

    // [Authorize(Policy = "Admin")]
    [HttpPost]
    [Route("[controller]/AddPart")]
    public async Task<IActionResult> AddPart(PartAdd partAdd)
    {
        try
        {
            var prt = mapper.Map<Part>(partAdd);
            await unitOfWork.partRepository.Add(prt);
            await unitOfWork.CompleteAsync();
            return Ok(new Response<object>(prt));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<object>() { Errors = new string[] { ex.Message.ToString() } });
        }
    }

    // [Authorize(Policy = "Admin")]
    [HttpPost, DisableRequestSizeLimit]
    [Route("[controller]/AddPartImage")]
    public async Task<IActionResult> AddPartImage([FromForm] PartImageDtos partImageDtos)
    {
        try
        {
            string filepath = webHostEnvironment.WebRootPath + "/PartImgs/";
            PartImages images;
            foreach (var file in partImageDtos.PartFiles)
            {
                images = new PartImages();
                var extension = Path.GetExtension(file.FileName);
                images.PiIsTD = extension.ToLower() == ".glb";
                string name = Guid.NewGuid().ToString() + file.FileName.Substring(file.FileName.LastIndexOf('.')).ToString();
                using var Stream = new FileStream(filepath + name, FileMode.Create);
                await file.CopyToAsync(Stream);
                images.Pid = partImageDtos.PartID;
                images.PiFilename = name;
                await unitOfWork.partImageRepository.Add(images);
                await unitOfWork.CompleteAsync();
            }
            return Ok(new Response<object>("File Uploaded Successfully", true));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<object>() { Errors = new string[] { ex.Message.ToString() } });
        }
    }

    [HttpGet]
    [Route("[controller]/GetPartsDetailed")]
    public async Task<IActionResult> GetPartsDetailed()
    {
        try
        {
            string urlpath = Request.Scheme + "://" + Request.Host.Value;
            var data = await (from pa in context.Part
                              select new
                              {
                                  ID = pa.Pid,
                                  Part = pa.PartName,
                                  Description = pa.PartDesc,
                                  Price = pa.PartPrice,
                                  Images = context.PartImages.Where(x => x.Pid == pa.Pid).Select(x => new
                                  {
                                      IsTd = x.PiIsTD,
                                      Url = x.PiIsTD ? x.PiFilename : urlpath + Url.Content($"/PartImgs/{x.PiFilename}")
                                  }).ToList()
                              }).ToListAsync();
            Response.Headers.Add("X-Pagination", urlpath);
            return Ok(new Response<object>(data));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<object>() { Errors = new string[] { ex.Message.ToString() } });
        }
    }

    [HttpGet, DisableRequestSizeLimit]
    [Route("[controller]/DownloadFile/{fileUrl}")]
    public async Task<IActionResult> DownloadFile([FromRoute] string fileUrl)
    {
        var filePath = webHostEnvironment.WebRootPath + "/PartImgs/" + fileUrl;
        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var memory = new MemoryStream();
        await using (var stream = new FileStream(filePath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;
        return File(memory, GetContentType(filePath), filePath);
    }

    [HttpGet]
    [Route("[controller]/SearchProduct/{searchText}")]
    public async Task<IActionResult> SearchProductAutocomplete([FromRoute] string searchText)
    {
        try
        {
            var data = await unitOfWork.partRepository.SearchbyText(searchText);
            return Ok(new Response<object>(data));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<object>() { Errors = new string[] { ex.Message.ToString() } });
        }
    }

    [HttpGet]
    [Route("[controller]/SearchPrd/{searchText}/{pageNumber}/{pageSzie}")]
    public async Task<IActionResult> SearchPrd([FromRoute] string searchText, int pageNumber, int pageSzie)
    {
        try
        {
            string urlpath = Request.Scheme + "://" + Request.Host.Value;
            var currentUser = await getCurrentUser();
            if (currentUser is not null)
            {
                SearchViewHistory searchViewHistory = new SearchViewHistory();
                searchViewHistory.searchterm = searchText;
                searchViewHistory.visitedPrd = null;
                var users = await unitOfWork.Users.CheckEmailExists(currentUser.emailID);
                searchViewHistory.UsID = users.UsID.ToString();
                await unitOfWork.searchViewHistoryRepository.Add(searchViewHistory);
                await unitOfWork.CompleteAsync();
            }
            var totalcount = context.Part.Where(x => x.PartName.Contains(searchText)).Count();

            var result = await (from prt in context.Part
                                from dis in context.Discount.Where(x => (x.PrdId == prt.Pid || x.CId == prt.PcId) && x.EndDate > DateTime.Now).DefaultIfEmpty()
                                where prt.PartName.Contains(searchText)
                                select new
                                {
                                    PrdName = prt.PartName,
                                    Id = prt.Pid,
                                    desc = prt.PartDesc,
                                    price = prt.PartPrice,
                                    images = context.PartImages.Where(x => x.Pid == prt.Pid && x.PiIsTD == false).Select(x => urlpath + Url.Content($"/PartImgs/{x.PiFilename}")).ToList(),
                                    Discount = dis != null ? new
                                    {
                                        Amount = dis.Amount,
                                        id = dis.Did,
                                        ends = dis.EndDate.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                                        now = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                                        diff = dis.EndDate - DateTime.Now,
                                        type = dis.DType
                                    } : null
                                }).OrderBy(x => x.PrdName).Skip((pageNumber - 1) * pageSzie).Take(pageSzie).ToListAsync();

            var data = new { total = totalcount, data = result };
            return Ok(new Response<object>(data));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<object>() { Errors = new string[] { ex.Message.ToString() } });
        }
    }

    [HttpPost]
    [Route("[controller]/[action]")]
    public async Task<IActionResult> LoginUser(LoginDtos login)
    {
        var user = await unitOfWork.Users.GetUserbyCondition(x => x.EmailID == login.emailID && x.IsAdmin == false);
        if (user is null)
        {
            return BadRequest(new Response<object>("Invalid Username", false));
        }
        string hashpass = jwtService.HashPassword(login.Password);
        if (hashpass != user.Password)
        {
            return BadRequest(new Response<object>("Invalid Username or Password", false));
        }
        var accestoken = jwtService.GenerateJwt(user);
        return Ok(new Response<object>() { Message = "Login successfully", Data = accestoken, Succeeded = true });
    }

    [HttpGet]
    [Route("[controller]/GetPartbyID/{Pid}")]
    public async Task<IActionResult> GetPartbyID([FromRoute] Guid Pid)
    {
        try
        {
            string urlpath = Request.Scheme + "://" + Request.Host.Value;
            var currentUser = await getCurrentUser();
            if (currentUser is not null)
            {
                SearchViewHistory searchViewHistory = new SearchViewHistory();
                searchViewHistory.searchterm = null;
                searchViewHistory.visitedPrd = Pid.ToString();
                var users = await unitOfWork.Users.CheckEmailExists(currentUser.emailID);
                searchViewHistory.UsID = users.UsID.ToString();
                await unitOfWork.searchViewHistoryRepository.Add(searchViewHistory);
                await unitOfWork.CompleteAsync();
            }

            var data = await (from pa in context.Part
                              where pa.Pid == Pid
                              select new
                              {
                                  ID = pa.Pid,
                                  Part = pa.PartName,
                                  Description = pa.PartDesc,
                                  Price = pa.PartPrice,
                                  Images = context.PartImages.Where(x => x.Pid == pa.Pid).Select(x => new
                                  {
                                      IsTd = x.PiIsTD,
                                      Url = x.PiIsTD ? x.PiFilename : urlpath + Url.Content($"/PartImgs/{x.PiFilename}")
                                  }).ToList()
                              }).ToListAsync();
            return Ok(new Response<object>(data));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<object>() { Errors = new string[] { ex.Message.ToString() } });
        }
    }

    [HttpGet, Authorize]
    [Route("[controller]/getHomeDetails")]
    public async Task<IActionResult> getHomeDetails()
    {
        try
        {
            var userdetails = await getCurrentUser();
            string urlpath = Request.Scheme + "://" + Request.Host.Value;
            var users = await unitOfWork.Users.CheckEmailExists(userdetails.emailID);
            var lastvisited = await (from sHis in context.SearchViewHistory
                                     join pa in context.Part on sHis.visitedPrd equals pa.Pid.ToString()
                                     where sHis.visitedPrd != null
                                     select new
                                     {
                                         ID = pa.Pid,
                                         Part = pa.PartName,
                                         Description = pa.PartDesc,
                                         Price = pa.PartPrice,
                                         Images = context.PartImages.Where(x => x.Pid == pa.Pid && x.PiIsTD == false).Select(x => urlpath + Url.Content($"/PartImgs/{x.PiFilename}")).ToList(),
                                         lvdatetime = sHis.SerachedOn
                                     }).OrderByDescending(x => x.lvdatetime).ToListAsync();
            var lastvisited1 = (from lv in lastvisited
                                group lv by new { lv.ID } into hj
                                select new
                                {
                                    ID = hj.Key.ID,
                                    Part = hj.Select(x => x.Part).FirstOrDefault(),
                                    Description = hj.Select(x => x.Description).FirstOrDefault(),
                                    Price = hj.Select(x => x.Price).FirstOrDefault(),
                                    Images = hj.Select(x => x.Images).FirstOrDefault()
                                }).Take(10).ToList();

            var lastsrched = await (from sHis in context.SearchViewHistory
                                    where sHis.searchterm != null
                                    select new
                                    {
                                        srches = sHis.searchterm
                                    }).Distinct().ToListAsync();
            var data = new { lstVistited = lastvisited1, srchhis = lastsrched };

            return Ok(new Response<object>(data));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<object>() { Errors = new string[] { ex.Message.ToString() } });
        }

    }

    [Authorize]
    [HttpGet]
    [Route("[controller]/[action]")]
    public async Task<IActionResult> getUserdetails()
    {
        var data = await getCurrentUser();
        if (data is not null)
        {
            return Ok(new Response<TokenUserDetails>(data));
        }
        return BadRequest(new Response<object>("Invalid Token", false));
    }

    [NonAction]
    private string GetContentType(string path)
    {
        var provider = new FileExtensionContentTypeProvider();
        string contentType;

        if (!provider.TryGetContentType(path, out contentType))
        {
            contentType = "application/octet-stream";
        }

        return contentType;
    }
    [NonAction]
    public async Task<TokenUserDetails> getCurrentUser()
    {
        var identity = User.Identity as ClaimsIdentity;
        var usid = identity.Claims.Cast<Claim>().Where(x => x.Type == "UserID").FirstOrDefault()?.Value;
        if (usid is not null)
        {
            var user = await unitOfWork.Users.GetUserbyCondition(x => x.UsID == Guid.Parse(usid));
            var userDetails = mapper.Map<TokenUserDetails>(user);
            return userDetails;
        }
        return null;
    }
}
