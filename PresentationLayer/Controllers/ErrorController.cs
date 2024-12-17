using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
	/// <summary>
	/// Error Controller
	/// </summary>
	public class ErrorController : Controller
	{
		/// <summary>
		/// Error
		/// </summary>
		/// <returns></returns>
		[Route("Error")]
		public IActionResult Error()
		{
			var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

			if (exceptionFeature != null)
			{
				// exceptionFeature.Error
			}
			return View();
		}

		/// <summary>
		/// Throw Error
		/// </summary>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public IActionResult ThrowError()
		{
			throw new Exception("Launch Exception Tester.");
		}

	}
}
