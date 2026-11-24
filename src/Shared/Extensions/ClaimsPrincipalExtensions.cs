using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Shared.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
		public static bool TryParseUserId (this ClaimsPrincipal user, out Guid userId)
		{
			Claim? userIdClaim = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
			if ( userIdClaim != null 
				&& Guid.TryParse(userIdClaim.Value, out Guid result) )
			{
				userId = result;
				return true;
			}
			userId = Guid.Empty;
			return false;
		}
	}
}
