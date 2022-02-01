using AutoMapper;
using dbclient.data.EF;
using dbclient.data.EF.Validators;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stkpnt.Contracts;
using System;

namespace dbclient.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserValidator _validator;
        private readonly IPublishEndpoint _publishEndPoint;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IPublishEndpoint publishEndPoint, IMapper mapper)
        {
            _logger = logger;
            _publishEndPoint = publishEndPoint;
            _mapper = mapper;
            _validator = new UserValidator();
        }

        private bool IsValidData(User user)
        {
            var result = _validator.Validate(user);
            if (result.IsValid)
            {
                return true;
            }

            foreach (var error in result.Errors)
            {
                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} User validation errors: {error.ErrorMessage}");
            }
            return false;
        }

        internal class Message
        {
            public string Text { get; set; }
        }

        [HttpPost("Add/{name},{surname},{patronymic},{email}")]
        public ActionResult Add(string name, string surname, string patronymic, string email)
        {
            var u = new User { Name = name, Surname = surname, Patronymic = patronymic, Guid = Guid.NewGuid(), Email = email };
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Add ({u})");
            if (IsValidData(u))
            {

                var configuration = new MapperConfiguration(cfg =>
                    cfg.CreateMap<User, ApplicationUserAdd>()
                    .ForMember(to => to.Id, conf => conf.MapFrom(ol => ol.Guid)));
                
                //_publishEndPoint.Publish(u);
                
                _publishEndPoint.Publish(new ApplicationUserAdd
                {
                    Id = u.Guid,
                    Name = u.Name,
                    Surname = u.Surname,
                    Patronymic = u.Patronymic,
                    Email = u.Email
                });
                
            }
            else
            {
                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Adding aborted!!!");
                return BadRequest("Incorect params");
            }
            return Ok();
        }

    }
}
