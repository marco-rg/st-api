using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ST.Controllers
{
    [EnableCors(origins: "http://localhost:4200 , http://ctec.support-royalticgroup.com", headers: "*", methods: "*")]
    [RoutePrefix("api/Categorias")]
    public class CategoriasController : ApiController
    {
        // public Models.ModelHealthAdvisor dbContext = new Models.ModelHealthAdvisor();
        /// <summary>
        /// Ge List
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Ge List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public Models.Results GetAll()
        {

            Models.Results resultado = new Models.Results();
            //if (TokenGenerator.HasPermissions(Request, "HA_SAMPLE", "Read"))
            //{
            using (var dbContextSample = new Models.ModelHealthAdvisor())
            {
                resultado.OBJETO = dbContextSample.Categorias.ToList();
                resultado.MENSAJE = "Listado de Sample consultadas exitosamente";
                resultado.STATUS = "success";
                return resultado;

                /*resultado.OBJETO = dbContextSample.Sample.Include("SampleType").Where(t => t.samIsDeleted != true).ToList();
                resultado.MENSAJE = "Successful Sample List";
                resultado.STATUS = "success";
                return resultado;*/
            }
            //}
            //else
            //{
            //    resultado.OBJETO = null;
            //    resultado.MENSAJE = "No authorized";
            //    resultado.STATUS = "unsuccess";
            //    return resultado;
            //}

        }

    }
}
