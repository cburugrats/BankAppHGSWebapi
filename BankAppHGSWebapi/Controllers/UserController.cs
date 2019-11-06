using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAppHGSWebapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAppHGSWebapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		#region Register
		// POST api/user
		[HttpPost]
		public int PostRegister([FromBody] HgsUser userModel)
		{
			if (userModel.HgsNo.Equals(null))
			{
				return 0;//HgsNo boş bırakılamaz!
			}
			else if (userModel.balance.Equals(null))
			{
				return 2;//Balance boş bırakılamaz!
			}
			else if (userModel.balance > decimal.MaxValue || userModel.balance < 1)
			{
				return 3;//Geçersiz bir para miktarı girdiniz!
			}
			using (var db = new RugratsHgsDbContext())
			{
				int HgsNo;
				var count = (from o in db.Users
							 select o).Count();
				if (count > 0)
				{
					var lastRecordHgsNo = db.Users.Max(x => x.HgsNo);
					HgsNo = lastRecordHgsNo + 1;
				}
				else
					HgsNo = 1000;			
				try
				{
					var execSpUers = db.Database.ExecuteSqlCommand("exec [sp_Users] {0},{1}", HgsNo, userModel.balance);
					db.SaveChanges();
					return HgsNo;
				}
				catch (Exception)
				{
					return 4;//Veritabanına kaydedilirken hata oluştu!
				}
			}
		}
		#endregion Register

		#region To Deposit Money Hgs
		// PUT api/user/toDepositMoney
		[HttpPut("toDepositMoney")]
		public int PutToDepositMoney([FromBody] HgsUser userModel)
		{
			if ((!(userModel.HgsNo > 0))|| userModel.HgsNo>int.MaxValue)
			{
				return 0;//Geçersiz bir HgsNo girdiniz!
			}
			else if ((!(userModel.balance > 0) || userModel.balance>int.MaxValue))
			{
				return 3;//Geçersiz bir para miktarı girdiniz!
			}
			using (var db = new RugratsHgsDbContext())
			{
				HgsUser tempUser = db.Users.Where(x => x.HgsNo == userModel.HgsNo).FirstOrDefault();
				if (tempUser != null)
				{
					var execSpParaYatir = db.Database.ExecuteSqlCommand("exec [sp_ParaEkle] {0},{1}", userModel.HgsNo, userModel.balance);
					try
					{
						db.SaveChanges();
						return 1;//Güncelleme işlemi başarılı!
					}
					catch (Exception)
					{
						return 4;//Veritabanına kaydedilirken hata oluştu!
					}
				}
				else
				{
					return 2;//Bu HgsNo'ye bağlı bir hgs kaydı bulunamadı!
				}
			}
		}

		#endregion To Deposit Money Hgs

		#region With Draw Money Hgs
		// PUT api/user/toDepositMoneym
		[HttpPut("withDrawMoney")]
		public int PutWithDrawMoney(int id, [FromBody] HgsUser userModel)
		{
			if ((!(userModel.HgsNo > 0)) || userModel.HgsNo > int.MaxValue)
			{
				return 0;//Geçersiz bir HgsNo girdiniz!
			}
			else if ((!(userModel.balance > 0) || userModel.balance > int.MaxValue))
			{
				return 3;//Geçersiz bir para miktarı girdiniz!
			}
			using (var db = new RugratsHgsDbContext())
			{
				HgsUser tempUser = db.Users.Where(x => x.HgsNo == userModel.HgsNo).FirstOrDefault();
				if (tempUser != null)
				{
					var execSpParaCek = db.Database.ExecuteSqlCommand("exec [sp_ParaSil] {0},{1}", userModel.HgsNo, userModel.balance);
					try
					{
						db.SaveChanges();
						return 1;//Güncelleme işlemi başarılı!
					}
					catch (Exception)
					{
						return 4;//Veritabanına kaydedilirken hata oluştu!
					}
				}
				else
				{
					return 2;//Bu HgsNo'ye bağlı bir hgs kaydı bulunamadı!
				}
			}
		}

		#endregion With Draw Money

		#region Get HgsUser By HgsNo
		// GET api/getaccountbyid/5
		[HttpGet("{HgsNo}")]
		public HgsUser GetById(int HgsNo)
		{
			if (!(HgsNo > 0))
			{
				return null;//Geçersiz bir HgsNo girdiniz!
			}
			HgsUser user;
			using (var db = new RugratsHgsDbContext())
			{
				user = db.Users.Where(x => x.HgsNo == HgsNo).FirstOrDefault();
				if (user != null)
				{
					return user;
				}
				else
					return null;
			}
		}
		#endregion Get User By HgsNo
	}
}