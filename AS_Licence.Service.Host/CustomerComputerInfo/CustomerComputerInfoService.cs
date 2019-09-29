﻿using AS_Licence.Data.Interface.UnitOfWork;
using AS_Licence.Entites.Validation.Customer;
using AS_Licence.Entities.ViewModel.Operations;
using AS_Licence.Helpers.Extension;
using AS_Licence.Service.Interface.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AS_Licence.Entites.Validation.CustomerComputerInfo;

namespace AS_Licence.Service.Host.CustomerComputerInfo
{

  public class CustomerComputerInfoService : ICustomerComputerInfoManager
  {
    private readonly IUnitOfWork _unitOfWork;

    public CustomerComputerInfoService(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public OperationResponse<List<Entities.Model.CustomerComputerInfo.CustomerComputerInfo>> GetCustomerComputerInfoList(Expression<Func<Entities.Model.CustomerComputerInfo.CustomerComputerInfo, bool>> filter = null, Func<IQueryable<Entities.Model.CustomerComputerInfo.CustomerComputerInfo>, IOrderedQueryable<Entities.Model.CustomerComputerInfo.CustomerComputerInfo>> orderBy = null, string includeProperties = "")
    {
      OperationResponse<List<Entities.Model.CustomerComputerInfo.CustomerComputerInfo>> response = new OperationResponse<List<Entities.Model.CustomerComputerInfo.CustomerComputerInfo>>();
      try
      {
        response.Data = _unitOfWork.CustomerComputerInfoRepository.Get(filter, orderBy, includeProperties);
        response.Status = true;
      }
      catch (Exception e)
      {
        response.Status = false;
        response.Message = e.Message;
      }
      return response;
    }

    public OperationResponse<Entities.Model.CustomerComputerInfo.CustomerComputerInfo> SaveCustomerComputerInfo(Entities.Model.CustomerComputerInfo.CustomerComputerInfo customer)
    {
      OperationResponse<Entities.Model.CustomerComputerInfo.CustomerComputerInfo> response = new OperationResponse<Entities.Model.CustomerComputerInfo.CustomerComputerInfo>();

      try
      {
        if (customer == null)
        {
          throw new Exception("Customer nesnesi null olamaz");
        }

        var valid = new CustomerComputerInfoValidator().Validate(customer);
        if (valid.IsValid == false)
        {
          throw new Exception(valid.GetErrorMessagesOnSingleLine());
        }

        if (customer.CustomerComputerInfoId > 0)
        {
          var existsCustomer = _unitOfWork.CustomerComputerInfoRepository.GetById(customer.CustomerComputerInfoId);
          if (existsCustomer == null)
          {
            throw new Exception("Sistemde kayıtlı bir müşteri bilgisi bulunamadı.");
          }
        }

        if (customer.CustomerComputerInfoId > 0)
        {
          customer.UpdatedDateTime = DateTime.Now;
          _unitOfWork.CustomerComputerInfoRepository.Update(customer);
        }
        else
        {
          customer.CreatedDateTime = DateTime.Now;
          _unitOfWork.CustomerComputerInfoRepository.Insert(customer);
        }

        response.Data = customer;
        var responseUnitOfWork = _unitOfWork.Save();
        response.Status = responseUnitOfWork.Status;
        response.Message = responseUnitOfWork.Message;
      }
      catch (Exception e)
      {
        response.Status = false;
        response.Message = e.Message;
      }

      return response;
    }

    public OperationResponse<Entities.Model.CustomerComputerInfo.CustomerComputerInfo> DeleteCustomerComputerInfoByCustomerComputerInfoId(int id)
    {
      OperationResponse<Entities.Model.CustomerComputerInfo.CustomerComputerInfo> response = new OperationResponse<Entities.Model.CustomerComputerInfo.CustomerComputerInfo>();

      try
      {
        var existsCustomer = _unitOfWork.CustomerComputerInfoRepository.GetById(id);
        if (existsCustomer == null)
        {
          throw new Exception("Sistemde kayıtlı bir müşteri bilgisi bulunamadı.");
        }

        _unitOfWork.CustomerComputerInfoRepository.Delete(existsCustomer);
        var responseUnitOfWork = _unitOfWork.Save();
        response.Status = responseUnitOfWork.Status;
        response.Message = responseUnitOfWork.Message;
      }
      catch (Exception e)
      {
        response.Status = false;
        response.Message = e.Message;
      }
      return response;
    }

    public OperationResponse<Entities.Model.CustomerComputerInfo.CustomerComputerInfo> GetByCustomerComputerInfoId(int id)
    {
      OperationResponse<Entities.Model.CustomerComputerInfo.CustomerComputerInfo> response = new OperationResponse<Entities.Model.CustomerComputerInfo.CustomerComputerInfo>();

      try
      {
        var existsCustomer = _unitOfWork.CustomerComputerInfoRepository.GetById(id);
        response.Data = existsCustomer ?? throw new Exception("Sistemde kayıtlı bir müşteri bilgisi bulunamadı.");
        response.Status = true;
      }
      catch (Exception e)
      {
        response.Status = false;
        response.Message = e.Message;
      }
      return response;
    }

    public OperationResponse<List<Entities.Model.CustomerComputerInfo.CustomerComputerInfo>> GetByCustomerComputerInfoListBySubscriptionId(int SubscriptionId)
    {
      OperationResponse<List<Entities.Model.CustomerComputerInfo.CustomerComputerInfo>> response = new OperationResponse<List<Entities.Model.CustomerComputerInfo.CustomerComputerInfo>>();
      try
      {
        response.Data = _unitOfWork.CustomerComputerInfoRepository.Get(x => x.SubscriptionId == SubscriptionId, o => o.OrderBy(info => info.CreatedDateTime));
        response.Status = true;
      }
      catch (Exception e)
      {
        response.Status = false;
        response.Message = e.Message;
      }
      return response;
    }

    public OperationResponse<int> GetAlreadyComputerCountsBySubscriptionId(int subscriptionId)
    {
      OperationResponse<int> response = new OperationResponse<int>();

      try
      {
        var customerComputerInfoListResult = this.GetCustomerComputerInfoList(x => x.SubscriptionId == subscriptionId);
        if (customerComputerInfoListResult.Status == false)
        {
          throw new Exception(customerComputerInfoListResult.Message);
        }

        response.Status = true;
        response.Data = customerComputerInfoListResult.Data.Count;
      }
      catch (Exception e)
      {
        response.Status = false;
        response.Message = e.Message;
      }
      return response;
    }

    public OperationResponse<Entities.Model.CustomerComputerInfo.CustomerComputerInfo> GetByCustomerComputerHddAndMacAndProcessSerialCode(string hddCode, string macCode, string processCode)
    {
      OperationResponse<Entities.Model.CustomerComputerInfo.CustomerComputerInfo> response = new OperationResponse<Entities.Model.CustomerComputerInfo.CustomerComputerInfo>();

      try
      {
        var existsCustomer = _unitOfWork.CustomerComputerInfoRepository.GetByCustomerComputerHddAndMacAndProcessSerialCode(hddCode, macCode, processCode);
        response.Data = existsCustomer ?? throw new Exception("Sistemde kayıtlı bir müşteri bilgisayar bilgisi bulunamadı.");
        response.Status = true;
      }
      catch (Exception e)
      {
        response.Status = false;
        response.Message = e.Message;
      }
      return response;
    }
  }
}
