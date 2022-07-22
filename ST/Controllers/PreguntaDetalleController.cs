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
        public Models.PreguntaDetalle objParameter = null;
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
                    .Include("Pregunta")
                    .Where(x => x.PreguntaId == id && x.EstaEliminado != true)
           .Select(p => new
           {
               PreguntaDetalleId = p.PreguntaDetalleId,
               PreguntaId = p.PreguntaId,
               Descripcion = p.Descripcion,
               EsCorrecta = p.EsCorrecta,
               EstaEliminado = p.EstaEliminado,
               Pregunta = p.Pregunta.Descripcion
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
                            qval = "",
                            roc = y.PreguntaDetalle.Where(t=>t.EsCorrecta==true).Count()
                        }).ToList()
                    })
                    .ToList();
                resultado.MENSAJE = "Successful Parameters List";
                resultado.STATUS = "success";
                return resultado;
            }



        }
        /// <summary>
        /// Create Parameter
        /// </summary>
        /// <param name="_objParameter">Parameter Object Data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public Models.Results Post([FromBody]Models.PreguntaDetalle _objParameter)
        {

            Models.Results resultado = new Models.Results();


            using (var dbContextParameter = new Models.ModelHealthAdvisor())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        _objParameter.CreadoAl = System.DateTime.Now;
                        _objParameter.UserCreatorId = TokenGenerator.GetUserSystem(Request);
                        objParameter = _objParameter;
                        dbContextParameter.PreguntaDetalle.Add(_objParameter);
                        dbContextParameter.SaveChanges();
                        resultado.OBJETO = objParameter;
                        resultado.MENSAJE = "Parameter created";
                        resultado.STATUS = "success";
                        return resultado;
                    }
                    else
                    {
                        // ModelState.
                        string mensajeDeErrores = "";
                        foreach (var state in ModelState)
                        {
                            foreach (var error in state.Value.Errors)
                            {
                                mensajeDeErrores = mensajeDeErrores + error.ErrorMessage + "." + System.Environment.NewLine;
                            }
                        }

                        resultado.MENSAJE = mensajeDeErrores;
                        resultado.STATUS = "error";
                        return resultado;

                    }

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {

                    var sqlex = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;

                    if (sqlex != null)
                    {
                        switch (sqlex.Number)
                        {
                            case 547:
                                resultado.MENSAJE = "Parámetro cant be eliminated.";
                                break; //FK exception
                            case 2601:
                                string value = sqlex.Message.Split('(', ')')[1];
                                resultado.MENSAJE = @"An Parameter with the same CODE or NAME value already exists. Value is " + value;//primary key exception
                                break;
                            case 2627:
                            default: resultado.MENSAJE = ex.Message; break;//otra excepcion que no controlo.
                        }
                    }

                    resultado.STATUS = "error";
                    return resultado;
                }
            }

        }

        /// <summary>
        /// Update Parameter Controller
        /// </summary>
        /// <param name="_objParameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public Models.Results PostUpdate([FromBody]Models.PreguntaDetalle _objParameter)
        {
            Models.Results resultado = new Models.Results();


            using (var dbContextParameter = new Models.ModelHealthAdvisor())
            {
                try
                {
                    //if (ModelState.IsValid)
                    //{
                    //fetching and filter specific member id record   
                    objParameter = (from a in dbContextParameter.
                                    PreguntaDetalle
                                    .Include("Pregunta")
                                    where a.PreguntaDetalleId == _objParameter.PreguntaDetalleId
                                    select a).FirstOrDefault();

                    //checking fetched or not with the help of NULL or NOT.  
                    if (objParameter != null)
                    {
                        //set received _member object properties with memberdetail  
                        _objParameter.CreadoAl = objParameter.CreadoAl;
                        _objParameter.UserCreatorId = objParameter.UserCreatorId;
                        _objParameter.ModificadoAl = System.DateTime.Now;
                        _objParameter.UserModifierId = TokenGenerator.GetUserSystem(Request);
                        objParameter.Descripcion = _objParameter.Descripcion;                        
                        objParameter.EsCorrecta = _objParameter.EsCorrecta;
                        // Update parent
                        dbContextParameter.Entry(objParameter).CurrentValues.SetValues(_objParameter);


                        ///******************************************************************************/
                        ///***************************** PARAMETER VALUE *********************************/
                        ///******************************************************************************/


                        //List<long> previousIds = dbContextParameter.Parameter.AsNoTracking().Include("ParameterValue").FirstOrDefault(ff => ff.parId == _objParameter.parId).ParameterValue.Select(t => t.pvalueId).ToList();
                        //List<long> currentIds = _objParameter.ParameterValue.Select(t => t.pvalueId).ToList();
                        //List<long> deletedIds = previousIds
                        //    .Except(currentIds).ToList();



                        //foreach (var deletedId in deletedIds)
                        //{
                        //    Models.ParameterValue caracteristicas = dbContextParameter.ParameterValue
                        //        .Single(od => od.pvalueParameterId == _objParameter.parId && od.pvalueParameterId == deletedId);
                        //    //dbContextSite.Entry(caracteristicas).State = System.Data.Entity.EntityState.Deleted;
                        //    dbContextParameter.ParameterValue.Remove(caracteristicas);

                        //}

                        //foreach (var caracteristica in _objParameter.ParameterValue)
                        //{
                        //    var existingChild = objParameter.ParameterValue
                        //            .Where(c => c.pvalueParameterId == caracteristica.pvalueParameterId && c.pvalueParameterId != default(int))
                        //            .SingleOrDefault();

                        //    if (existingChild != null)
                        //    {
                        //        //caracteristica.modi = DateTime.Now;
                        //        //caracteristica.LastModifierID = TokenGenerator.GetUserSystem(Request);
                        //        dbContextParameter.Entry(existingChild).CurrentValues.SetValues(caracteristica);

                        //    }
                        //    else
                        //    {
                        //        // Insert child
                        //        var newChild = new Models.ParameterValue()
                        //        {
                        //            Parameter = caracteristica.Parameter,
                        //            pvalueParameterId = caracteristica.pvalueParameterId,
                        //            pvalueInfo = caracteristica.pvalueInfo
                        //        };
                        //        objParameter.ParameterValue.Add(newChild);
                        //    }
                        //}




                        ///******************************************************************************/
                        ///***************************** FormulaParameter *********************************/
                        ///******************************************************************************/



                        //List<long> previousIds2 = null;
                        //var formsParams = from pv in objParameter.ParameterValue
                        //                  where pv.FormulaParameter.Any()
                        //                  select pv;


                        //foreach (var fom in formsParams)
                        //    previousIds2 = fom.FormulaParameter.Select(t => t.formpId).ToList();



                        //List<long> currentIds2 = null;
                        //var formsParamsCurrent = from pv in _objParameter.ParameterValue
                        //                         where pv.FormulaParameter.Any()
                        //                         select pv;


                        //foreach (var fom in formsParamsCurrent)
                        //    currentIds2 = fom.FormulaParameter.Select(t => t.formpId).ToList();



                        //List<long> deletedIds2 = previousIds2
                        //    .Except(currentIds2).ToList();



                        //foreach (var deletedId2 in deletedIds2)
                        //{
                        //    foreach (var fom in formsParams)
                        //    {
                        //        //  Models.FormulaParameter caracteristicas = dbContextParameter.FormulaParameter
                        //        //.Single(od => od.pvalueParameterId == _objParameter.parId && od.formpId == deletedId2);
                        //        //  dbContextParameter.ParameterValue.Remove(caracteristicas);
                        //    }
                        //}

                        //foreach (var caracteristica in _objParameter.ParameterValue)
                        //{
                        //    var existingChild = objParameter.ParameterValue
                        //            .Where(c => c.pvalueParameterId == caracteristica.pvalueParameterId && c.pvalueParameterId != default(int))
                        //            .SingleOrDefault();

                        //    if (existingChild != null)
                        //    {
                        //        //caracteristica.modi = DateTime.Now;
                        //        //caracteristica.LastModifierID = TokenGenerator.GetUserSystem(Request);
                        //        dbContextParameter.Entry(existingChild).CurrentValues.SetValues(caracteristica);

                        //    }
                        //    else
                        //    {
                        //        // Insert child
                        //        var newChild = new Models.ParameterValue()
                        //        {
                        //            Parameter = caracteristica.Parameter,
                        //            pvalueParameterId = caracteristica.pvalueParameterId,
                        //            pvalueInfo = caracteristica.pvalueInfo
                        //        };
                        //        objParameter.ParameterValue.Add(newChild);
                        //    }
                        //}





                        //save set allocation.  
                        dbContextParameter.SaveChanges();
                        resultado.OBJETO = objParameter;
                        resultado.MENSAJE = "Parámetro actualizada exitosamente";
                        resultado.STATUS = "success";
                        return resultado;
                    }
                    else
                    {

                        resultado.MENSAJE = "No existe parámetro con el Identificador indicado";
                        resultado.STATUS = "error";
                        return resultado;
                    }
                    //}
                    //else
                    //{

                    //    string mensajeDeErrores = "";
                    //    foreach (var state in ModelState)
                    //    {
                    //        foreach (var error in state.Value.Errors)
                    //        {
                    //            mensajeDeErrores = mensajeDeErrores + error.ErrorMessage + "." + System.Environment.NewLine;
                    //        }
                    //    }

                    //    resultado.MENSAJE = mensajeDeErrores;
                    //    resultado.STATUS = "error";
                    //    return resultado;

                    //}

                }
                catch (Exception ex)
                {
                    resultado.MENSAJE = ex.Message;
                    resultado.STATUS = "error";
                    return resultado;
                }
            }

        }

        /// <summary>
        /// Logical Delete
        /// </summary>
        /// <param name="_objParameter">Parameter Data Object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("LogicalDelete")]
        public Models.Results PostDeleteLogical([FromBody]Models.PreguntaDetalle _objParameter)
        {
            Models.Results resultado = new Models.Results();

            using (var dbContextSampleType = new Models.ModelHealthAdvisor())
            {

                try
                {
                    //fetching and filter specific member id record   
                    objParameter = (from a in dbContextSampleType.PreguntaDetalle where a.PreguntaDetalleId == _objParameter.PreguntaDetalleId select a).FirstOrDefault();
                    //checking fetched or not with the help of NULL or NOT.  
                    if (objParameter != null)
                    {

                        //objParameter.Eli = DateTime.Now;
                        //objParameter.U = TokenGenerator.GetUserSystem(Request);
                        objParameter.EstaEliminado = true;

                        // dbContextAnalysis.Entry(objTipoAnalisis).CurrentValues.SetValues(objTipoAnalisis);
                        dbContextSampleType.PreguntaDetalle.Attach(objParameter);
                        dbContextSampleType.Entry(objParameter).Property(x => x.EstaEliminado).IsModified = true;
                        //dbContextSampleType.Entry(objParameter).Property(x => x.parDeletionTime).IsModified = true;
                        //dbContextSampleType.Entry(objParameter).Property(x => x.parDeleterUserId).IsModified = true;
                        dbContextSampleType.SaveChanges();

                        resultado.OBJETO = objParameter;
                        resultado.MENSAJE = "Parameter borrado exitosamente";
                        resultado.STATUS = "success";
                        return resultado;
                    }
                    else
                    {

                        resultado.MENSAJE = "No existe Parameter  con el Identificador indicado";
                        resultado.STATUS = "error";
                        return resultado;
                    }




                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {

                    var sqlex = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;

                    if (sqlex != null)
                    {
                        switch (sqlex.Number)
                        {
                            case 547:
                                resultado.MENSAJE = "Parameter  no puede ser eliminada.";
                                break; //FK exception
                            case 2601:
                                resultado.MENSAJE = "Ya existe un Parameter con el mismo valor de código o nombre.";//primary key exception
                                break;
                            case 2627:
                            default: resultado.MENSAJE = ex.Message; break;//otra excepcion que no controlo.
                        }
                    }

                    resultado.STATUS = "error";
                    return resultado;
                }
            }

        }

    }
}
