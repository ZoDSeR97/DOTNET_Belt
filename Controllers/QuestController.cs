using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DOTNET_Belt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_Belt.Controllers;

public class QuestController : Controller
{
    private readonly ILogger<QuestController> _logger;

    MyContext _context;

    public QuestController(ILogger<QuestController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    [SessionCheck]
    [HttpGet("dashboard")]
    public IActionResult Index(){
        int? id = HttpContext.Session.GetInt32("UserId");
        User? Player = _context.Users.Include(u=>u.PostedQuests).ThenInclude(q=>q.Players)
            .Include(u=>u.TakenQuests).ThenInclude(p=>p.Quest).ThenInclude(q=>q.Players).FirstOrDefault(u=>u.UserId==id);
        return View("Index", Player);
    }
    [SessionCheck]
    [HttpGet("quests")]
    public IActionResult Quests(){
        int? id = HttpContext.Session.GetInt32("UserId");
        User? Player = _context.Users.FirstOrDefault(u=>u.UserId==id);

        List<Quest> AvailableQuests =  _context.Quests.Include(c=>c.Creator).Include(c=>c.Players)
            .Where(c=>c.Players.All(p=>p.UserId!=Player.UserId)&&c.Creator.UserId!=Player.UserId&&!c.Closed)
            .ToList();
        return View("Quests", AvailableQuests);
    }
    [SessionCheck]
    [HttpGet("quests/new")]
    public IActionResult NewQuest()=>View("NewQuest");
    [SessionCheck]
    [HttpPost("quests/new")]
    public IActionResult AddQuest(Quest q){
        if(ModelState.IsValid){
            q.UserId = HttpContext.Session.GetInt32("UserId")??0;
            _context.Add(q);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return NewQuest();
    }
    [SessionCheck]
    [HttpGet("quests/{id}/join")]
    public IActionResult JoinQuest(int id){
        int? UserId = HttpContext.Session.GetInt32("UserId");
        if(UserId!=null){
            Progress? p = _context.Progresses.FirstOrDefault(p=>p.QuestId==id && p.UserId == UserId);
            if(p==null){
                p = new Progress{UserId=(int)UserId, QuestId=id};
                _context.Add(p);
                _context.SaveChanges();
            }
        }
        return RedirectToAction("Quests");
    }
    [SessionCheck]
    [HttpGet("quests/{id}/close")]
    public IActionResult CloseQuest(int id){
        Quest? q = _context.Quests.FirstOrDefault(quest=>quest.QuestId==id);
        if(q!=null){
            q.Closed=true;
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
    [SessionCheck]
    [HttpGet("progress/{id}/{cmd}")]
    public IActionResult ProgressUpdate(int id, string cmd){
        int? UserId = HttpContext.Session.GetInt32("UserId");
        if(UserId!=null){
            Progress? p = _context.Progresses.Include(p=>p.Quest).FirstOrDefault(p=>p.ProgressId==id);
            if (p!=null && !p.Completed){
                switch(cmd){
                    case "leave":
                        _context.Remove(p);
                        _context.SaveChanges();
                        break;
                    case "complete":
                        p.Completed = true;
                        User Player = _context.Users.First(u=>u.UserId==UserId);
                        Player.Gold += p.Quest.Gold;
                        _context.SaveChanges();
                        break;
                    default:
                        Console.WriteLine("WoW How did that happen!?");
                        break;
                }
            }
        }
        return RedirectToAction("Index");
    }
    [HttpGet("logout")]
    public IActionResult Logout(){
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
    // Name this anything you want with the word "Attribute" at the end
    public class SessionCheckAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext context) {
            // Find the session, but remember it may be null so we need int?
            int? userId = context.HttpContext.Session.GetInt32("UserId");
            // Check to see if we got back null
            if(userId == null) {
                // Redirect to the Index page if there was nothing in session
                // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}