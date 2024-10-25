using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StargateAPI.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IMediator mediator, ILogger<PersonController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPeople()
        {
            try
            {
                _logger.LogInformation("{0}:\n Getting List of People.\n\n", DateTime.Now.ToString());
                var result = await _mediator.Send(new GetPeople()
                {

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

                var resp = this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });

                return resp;
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetPersonByName(string name)
        {
            try
            {
                _logger.LogInformation("{0}:\n Getting Person By Name.\nName: {1}\n\n", DateTime.Now.ToString(), name);
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
        public async Task<IActionResult> CreatePerson([FromBody] string name)
        {
            try
            {
                _logger.LogInformation("{0}:\n Creating Person.\nName: {1}\n\n", DateTime.Now.ToString(), name);

                var result = await _mediator.Send(new CreatePerson()
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
    }
}