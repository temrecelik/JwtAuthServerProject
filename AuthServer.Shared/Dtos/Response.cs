using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Shared.Dtos
{
    /*
     *Response Dto'su API'lerde yazılan end-point'lerin dönüş veri tipi olarak kullanılır.Normalde bir end-point işlemini gerçekleştirdikten
     sonra herhangi bir veri tipi dönebilir ve ya hiçbir şey dönmeyebilir. Ancak client tarafını kodlayan bir yazılımcı bu end-point'i 
     kullanacağı için ilgili end-point'in parametreleri ve döneceği veriler anlaşılır olmalıdır. Bu nedenle tüm end-point'ler aynı tip  
     dto'yu dönerse bu dto'nun içine ilgili end-point için varsa hata ve hatanın kodu varsa ilgili Entity eklenerek direkt bu dto'yu dönmek 
     client tarafı için oldukça yararladır. Bu dto'yu projedeki tüm API'ler kullanacağı için shared projesinde oluşturulması mantıklıdır.

    *Generic olmasının nedeni her end-point için kullanabilir olmasıdır. Diyelimki data olarak product entity'si dönülecek o zaman data'nın 
     Product olması gereklidir.

     */
    public class Response<T> where T : class
    {
        public T? Data { get; private set; }

        /*
        status code client'ın anlayacağı kodlardan oluşur. End-point'e koduna göre status kod başarılı ya da başarısız şekilde girilerek 
        Response dönülür.
        */
        public int StatusCode { get; private set; } 


        /*
         Response olarak eğer bir hata dönülmek istenirse oluşturduğumuz ErroDto'yu da response'a ekleyerek hatada dönebiliriz. Bu alan 
        nullable'dır yani her zaman bir end-point hata dönmez ancak hata döndüğü yerlerde vardır.
         */
        public ErrorDto? Error { get; private set; }

        /*
        Örneğin ProductController'da bulunun bir end-point için başarılı bir response dönmek istersek
        return  Response<Product>.Success(Product,2001) şeklinde bir dönüş yapabiliriz. Success methodu direkt olarak static olduğu için
        Response classı üzerinden nesne üretmeden çağırabiliriz. Aynı methodu birden fazla yazıp parametrelerin sayısını farklı vermek 
        o methodu overload etmek demektir. Örnek verecek olursak bir end-point succeed olarak bir response döner ancak data dönmek istemeyebilir
        o zaman success method overload ederek iki veya daha fazla oluşturulabilir.
        */
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode };
        }

        public static Response<T> Success(int statusCode) { 
            return new Response<T> {Data= default, StatusCode = statusCode };
        }

        //Response olarak birden fazla hata mesajı dönülecekse ErrorDto olarak dönülebilir.
        public static Response<T> Fail(int statusCode, ErrorDto error) { 
            return new Response<T> { StatusCode = statusCode, Error = error,  };
        }

        /*
         Fail methodunu overload ettik eğer bir tane hata mesajı var ise bunu string olarak olarak ErrorDto'ya çevirerek Fail response
        static methodunu oluşturduk
        */
        public static Response<T> Fail(int statusCode,  string ErrorMessage ,bool ErrorIsShow)
        {

            return new Response<T> { StatusCode = statusCode, Error = new ErrorDto(ErrorMessage,ErrorIsShow) };
        }

    }
}
