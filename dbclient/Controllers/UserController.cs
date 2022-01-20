using dbclient.data.EF;
using dbclient.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using dbclient.Connections;
using System.Text;

namespace dbclient.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserValidator _validator;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            _validator = new UserValidator();
        }

        private bool isValidData(User user)
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

        [HttpPost("Add/{name},{surname},{patronymic}")]
        public ActionResult Add(string name, string surname, string patronymic)
        {
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Add({name})");

            var u = new User { Name = name, Surname = surname, Patronymic = patronymic };
            if (isValidData(u))
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "rabbit_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    var body = Encoding.UTF8.GetBytes($"Add user;{u.Name};{u.Surname};{u.Patronymic}");
                    channel.BasicPublish(exchange: "", routingKey: "rabbit_queue", basicProperties: null, body: body);
                }
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
