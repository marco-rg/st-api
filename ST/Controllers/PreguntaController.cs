using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ST.Controllers
{
    //[Authorize]
    [EnableCors(origins: "http://localhost:4200 , https://ctec.sydfast.com , http://ctec.sydfast.com", headers: "*", methods: "*")]
    [RoutePrefix("api/Pregunta")]
    public class PreguntaController : ApiController
    {
        public Models.Pregunta objPregunta = null;
        // public Models.ModelHealthAdvisor dbContext = new Models.ModelHealthAdvisor();

        /// <summary>
        /// List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public Models.Results GetAll()
        {

            Models.Results resultado = new Models.Results();

            using (var dbContextPregunta = new Models.ModelHealthAdvisor())
            {

                resultado.OBJETO = dbContextPregunta.Pregunta.Include("Categorias").Where(t => t.CategoriaId > 0 && t.EstaEliminado == false).ToList();//
                resultado.MENSAJE = "Successful Sample Type List ";
                resultado.STATUS = "success";
                return resultado;
            }




        }



        /// <summary>
        /// Get Smaple Type by Id
        /// </summary>
        /// <param name="id">Sample Type Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get/{id:int}")]
        public Models.Results Get(int id)
        {

            Models.Results resultado = new Models.Results();

            using (var dbContextPregunta = new Models.ModelHealthAdvisor())
            {
                Models.Pregunta objPregunta = null;


                objPregunta = dbContextPregunta.Pregunta.FirstOrDefault((p) => p.PreguntaId == id);
                if (objPregunta != null)
                {
                    resultado.OBJETO = objPregunta;
                    resultado.MENSAJE = "Successful Sample Type";
                    resultado.STATUS = "success";
                    return resultado;
                }
                else
                {

                    resultado.MENSAJE = "Sample Type Id does not exist";
                    resultado.STATUS = "error";
                    return resultado;
                }
            }






        }

        /// <summary>
        /// Create Sample type
        /// </summary>
        /// <param name="_objPregunta">Data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        // POST api/<controller>
        public Models.Results Post([FromBody]Models.Pregunta _objPregunta)
        {

            Models.Results resultado = new Models.Results();


            using (var dbContextPregunta = new Models.ModelHealthAdvisor())
            {

                try
                {
                    if (ModelState.IsValid)
                    {
                        _objPregunta.CreadoAl = System.DateTime.Now;
                        _objPregunta.UserCreatorId = TokenGenerator.GetUserSystem(Request);
                        objPregunta = _objPregunta;
                        dbContextPregunta.Pregunta.Add(_objPregunta);
                        dbContextPregunta.SaveChanges();
                        resultado.OBJETO = objPregunta;
                        resultado.MENSAJE = "Question created";
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
                                if (!String.IsNullOrEmpty(error.ErrorMessage.Trim()))
                                    mensajeDeErrores = mensajeDeErrores + error.ErrorMessage + "." + System.Environment.NewLine;
                                else if (!String.IsNullOrEmpty(error.Exception.Message.Trim()))
                                    mensajeDeErrores = mensajeDeErrores + error.Exception.Message + "." + System.Environment.NewLine;
                                else
                                    mensajeDeErrores = "Uncontrolled error";
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
                                resultado.MENSAJE = "Sample Type cant be deleted.";
                                break; //FK exception
                            case 2601:

                                string value = sqlex.Message.Split('(', ')')[1];
                                resultado.MENSAJE = @"An Sample Type with the same CODE or NAME value already exists. Value is " + value;//primary key exception
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
        /// Update Info
        /// </summary>
        /// <param name="_objMuestra">Sample data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public Models.Results PostUpdate([FromBody]Models.Pregunta _objMuestra)
        {

            Models.Results resultado = new Models.Results();

            using (var dbContextPregunta = new Models.ModelHealthAdvisor())
            {


                try
                {
                    if (ModelState.IsValid)
                    {
                        //fetching and filter specific member id record   
                        objPregunta = (from a in dbContextPregunta.Pregunta where a.PreguntaId == _objMuestra.PreguntaId select a).FirstOrDefault();

                        //checking fetched or not with the help of NULL or NOT.  
                        if (objPregunta != null)
                        {
                            //set received _member object properties with memberdetail  
                            objPregunta.ModificadoAl = System.DateTime.Now;
                            objPregunta.UserModifierId = TokenGenerator.GetUserSystem(Request);
                            objPregunta.CategoriaId = _objMuestra.CategoriaId;                            
                            objPregunta.Peso = _objMuestra.Peso;
                            objPregunta.Descripcion = _objMuestra.Descripcion;
                            objPregunta.CategoriaId = _objMuestra.CategoriaId;
                            //save set allocation.  
                            dbContextPregunta.SaveChanges();
                            resultado.OBJETO = objPregunta;
                            resultado.MENSAJE = "Pregunta Tupe updated";
                            resultado.STATUS = "success";
                            return resultado;

                        }
                        else
                        {

                            resultado.MENSAJE = "Pregunta Id does not exist.";
                            resultado.STATUS = "error";
                            return resultado;
                        }

                    }
                    else
                    {

                        string mensajeDeErrores = "";
                        foreach (var state in ModelState)
                        {
                            foreach (var error in state.Value.Errors)
                            {
                                if (!String.IsNullOrEmpty(error.ErrorMessage.Trim()))
                                    mensajeDeErrores = mensajeDeErrores + error.ErrorMessage + "." + System.Environment.NewLine;
                                else if (!String.IsNullOrEmpty(error.Exception.Message.Trim()))
                                    mensajeDeErrores = mensajeDeErrores + error.Exception.Message + "." + System.Environment.NewLine;
                                else
                                    mensajeDeErrores = "Uncontrolled error";
                            }
                        }

                        resultado.MENSAJE = mensajeDeErrores;
                        resultado.STATUS = "error";
                        return resultado;

                    }

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
        /// Permanently Delete
        /// </summary>
        /// <param name="_objMuestra"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PhysicalDelete")]
        public Models.Results PostDelete([FromBody]Models.Pregunta _objMuestra)
        {
            Models.Results resultado = new Models.Results();

            using (var dbContextPregunta = new Models.ModelHealthAdvisor())
            {

                try
                {

                    //fetching and filter specific member id record   
                    objPregunta = (from a in dbContextPregunta.Pregunta where a.PreguntaId == _objMuestra.PreguntaId select a).FirstOrDefault();
                    //checking fetched or not with the help of NULL or NOT.  
                    if (objPregunta != null)
                    {
                        dbContextPregunta.Pregunta.Remove(objPregunta);
                        dbContextPregunta.SaveChanges();
                        resultado.OBJETO = (from a in dbContextPregunta.Pregunta where a.PreguntaId == _objMuestra.PreguntaId select a).FirstOrDefault();
                        resultado.MENSAJE = "Sample Type deleted";
                        resultado.STATUS = "success";
                        return resultado;
                    }
                    else
                    {

                        resultado.MENSAJE = "Sample Type Id does not exist.";
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
                                resultado.MENSAJE = "Sample Type cant be deleted.";
                                break; //FK exception
                            case 2601:

                                string value = sqlex.Message.Split('(', ')')[1];
                                resultado.MENSAJE = @"An Sample Tupe with the same CODE or NAME value already exists. Value is " + value;//primary key exception
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
        /// Logical Delete
        /// </summary>
        /// <param name="_objMuestra">Sample Type Data Object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("LogicalDelete")]
        public Models.Results PostDeleteLogical([FromBody]Models.Pregunta _objMuestra)
        {
            Models.Results resultado = new Models.Results();

            using (var dbContextPregunta = new Models.ModelHealthAdvisor())
            {

                try
                {


                    //fetching and filter specific member id record   
                    objPregunta = (from a in dbContextPregunta.Pregunta where a.PreguntaId == _objMuestra.PreguntaId select a).FirstOrDefault();
                    //checking fetched or not with the help of NULL or NOT.  
                    if (objPregunta != null)
                    {

                        objPregunta.ModificadoAl = DateTime.Now;
                        objPregunta.UserModifierId = TokenGenerator.GetUserSystem(Request);
                        objPregunta.EstaEliminado = true;

                        // dbContextAnalysis.Entry(objTipoAnalisis).CurrentValues.SetValues(objTipoAnalisis);
                        dbContextPregunta.Pregunta.Attach(objPregunta);
                        dbContextPregunta.Entry(objPregunta).Property(x => x.ModificadoAl).IsModified = true;
                        dbContextPregunta.Entry(objPregunta).Property(x => x.UserModifierId).IsModified = true;
                        dbContextPregunta.Entry(objPregunta).Property(x => x.EstaEliminado).IsModified = true;
                        dbContextPregunta.SaveChanges();

                        resultado.OBJETO = objPregunta;
                        resultado.MENSAJE = "Pregunta deleted";
                        resultado.STATUS = "success";
                        return resultado;
                    }
                    else
                    {

                        resultado.MENSAJE = "Pregunta Id does not exist.";
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
                                resultado.MENSAJE = "Sample Type cant be deleted.";
                                break; //FK exception
                            case 2601:

                                string value = sqlex.Message.Split('(', ')')[1];
                                resultado.MENSAJE = @"An Sample Tupe with the same CODE or NAME value already exists. Value is " + value;//primary key exception
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
