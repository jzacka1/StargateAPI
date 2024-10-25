using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using System.Net;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AstronautDutyController> _logger;
        public AstronautDutyController(IMediator mediator, ILogger<AstronautDutyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAstronautDutiesByName(string name)
        {
            try
            {
                _logger.LogInformation("{0}:\n Getting List of Astronaut Duties.\n\n", DateTime.Now.ToString());
                var result = await _mediator.Send(new GetPersonByName()
                {
                    Name = name
                });

                var resp = this.GetResponse(result);

                _logger.LogInformation("{0}:\n Response Successful.\n\n", DateTime.Now.ToString());

                return resp;
            }
            catch (Exception ex)
            {
                var error = new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                };

                _logger.LogError("{0}:\nMessage - {1}\nSuccess - {2}\nResponseCode - {3}\n\n", DateTime.Now.ToString(), error.Message, error.Success, error.ResponseCode);

                var resp = this.GetResponse(error);

                return resp;
            }            
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDuty request)
        {
            try
            {
                _logger.LogInformation("{0}:\n Creating Astronaut Duty.\n\n", DateTime.Now.ToString());
                var result = await _mediator.Send(request);
                return this.GetResponse(result);
            }
            catch(Exception ex){
                _logger.LogError("{0}:\nMessage - {1}\n\n", DateTime.Now.ToString(), ex.Message);
            }

            return await Task.FromResult<IActionResult>(null);
        }
    }
}