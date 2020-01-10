using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankAppHGSWebapi.Models
{
	public class HgsUser
	{
		[Key]
		public int Id { get; set; }
		public long TcNo { get; set; }
		public long HgsNo { get; set; }
		public decimal balance { get; set; }
	}
}
