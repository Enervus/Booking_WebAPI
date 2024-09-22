using Booking.Domain.Dto.RoleDto;
using Booking.Domain.Dto.UserRoleDto;
using Booking.Domain.Entity;
using Booking.Domain.Interfaces.Services;
using Booking.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Booking.Api.Controllers
{
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Создание роли
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        ///    Sample request:
        ///    
        ///        POST
        ///        {
        ///            "name": "User",
        ///        }
        /// </remarks>
        /// <response code = "200">Если роль создалась</response>
        /// <response code = "400">Если роль не была создана</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<RoleDto>>> Create([FromBody] CreateRoleDto dto)
        {
            var response = await _roleService.CreateRoleAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }


        /// <summary>
        /// Добавление роли пользователю
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        ///    Sample request:
        ///    
        ///        POST
        ///        {
        ///            "login": "User#1",
        ///            "roleName": "User"
        ///        }
        /// </remarks>
        /// <response code = "200">Если роль была добавлена</response>
        /// <response code = "400">Если роль не была добавлена</response>
        [HttpPost("addRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserRoleDto>>> AddRoleForUser([FromBody] UserRoleDto dto)
        {
            var response = await _roleService.AddRoleForUserAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Удаление роли у пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        ///    Sample request:
        ///    
        ///        DELETE
        ///        {
        ///            "login": "User#1",
        ///            "roleId": 1
        ///        }
        /// </remarks>
        /// <response code = "200">Если роль была удалена у пользователя</response>
        /// <response code = "400">Если роль не была удалена у пользователя</response>
        [HttpDelete("deleteRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserRoleDto>>> DeleteRoleForUser([FromBody] DeleteUserRoleDto dto)
        {
            var response = await _roleService.DeleteRoleForUserAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Обновление роли у пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        ///    Sample request:
        ///    
        ///        PUT
        ///        {
        ///            "login": "User#1",
        ///            "oldRoleId": 1,
        ///            "newRoleId": 2,
        ///        }
        /// </remarks>
        /// <response code = "200">Если роль была обнавлена у пользователя</response>
        /// <response code = "400">Если роль не была обнавлена у пользователя</response>
        [HttpPut("updateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserRoleDto>>> UpdateRoleForUser([FromBody] UpdateUserRoleDto dto)
        {
            var response = await _roleService.UpdateRoleForUserAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Обновление роли
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        ///    Sample request:
        ///    
        ///        PUT
        ///        {
        ///            "id":1
        ///            "name": "User",
        ///        }
        /// </remarks>
        /// <response code = "200">Если роль обновилась</response>
        /// <response code = "400">Если роль не была обновлена</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<RoleDto>>> Update([FromBody] RoleDto dto)
        {
            var response = await _roleService.UpdateRoleAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Удаление роли
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        ///    Sample request:
        ///    
        ///        DELETE
        ///        {
        ///            "id": 1,
        ///        }
        /// </remarks>
        /// <response code = "200">Если роль удалилась</response>
        /// <response code = "400">Если роль не была удалена</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<RoleDto>>> Delete(long id)
        {
            var response = await _roleService.DeleteRoleAsync(id);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
