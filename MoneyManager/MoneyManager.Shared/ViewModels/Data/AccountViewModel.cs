﻿using MoneyManager.Models;
using MoneyManager.Src;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.ViewModels.Data
{
    [ImplementPropertyChanged]
    public class AccountViewModel : AbstractDataAccess<Account>
    {
        private TransactionViewModel TransactionViewModel
        {
            get { return new TransactionViewModel(); }
        }

        public Account SelectedAccount { get; set; }

        public ObservableCollection<Account> AllAccounts { get; set; }

        public double TotalBalance
        {
            get
            {
                return AllAccounts != null ?
                    AllAccounts.Sum(x => x.CurrentBalance)
                    : 0;
            }
        }

        protected override void SaveToDb(Account account)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (AllAccounts == null)
                {
                    AllAccounts = new ObservableCollection<Account>();
                }

                AllAccounts.Add(account);
                account.Id = dbConn.Insert(account);
            }
        }

        protected override void DeleteFromDatabase(Account account)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                TransactionViewModel.DeleteAssociatedTransactionsFromDatabase(account.Id);

                AllAccounts.Remove(account);
                dbConn.Delete(account);
            }
        }

        protected override sealed void GetListFromDb()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AllAccounts = new ObservableCollection<Account>(dbConn.Table<Account>().ToList());
            }
        }

        protected override void UpdateItem(Account account)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.Update(account, typeof(Account));
            }
        }

        public void AddTransactionAmount(FinancialTransaction transaction)
        {
            if (transaction.ClearTransactionNow)
            {
                var account = AllAccounts.FirstOrDefault(x => x.Id == transaction.ChargedAccountId);
                if (account == null) return;
                account.CurrentBalance += transaction.Amount;
                transaction.Cleared = true;
                UpdateItem(account);
            }
        }
    }
}