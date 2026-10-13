using Microsoft.AspNetCore.Mvc;
using UserApi.Dtos;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("api/users")]
[Tags("Users")]
[Produces("application/json")]
public class UserRestController(UserService userService) : ControllerBase
{

    [HttpGet]
    [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointSummary("Список пользователей за период")]
    [EndpointDescription("Возвращает пользователей, созданных в диапазоне [startDateTime; entDateTime] включительно.")]
    public async Task<ActionResult<List<UserResponse>>> GetBetween(
        [FromQuery] UsersBetweenForm form,
        CancellationToken cancellationToken)
    {
        var users = await userService.GetAllUsersBetweenAsync(form, cancellationToken);
        return Ok(users);
    }

    [HttpPost("register")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [EndpointSummary("Регистрация пользователя")]
    [EndpointDescription("Создаёт нового пользователя. Возвращает 409, если username уже занят.")]
    public async Task<IActionResult> Register(
        [FromBody] UserRegisterForm form,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await userService.AddUserAsync(form, cancellationToken);
            return CreatedAtAction(
                nameof(Register),
                new { username = user.Username },
                user);
        }
        catch (ArgumentNullException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException)
        {
            return Conflict(new { message = "Username already taken." });
        }
    }


    [HttpPut]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("Обновление пользователя")]
    [EndpointDescription("Обновляет возраст и/или пароль. Требует подтверждения текущим паролем.")]
    public async Task<IActionResult> Update(
        [FromBody] UserUpdateForm form,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await userService.UpdateUserAsync(form, cancellationToken);
            if (user is null) return NotFound();
            return Ok(user);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    
    [HttpDelete]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("Удаление пользователя")]
    [EndpointDescription("Удаляет пользователя после подтверждения паролем.")]
    public async Task<IActionResult> Delete(
        [FromBody] UserDeleteForm form,
        CancellationToken cancellationToken)
    {
        try
        {
            await userService.DeleteUserAsync(form, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    
    [HttpPost("login")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [EndpointSummary("Вход в систему")]
    [EndpointDescription("Проверяет username/password. При успехе возвращает UserResponse.")]
    public async Task<IActionResult> Login(
        [FromBody] UserLoginForm form,
        CancellationToken cancellationToken)
    {
        var user = await userService.LoginAsync(form, cancellationToken);
        if (user is null) return Unauthorized();
        return Ok(user);
    }
}