using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ST.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [RoutePrefix("api/PreguntaDetalle")]
    public class PreguntaDetalleController : ApiController
    {
        /// <summary>
        /// Get List Parameters by Analysis by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListAnswerByQuestion/{id:int}")]
        public Models.Results GetListAnswerByQuestion(int id)
        {

            Models.Results resultado = new Models.Results();


            using (var dbContextParameter = new Models.ModelHealthAdvisor())
            {
                // dbContextParameter.Configuration.LazyLoadingEnabled = false;
                var parameterList = dbContextParameter.PreguntaDetalle
                    .Where(x => x.PreguntaId == id && x.EstaEliminado != true)
           .Select(p => new
           {
               PreguntaDetalleId = p.PreguntaDetalleId,
               PreguntaId = p.PreguntaId,
               Descripcion = p.Descripcion,
               EsCorrecta = p.EsCorrecta,
               EstaEliminado = p.EstaEliminado
               //Value = p.ParameterValue.Select(x => x.pvalueInfo).FirstOrDefault()
           })
           .ToList();

                /*List<Models.IF_PARAM_LIST> listParams = new List<Models.IF_PARAM_LIST>();
                foreach (var item in parameterList)
                {
                    Models.IF_PARAM_LIST objParam = new Models.IF_PARAM_LIST();
                    objParam.parId = item.parId;
                    objParam.parSequence = item.parSequence;
                    objParam.parCode = item.parCode;
                    objParam.parName = item.parName;
                    objParam.parIsManualType = item.parIsManualType;
                    objParam.parIsFixedType = item.parIsFixedType;
                    objParam.parIsFormulaType = item.parIsFormulaType;
                    objParam.parIsRangeType = item.parIsRangeType;
                    objParam.parIsEditable = item.parIsEditable;
                    objParam.parIsRequired = item.parIsRequired;
                    objParam.pvalueFactors = item.pvalueFactors;
                    objParam.parShowInReport = item.parShowInReport;
                    objParam.parHidden = item.parHidden;
                    objParam.parRangeMax = item.parRangeMax;
                    objParam.parRangeMin = item.parRangeMin;
                    objParam.parShowInGraphic = item.parShowInGraphic;
                    
                        objParam.Formula = null;

                    listParams.Add(objParam);
                }*/


                resultado.OBJETO = parameterList;
                resultado.MENSAJE = "Successful Parameter by Analysis";
                resultado.STATUS = "success";
                return resultado;
            }


        }


        /// <summary>
        /// List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AllGetAllAnswer")]
        public Models.Results GetAllAnswer()
        {
            Models.Results resultado = new Models.Results();

            using (var dbContextParameter = new Models.ModelHealthAdvisor())
            {
                resultado.OBJETO = dbContextParameter.Categorias
                    .Include("Pregunta")
                    .AsNoTracking()//.Where(t => t.EstaEliminado != true)
                    .Select(x => new
                    {
                        group = x.CategoriaId,                        
                        items = x.Pregunta.Select(y => new
                        {                            
                            name = y.Descripcion,
                            fname = y.PreguntaId,
                            qty = y.Peso,
                            source = y.PreguntaDetalle.Select(ñ => new
                            {
                                id = ñ.PreguntaDetalleId,
                                name = ñ.Descripcion,
                                state = ñ.EsCorrecta
                            }).ToList(),
                            fval = "",
                            qval = ""
                        }).ToList()
                    })
                    .ToList();
                resultado.MENSAJE = "Successful Parameters List";
                resultado.STATUS = "success";
                return resultado;
            }



        }

    }
}
