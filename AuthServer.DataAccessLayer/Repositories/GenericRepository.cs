using AuthServer.CoreLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.DataAccessLayer.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        /*
        DbContext olarak  oluşturduğumuz AppdBContext'i verdik.AppDbContext DbContext'i miras aldığı için _context field'ı
        oluşturuken direkt olarak DbContext'ten oluşturduk ancak consructur'da bu context'in AppDbContext'ten oluşturduk
        Dbset'i de contextten oluşturup verdik. Dbset ile veritabanında tabloara erişim sağlanarak get işlemi yapılır.

        Aşağıdaki işlemler bittiğinde henüz  saveChange methodu çağrılmadığı için veritabanında bir değişiklik olmaz.
        UnitOfWork ile saveChange işlemi yapılır.
        */
        public GenericRepository(AppDbContext context, DbSet<TEntity> dbSet)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();    
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        //Tüm entity'leri getirdiği direkt olarak IQuerable dönmeye gerek yoktur.Direkt tolist'e çevirip tüm entity'leri
        //döneriz. Bu işlem ile veritabanından tüm veriler direkt olarak list olarak alınır.
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
          return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if(entity != null)
            {
                /*Eğer ilgili ID'ye ait  entity veri tabanında varsa bu entity'nin state durumunu detached yaparız.
                  böylece bu veri memory'da takip edilmez.*/
                _context.Entry(entity).State = EntityState.Detached;  
            }
            return entity;
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public TEntity Update(TEntity entity)
        {
            //Entity'nin state durumunu modified yaparız. Böylece bu entity'nin veritabanında güncelleneceğini belirtiriz.
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /*
        * quearable döndüğü için alınan datalar tekrar sorgulanabilir.Ve yeni bir list oluşturulabilir.Bu işlemi service
          katmanında sorgulanmış veriler istenirken kullanılır.Tüm sorgu bittikten sonra tolist() methodunu çağırdığımzda
          veri tabanından veriyi çeker.
        */
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
    }
}
