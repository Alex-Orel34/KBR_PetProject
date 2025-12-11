using System.Security.Claims;

namespace KBR.Extensions
{
    /// <summary>
    /// Расширения для упрощения работы с аутентификацией
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Получаем идентификатор пользователя
        /// </summary>
        /// <param name="user">ClaimsPrincipal пользователя</param>
        /// <returns>Guid идентификатор пользователя или null, если не найден</returns>
        public static Guid? GetUserId(this ClaimsPrincipal user)
        {
            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (userIdString == null || !Guid.TryParse(userIdString, out var userId))
            {
                return null;
            }
            
            return userId;
        }

        /// <summary>
        /// Получаем идентификатор пользователя
        /// </summary>
        /// <param name="user">ClaimsPrincipal пользователя</param>
        /// <returns>Guid идентификатор пользователя</returns>
        /// <exception cref="UnauthorizedAccessException">Если пользователь не аутентифицирован</exception>
        public static Guid GetUserIdOrThrow(this ClaimsPrincipal user)
        {
            var userId = user.GetUserId();
            
            if (userId == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            
            return userId.Value;
        }
    }
}

