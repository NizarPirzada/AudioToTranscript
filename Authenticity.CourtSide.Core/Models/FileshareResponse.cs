using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Authenticity.CourtSide.Core.Models
{
	public class FileshareResponse
	{
		public Stream Stream { get; set; }
		public FtpWebResponse FtpWebResponse { get; set; }
	}
}
