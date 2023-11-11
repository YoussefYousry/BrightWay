using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Contracts
{
	public interface IEmailService
	{
		Task SendPasswordResetEmailAsync(string email, string callbackUrl);
	}
}
