using AuthServer.CoreLayer.Repositories;
using AuthServer.CoreLayer.Services;
using AuthServer.CoreLayer.UnitOfWork;
using AuthServer.Shared.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.ServiceLayer.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }

        public async Task<Response<TDto>> AddAsync(TDto dto)
        {
           var newEntity = ObjectMapper.Mapper.Map<TEntity>(dto);
           await _genericRepository.AddAsync(newEntity);
           await _unitOfWork.CommitAsync();

           var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

           return Response<TDto>.Success(newDto, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var Dtos = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());
            return Response<IEnumerable<TDto>>.Success(Dtos, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Response<TDto>.Fail("Id not Found",404,true); ;
            }
            return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(entity), 200);
        }

        public async Task<Response<NoDataDto>> Remove(int Id)
        {
            var entity =await _genericRepository.GetByIdAsync(Id);
            if(entity == null)
            {
                return Response<NoDataDto>.Fail("Id not Found", 404, true);
            }
            _genericRepository.Remove(entity);
            _unitOfWork.Commit();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<NoDataDto>> Update(TDto dto, int Id)
        {
            /*GetByIdAsync repository'sini kodlarken state'i detached işaretledik böylece memory'de bu entity'nin
              track oluyunu kaldırdık yani memory'de Id'si işaratlenmedi alttaki update methodunda ise state'i modified
              yaptığımız için bu sefer aynı entity'i memory'de işaretlenmiş oldu eğer ikisi aynı anda işaretli olsaydı
              aynı id'li entity memory'de iki kez işaretlenmiş olacaktı ve bu durumda hata alacaktık.
             */
            var isExistentity = await _genericRepository.GetByIdAsync(Id);
            if (isExistentity == null)
            {
                return Response<NoDataDto>.Fail("Id not Found", 404, true);
            }
            var updateEntity = ObjectMapper.Mapper.Map<TEntity>(dto);
            _genericRepository.Update(updateEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);//204 success kodunda body'de data olmaz.
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            /*Tolist() demeden önce daha fazla sorgu yazılabilir. Tolist methodundan sonra veri tabanından veri alınır.
              döndürülen veri ise artık IEnumerable olur*/
            var list =await _genericRepository.Where(predicate).ToListAsync();
            return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(list),200);
        }
    }
}
