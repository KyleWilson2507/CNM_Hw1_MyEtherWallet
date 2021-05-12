using Blockchain_MyEtherWallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blockchain_MyEtherWallet.Controllers
{
    public class HomeController : Controller
    {
        const string minerAdd = "miner1@gmail.com";
        const string adminAdd = "admin@gmail.com";
        const string userAdd = "user@gmail.com";

        private static BlockChain BChain = new BlockChain(10, 2);
        private static bool IsLoaded = false;
        private static List<string> EmailList = new List<string>() { userAdd, adminAdd };
        private static List<string> PasswordList = new List<string>() { "user", "admin" };
        private static string AccountType = "";

        public ActionResult Index()
        {
            if (!IsLoaded)
            {
                this.LoadTransactions();
                IsLoaded = true;
            }
            ViewBag.AllBlocks = BChain.GetAll();
            return View("Index");
        }

        private void LoadTransactions()
        {
            BChain.NewTransaction(new Transaction(adminAdd, userAdd, 200));
            BChain.MineBlock(minerAdd);
        }

        public ActionResult NewWallet()
        {
            ViewBag.Alert = "";
            ViewBag.Account = AccountType;
            return View("NewWallet");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWallet(User u)
        {
            if (EmailList.Contains(u.Email))
            {
                ViewBag.Alert = "Email is already taken";
                ViewBag.Account = AccountType;
                return View("NewWallet");
            }
            else
            {
                EmailList.Add(u.Email);
                PasswordList.Add(u.Password);
                AccountType = u.Email;
                ViewBag.AllBlocks = BChain.GetAll();
                return View("Index");
            }
        }

        public ActionResult AccountDetails()
        {
            if (AccountType == "")
            {
                ViewBag.Account = AccountType;
                ViewBag.Coins = 0;
                return View("Details");
            }
            else
            {
                ViewBag.Account = AccountType;
                ViewBag.Coins = BChain.GetBalance(AccountType);
                return View("Details");
            }
        }

        public ActionResult Transfer()
        {
            ViewBag.Account = AccountType;
            ViewBag.Alert = "";
            return View("Transfer");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransferClick(TransactionsInWallet walltrans)
        {
            if (!EmailList.Contains(walltrans.WalletName))
            {
                ViewBag.Account = AccountType;
                ViewBag.Alert = "Cannot find the target wallet!";
                return View("Transfer");
            }
            else
            {
                var mon = int.Parse(walltrans.WalletBalance);
                var balance = BChain.GetBalance(AccountType);

                if (balance <= mon)
                {
                    ViewBag.Account = AccountType;
                    ViewBag.Alert = "Money transfered must not be over" + " (" + balance.ToString() + " coins) !";

                    return View("Transfer");
                }
                else
                {
                    BChain.NewTransaction(new Transaction(AccountType, walltrans.WalletName, mon));
                    BChain.MineBlock(minerAdd);

                    ViewBag.AllBlocks = BChain.GetAll(); 

                    return View("Index");
                }
            }
        }


        public ActionResult History()
        {
            ViewBag.AllTransaction = BChain.GetTransactionInfo();

            return View("History");
        }

        public ActionResult About()
        {
            ViewBag.Message = "My Simple Etherwallet";

            return View("About");
        }

        public ActionResult Login()
        {
            ViewBag.Account = AccountType;
            ViewBag.Alert = "";

            return View("Login");
        }

        // post, check login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckLogin(User u)
        {
            if (EmailList.Contains(u.Email))
            {
                var idx = EmailList.IndexOf(u.Email);
                var pass = PasswordList[idx];

                if (u.Password == pass)
                {
                    AccountType = u.Email;

                    ViewBag.AllBlocks = BChain.GetAll();

                    return View("Index");
                }
                else
                {
                    ViewBag.Account = AccountType;
                    ViewBag.Alert = "Wrong password";
                    return View("Login");
                }
            }
            else
            {
                ViewBag.Account = AccountType;
                ViewBag.Alert = "Wrong email";

                return View("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            AccountType = "";
            ViewBag.Account = AccountType;
            ViewBag.Alert = "";
            return View("Login");
        }
    }
}