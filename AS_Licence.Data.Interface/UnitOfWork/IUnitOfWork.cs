﻿using AS_Licence.Data.Interface.DataAccess;
using AS_Licence.Entities.ViewModel.Operations;

namespace AS_Licence.Data.Interface.UnitOfWork
{
  public interface IUnitOfWork
  {
    ICustomerDal CustomerRepository { get; }
    ICustomerComputerInfoDal CustomerComputerInfoRepository { get; }
    ISoftwareDal SoftwareRepository { get; }
    ISubscriptionDal SubscriptionRepository { get; }


    OperationResponse<string> Save();
  }
}