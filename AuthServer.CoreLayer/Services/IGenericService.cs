﻿using AuthServer.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.CoreLayer.Services
{
   
    public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<Response<TDto>> GetByIdAsync(int id);
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate); 
        Task<Response<TDto>> AddAsync(TDto dto);
        Task<Response<NoDataDto>> Remove(int Id);
        Task<Response<NoDataDto>> UpdateAsync(TDto dto,int Id);
    }
}
