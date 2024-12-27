using AuthServer.CoreLayer.Dtos;
using AuthServer.CoreLayer.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.ServiceLayer
{
    /*
    DtoMapper class'ı service katmanında yazılır. Profile classında miras alır business kodları yazarken entity'ler ile
    map'lanacak olan dto'ları burada birbiriyle eşleştirilir. Bu sayede entity'lerin dto'larına dönüşümü ve tam tersi de
    sağlanır. Proporty'ler birbiriyle eşleştirilir. Profile classı AutoMapper'den gelir. 
    Gelişmiş bir projede bir entity için getproductDto,addProductDto gibi farklı dto'lar oluşturulabilir.Burada biz jwt 
    odaklı olduğumuz için service kodlarımızı aşırı gelişmiş değildir. 
   
     *HerDto'ya ait bir entity'de olmayabilir örneğin CreateUserDto için bir entity oluşturulmaz.Çünkü bu token'lar 
     veritabanına kaydedilmez. Sadece oluşturulur ve client'a gönderilir.

    *Her entity için bir dto oluşturulmayadabilir. Örneğin userRefreshToken  bir entity'dir ancak client'tan gelmez
     direkt back-end'de oluşturulur ve veri tabanına kaydedilir. Bu nedenle userRefreshToken  için client ile iletişime 
     geçecek bir dto oluşturmaya gerek yoktur.
    */
    public class DtoMapper : Profile
    {
       public DtoMapper()
        {
          CreateMap<Product, ProductDto>().ReverseMap();
          CreateMap<UserApp, UserAppDto>().ReverseMap();
        }
    }
}
