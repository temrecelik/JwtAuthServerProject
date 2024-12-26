using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Shared.Dtos
{
    /*
     *ErrorDto ile API'lerde oluşan hatalar için dönecek bir hatalar dto tipinde döndürülürse kullanıcıya daha ölçeklendirilebilinir bir 
      deneyim sunulur.Bu hatalar validation hataları, token süresi hataları gibi hatalar olabilir. Hatalar tüm API'lerde ortak bir yapı 
      olduğu için shared  klasöründe tutmak mantıklıdır.

     *Property'lerin set kısmını private'a çektiğimizde kullanıcı direkt olarak bir dto'yu oluşturamaz oluşturması için constructor methodları
      çağırması gereklidir.
    */
    public class ErrorDto
    {
        public List<string> Errors { get; private set; }

        /*
        Validation hataları kullanıcya gösterilirken bazı hatalar sadece geliştiricinin görmesi gereken hatalardır. Bu nedenle
        Ishow property'si hatanın kullanıcıya gösterilip gösterilmemesini tutar.
        */
        public bool IsShow {  get; private set; }

      
        //bu constuctor ile bir adet ErrorDto nesnesi oluşturup property'lerini bu nesne üzerinden verebiliriz.
        public ErrorDto() { 
            Errors = new List<string>();
        }

        //hatalar bir list değilde tek bir hata olarak eklenirse bu hata için oluşacak Dto'nun constructor methodu
        public ErrorDto(string error,bool isShow)
        {
            Errors.Add(error);
            IsShow = isShow;
        }

        public ErrorDto(List<string> error, bool isShow)
        {
            Errors = error;
            IsShow =isShow;
        }
    }
}
