﻿using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.Interfaces
{
    /// <summary>
    ///     Represents the Actions for a view.
    ///     On Windows this is a normaly in the app bar. 
    ///     On Android for example in a floating action button.
    /// </summary>
    public interface IPaymentListViewActionViewModel : IViewActionViewModel
    {
        /// <summary>
        ///     Deletes the currently selected account.
        /// </summary>
        MvxAsyncCommand DeleteAccountCommand { get; }

        /// <summary>
        ///      Indicates wether the filter for only cleared Payments is active or not.
        /// </summary>
        bool IsClearedFilterActive { get; set; }

        /// <summary>
        ///      Indicates wether the filter to only display recurring Payments is active or not.
        /// </summary>
        bool IsRecurringFilterActive { get; set; }
    }
}