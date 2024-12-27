using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.ServiceLayer
{
   public static class ObjectMapper
   {
        /*
       *Aşağıdaki lazy işlemi ile nesne üretimi sadece istenildiğinde gerçekleşir.İnitialize edildiğindde bellekte
        yer kaplamaz ve performansı arttırır.
        *Bu işlem ile artık DtoMapper sınıfı içerisinde oluşturduğumuz Mapping lazy load olarak çalışır.Artık
         ObjectMapper.Mapper şeklinde çağırıldığında DtoMapper sınıfı içerisindeki mapping işlemi çalışır.Mapper
         çağrıldığında method çalışır bu işlen lazy class'ı ile gerçekleşir.
         */
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMapper>();
            });
            return config.CreateMapper();
        });

        public static IMapper Mapper => lazy.Value;
    }
}
