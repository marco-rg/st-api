using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ST.Controllers
{
    [EnableCors(origins: "http://localhost:4200 , http://ctec.support-royalticgroup.com", headers: "*", methods: "*")]
    [RoutePrefix("api/Locales")]
    public class LocalesController : ApiController
    {
        public Models.LocalesNacionales objLocalesNacionales = null;
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
            //if (TokenGenerator.HasPermissions(Request, "HA_SAMPLE_LOCATION_INSTANCE", "Read"))
            //{
            using (var dbContextLocalesNacionales = new Models.ModelHealthAdvisor())
            {
                var consulta = dbContextLocalesNacionales.LocalesNacionales;
                resultado.OBJETO = consulta.ToList();
                resultado.MENSAJE = "Successful Locales Nacionales";
                resultado.STATUS = "success";
                return resultado;
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

            using (var dbContextLocalesNacionales = new Models.ModelHealthAdvisor())
            {
                Models.LocalesNacionales objLocalesNacionales = null;


                objLocalesNacionales = dbContextLocalesNacionales.LocalesNacionales.FirstOrDefault((p) => p.CodigoLocal == id);
                if (objLocalesNacionales != null)
                {
                    resultado.OBJETO = objLocalesNacionales;
                    resultado.MENSAJE = "Successful Locales Nacionales";
                    resultado.STATUS = "success";
                    return resultado;
                }
                else
                {

                    resultado.MENSAJE = "Sample Location Type Id does not exist";
                    resultado.STATUS = "error";
                    return resultado;
                }
            }

        }


        /// <summary>
        /// Get Smaple Type by Id
        /// </summary>
        /// <param name="id">Sample Type Id</param>
        /// <param name="slinId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSampleLocationSite/{id:int}/{slinId:int}")]
        public Models.Results GetSampleLocationSite(String id, int slinId)
        {

            Models.Results resultado = new Models.Results();

            using (var dbContextLocalesNacionales = new Models.ModelHealthAdvisor())
            {
                Models.LocalesNacionales LocalesNacionalesSite = null;


                LocalesNacionalesSite = dbContextLocalesNacionales.LocalesNacionales.FirstOrDefault((p) => p.FormatoLocal == id && p.CodigoLocal == slinId);
                if (LocalesNacionalesSite != null)
                {
                    resultado.OBJETO = LocalesNacionalesSite;
                    resultado.MENSAJE = "Successful Sample Location Type";
                    resultado.STATUS = "success";
                    return resultado;
                }
                else
                {

                    resultado.MENSAJE = "Sample Location Type Id does not exist";
                    resultado.STATUS = "error";
                    return resultado;
                }
            }

        }

        /// <summary>
        /// Create Sample type
        /// </summary>
        /// <param name="_objLocalesNacionales">Data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        // POST api/<controller>
        public Models.Results Post([FromBody]Models.LocalesNacionales _objLocalesNacionales)
        {

            Models.Results resultado = new Models.Results();
            //if (TokenGenerator.HasPermissions(Request, "HA_SAMPLE_LOCATION_INSTANCE", "Create"))
            //{

            using (var dbContextLocalesNacionales = new Models.ModelHealthAdvisor())
            {

                try
                {
                    if (ModelState.IsValid)
                    {
                        //_objLocalesNacionales.slinCreationTime = System.DateTime.Now;
                        //_objLocalesNacionales.slinCreatorUserId = TokenGenerator.GetUserSystem(Request);
                        objLocalesNacionales = _objLocalesNacionales;
                        dbContextLocalesNacionales.LocalesNacionales.Add(_objLocalesNacionales);
                        dbContextLocalesNacionales.SaveChanges();
                        resultado.OBJETO = objLocalesNacionales;
                        resultado.MENSAJE = "Sample Location Type created";
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
                                resultado.MENSAJE = "Sample Location Type cant be deleted.";
                                break; //FK exception
                            case 2601:

                                string value = sqlex.Message.Split('(', ')')[1];
                                resultado.MENSAJE = @"An Sample Location Type with the same CODE or NAME value already exists. Value is " + value;//primary key exception
                                break;

                            case 2627:
                            default: resultado.MENSAJE = ex.Message; break;//otra excepcion que no controlo.
                        }
                    }

                    resultado.STATUS = "error";
                    return resultado;
                }
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

        /// <summary>
        /// Update Info
        /// </summary>
        /// <param name="_objMuestra">Sample data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public Models.Results PostUpdate([FromBody]Models.LocalesNacionales _objMuestra)
        {

            Models.Results resultado = new Models.Results();

            using (var dbContextLocalesNacionales = new Models.ModelHealthAdvisor())
            {

                try
                {
                    if (ModelState.IsValid)
                    {
                        //fetching and filter specific member id record   
                        objLocalesNacionales = (from a in dbContextLocalesNacionales.LocalesNacionales where a.CodigoLocal == _objMuestra.CodigoLocal select a).FirstOrDefault();

                        //checking fetched or not with the help of NULL or NOT.  
                        if (objLocalesNacionales != null)
                        {
                            //set received _member object properties with memberdetail  
                            objLocalesNacionales.Zona = _objMuestra.Zona;
                            objLocalesNacionales.Ciudad = _objMuestra.Ciudad;
                            objLocalesNacionales.NombreLocal = _objMuestra.NombreLocal;
                            objLocalesNacionales.Provincia = _objMuestra.Provincia;
                            objLocalesNacionales.Latitud = _objMuestra.Latitud;
                            objLocalesNacionales.Longitud = _objMuestra.Longitud;
                            objLocalesNacionales.FormatoLocal = _objMuestra.FormatoLocal;
                            //save set allocation.  
                            dbContextLocalesNacionales.SaveChanges();
                            resultado.OBJETO = objLocalesNacionales;
                            resultado.MENSAJE = "Sample Location Type Updated";
                            resultado.STATUS = "success";
                            return resultado;

                        }
                        else
                        {

                            resultado.MENSAJE = "ample Location Type Id does not exist";
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
                                mensajeDeErrores = mensajeDeErrores + error.ErrorMessage + "." + System.Environment.NewLine;
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
        public Models.Results PostDelete([FromBody]Models.LocalesNacionales _objMuestra)
        {
            Models.Results resultado = new Models.Results();

            using (var dbContextLocalesNacionales = new Models.ModelHealthAdvisor())
            {

                try
                {

                    //fetching and filter specific member id record   
                    objLocalesNacionales = (from a in dbContextLocalesNacionales.LocalesNacionales where a.CodigoLocal == _objMuestra.CodigoLocal select a).FirstOrDefault();
                    //checking fetched or not with the help of NULL or NOT.  
                    if (objLocalesNacionales != null)
                    {
                        dbContextLocalesNacionales.LocalesNacionales.Remove(objLocalesNacionales);
                        dbContextLocalesNacionales.SaveChanges();
                        resultado.OBJETO = (from a in dbContextLocalesNacionales.LocalesNacionales where a.CodigoLocal == _objMuestra.CodigoLocal select a).FirstOrDefault();
                        resultado.MENSAJE = "Sample Location Type deleted";
                        resultado.STATUS = "success";
                        return resultado;
                    }
                    else
                    {

                        resultado.MENSAJE = "Sample Location Id does not exist";
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
                                resultado.MENSAJE = "Sample Location Type cant be deleted.";
                                break; //FK exception
                            case 2601:
                                string value = sqlex.Message.Split('(', ')')[1];
                                resultado.MENSAJE = @"An Sample Location Type with the same CODE or NAME value already exists. Value is " + value;//primary key exception
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
        public Models.Results PostDeleteLogical([FromBody]Models.LocalesNacionales _objMuestra)
        {
            Models.Results resultado = new Models.Results();
            using (var dbContextLocalesNacionales = new Models.ModelHealthAdvisor())
            {

                try
                {


                    //fetching and filter specific member id record   
                    objLocalesNacionales = (from a in dbContextLocalesNacionales.LocalesNacionales where a.CodigoLocal  == _objMuestra.CodigoLocal select a).FirstOrDefault();
                    //checking fetched or not with the help of NULL or NOT.  
                    if (objLocalesNacionales != null)
                    {

                        /*objLocalesNacionales.slinDeletionTime = DateTime.Now;
                        objLocalesNacionales.slinDeleterUserId = TokenGenerator.GetUserSystem(Request);
                        objLocalesNacionales.slinIsDeleted = true;*/

                        // dbContextAnalysis.Entry(objTipoAnalisis).CurrentValues.SetValues(objTipoAnalisis);
                        dbContextLocalesNacionales.LocalesNacionales.Attach(objLocalesNacionales);
                        /*dbContextLocalesNacionales.Entry(objLocalesNacionales).Property(x => x.slinIsDeleted).IsModified = true;
                        dbContextLocalesNacionales.Entry(objLocalesNacionales).Property(x => x.slinDeletionTime).IsModified = true;
                        dbContextLocalesNacionales.Entry(objLocalesNacionales).Property(x => x.slinDeleterUserId).IsModified = true;*/
                        dbContextLocalesNacionales.SaveChanges();

                        resultado.OBJETO = objLocalesNacionales;
                        resultado.MENSAJE = "Sample Location Type deleted";
                        resultado.STATUS = "success";
                        return resultado;
                    }
                    else
                    {

                        resultado.MENSAJE = "Sample Location Id does not exist";
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
                                resultado.MENSAJE = "Sample Location Type cant be deleted.";
                                break; //FK exception
                            case 2601:
                                string value = sqlex.Message.Split('(', ')')[1];
                                resultado.MENSAJE = @"An Sample Location Type with the same CODE or NAME value already exists. Value is " + value;//primary key exception
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
        /// Ge List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AllFormat")]
        public Models.Results GetAllFormat()
        {

            Models.Results resultado = new Models.Results();
            
            using (var dbContextSampleLocation = new Models.ModelHealthAdvisor())
            {                
                resultado.OBJETO = dbContextSampleLocation.LocalesNacionales.Select(p=>new { p.FormatoLocal }).Distinct().ToList();
                resultado.MENSAJE = "Successful Sample Location List";
                resultado.STATUS = "success";
                return resultado;
            }            
        }
        /// <summary>
        /// Ge List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AllCity")]
        public Models.Results GetAllCity()
        {

            Models.Results resultado = new Models.Results();

            using (var dbContextSampleLocation = new Models.ModelHealthAdvisor())
            {
                resultado.OBJETO = dbContextSampleLocation.LocalesNacionales.Select(p => new { p.Ciudad }).Distinct().ToList();
                resultado.MENSAJE = "Successful Sample Location List";
                resultado.STATUS = "success";
                return resultado;
            }
        }

        [HttpGet]
        [Route("LocalEntities")]
        public Models.Results SamplingEntities()
        {
            Models.Results resultado = new Models.Results();
            //if (TokenGenerator.HasPermissions(Request, "IC_ALARM_RESPONSABLE", "Read"))
            //{
            using (var _context = new Models.ModelHealthAdvisor())
            {
                var username = TokenGenerator.GetUserSystem(Request);
                var usuario = _context.UserExternal.Where(t => t.Username.Equals(username)).FirstOrDefault();
                if (usuario != null)
                {
                    //string[] listCompanys = usuario.usrCompanyList.Split('|');
                    //var listCompanyIds = listCompanys.Select(x => long.Parse(x)).ToList();


                    var listado = _context.LocalesNacionales
                        /*.Include("Site")
                        .Include("Site.Unit")
                        .Include("Site.SampleLocationInstance")
                        .Where(x => listCompanyIds.Contains(x.CompAquasymId) && x.cmpActive == true && x.IsDeleted != true)*/

                         .Select(X => new
                         {
                             X.CodigoLocal,
                             NombreLocal= string.Concat(X.CodigoLocal, "-", X.NombreLocal),
                             HistorialDetalle = _context.Encuestas//.Include("Meta")
                                 .Where(t => t.CodigoLocal == X.CodigoLocal)
                                 .Select(y => new
                                 {
                                     y.EncuestaId,
                                     y.CodigoLocal,
                                     y.LocalesNacionales.NombreLocal,
                                     y.CreadoAl,
                                     y.CalificacionMarca,
                                     y.CalificacionTelefono
                                 }).OrderByDescending(c => c.CreadoAl).Take(3).ToList()/*,
                             Site = X.Site.Where(a => a.IsDeleted != true).Select(y => new
                             {
                                 y.SitAquasymId,
                                 y.sitName,
                                 SkrettingSectorId = _context.SkrettingSector.Where(z => z.skseId == y.sitSkrettingSectorId).Select(w => (long?)w.skseId).FirstOrDefault(),
                                 SkrettingSectorName = _context.SkrettingSector.Where(z => z.skseId == y.sitSkrettingSectorId).Select(w => w.skseName).FirstOrDefault(),

                                 //Unit = y.Unit.Where(t => t.ProductionGroup.Any(p => p.fgIsClosed == false) && t.IsDeleted != true)
                                 //                                                                             .Select(z => new { z.UniAquasimId, z.uniName }),

                                 //Para traer todas las piscinas
                                 Unit = y.Unit.Where(t => t.IsDeleted != true)
                                                                                                                //Unit = y.Unit.Where(t => t.UniHasPGOpened == true && t.IsDeleted != true)    
                                                                                                                .Select(z => new { z.UniAquasimId, z.uniName, unitActive = z.UniHasPGOpened }),
                                 SampleLocationInstance = y.SampleLocationInstance.Where(b => b.slinIsDeleted != true).Select(t => new { t.slinId, t.slinKey, t.slinName, t.slinDescription })
                             })*/
                         })

                        .ToList();

                    resultado.OBJETO = listado;
                    resultado.MENSAJE = "";
                    resultado.STATUS = "success";
                }
            }
            return resultado;
        }
    }
}
